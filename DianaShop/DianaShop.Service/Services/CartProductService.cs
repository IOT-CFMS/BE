using AutoMapper;
using DianaShop.Data.Entities;
using DianaShop.Repository.UnitOfWork;
using DianaShop.Service.Interfaces;
using DianaShop.Service.ReponseModel;
using DianaShop.Service.RequestModel;
using Google;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianaShop.Service.Services
{
    public class CartProductService : ICartProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CartProductService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<CartProductResponse> AddProductToCart(CartProductRequest request)
        {
            if (request == null || request.CartId <= 0 || request.ProductId <= 0 || request.Quantity <= 0)
                throw new ArgumentException("Invalid request data.");

            // Kiểm tra Cart có tồn tại không
            var cart = await _unitOfWork.Repository<Cart>().FindAsync(c => c.Id == request.CartId);
            if (cart == null)
                throw new KeyNotFoundException($"Cart with ID {request.CartId} not found.");

            // Kiểm tra Product có tồn tại không
            var product = await _unitOfWork.Repository<Product>().FindAsync(p => p.Id == request.ProductId);
            if (product == null)
                throw new KeyNotFoundException($"Product with ID {request.ProductId} not found.");

            // Kiểm tra sản phẩm đã có trong giỏ chưa
            var existingCartProduct = await _unitOfWork.Repository<CartProduct>()
                .FindAsync(cp => cp.CartId == request.CartId && cp.ProductId == request.ProductId);

            if (existingCartProduct != null)
            {
                existingCartProduct.Quantity += request.Quantity;
                _unitOfWork.Repository<CartProduct>().UpdateAsync(existingCartProduct);
            }
            else
            {
                var cartProduct = new CartProduct
                {
                    CartId = request.CartId,
                    ProductId = request.ProductId,
                    Quantity = request.Quantity
                };
                await _unitOfWork.Repository<CartProduct>().InsertAsync(cartProduct);
            }

            await _unitOfWork.SaveChangesAsync();

            // Lấy lại thông tin CartProduct sau khi thêm/cập nhật
            var result = await _unitOfWork.Repository<CartProduct>()
                .FindAsync(cp => cp.CartId == request.CartId && cp.ProductId == request.ProductId);

            return _mapper.Map<CartProductResponse>(result);
        }

        public async Task<IEnumerable<CartProductResponse>> GetCartProducts(int cartId)
        {
            var cartProducts = await _unitOfWork.Repository<CartProduct>()
                .GetQueryable()
                .Where(cp => cp.CartId == cartId)
                .Include(cp => cp.Product)
                .ToListAsync();
            return _mapper.Map<IEnumerable<CartProductResponse>>(cartProducts);
        }

        public async Task<CartProductResponse> UpdateCartProduct(int cartId, int productId, int quantity)
        {
            var cartProduct = await _unitOfWork.Repository<CartProduct>()
                .GetQueryable()
                .Where(cp => cp.CartId == cartId && cp.ProductId == productId)
                .Include(cp => cp.Product) 
                .FirstOrDefaultAsync();

            if (cartProduct == null)
                throw new Exception("CartProduct not found");

            cartProduct.Quantity = quantity;

            await _unitOfWork.Repository<CartProduct>().UpdateAsync(cartProduct);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<CartProductResponse>(cartProduct);
        }

        public async Task<bool> RemoveProductFromCart(int cartId, int productId)
        {
            var cartProduct = await _unitOfWork.Repository<CartProduct>()
                .FindAsync(cp => cp.CartId == cartId && cp.ProductId == productId);

            if (cartProduct == null)
                return false;

            _unitOfWork.Repository<CartProduct>().Delete(cartProduct);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ClearCart(int cartId)
        {
            var cartProducts = _unitOfWork.Repository<CartProduct>()
                .GetQueryable()
                .Where(cp => cp.CartId == cartId);

            if (!await cartProducts.AnyAsync())
                return false;

            _unitOfWork.Repository<CartProduct>().DeleteRange(cartProducts);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
