using AutoMapper;
using AutoMapper.QueryableExtensions;
using DianaShop.Data.Entities;
using DianaShop.Repository.UnitOfWork;
using DianaShop.Service.Interfaces;
using DianaShop.Service.ReponseModel;
using DianaShop.Service.RequestModel;
using Google.Apis.Storage.v1.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianaShop.Service.Services
{
    public class CartService : ICartService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CartService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // 🛒 Lấy danh sách tất cả giỏ hàng
        public async Task<IEnumerable<CartResponseModel>> GetAllCarts()
        {
            var carts = await _unitOfWork.Repository<Cart>()
                .GetQueryable()
                .ProjectTo<CartResponseModel>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return carts;
        }
        // 🛒 Tạo giỏ hàng mới cho User
        public async Task<CartResponseModel> CreateCart(CartRequestModel request)
        {
            var cart = new Cart
            {
                UserID = request.UserID,
                Quantity = 0,
                CartProducts = new List<CartProduct>()
            };

            await _unitOfWork.Repository<Cart>().InsertAsync(cart);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<CartResponseModel>(cart);
        }

        // 🔍 Lấy thông tin giỏ hàng theo UserID
        public async Task<CartResponseModel> GetCartByUser(int userId)
        {
            var cart = await _unitOfWork.Repository<Cart>()
                .GetQueryable()
                .Where(c => c.UserID == userId)
                .ProjectTo<CartResponseModel>
                (_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();

            if (cart == null)
                throw new KeyNotFoundException("Cart not found for user with ID: " + userId);

            return _mapper.Map<CartResponseModel>(cart);
        }

        /// 🛒 Tạo giỏ hàng mới cho User
        public async Task<CartResponseModel> CreateCarts(CartRequestModel request)
        {
            if (request == null || request.UserID <= 0)
                throw new ArgumentException("Invalid request data. UserID must be greater than 0.");

            var existingCart = await _unitOfWork.Repository<Cart>()
                .FindAsync(c => c.UserID == request.UserID);

            if (existingCart != null)
                throw new InvalidOperationException("User already has an existing cart.");

            var cart = new Cart
            {
                UserID = request.UserID,
                Quantity = 0,
                CartProducts = new List<CartProduct>()
            };

            await _unitOfWork.Repository<Cart>().InsertAsync(cart);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<CartResponseModel>(cart);
        }

        // ➕ Thêm sản phẩm vào giỏ hàng
        public async Task<CartResponseModel> AddProductToCart(int userId, int productId, int quantity)
        {
            var cart = await _unitOfWork.Repository<Cart>()
                .FindAsync(c => c.UserID == userId);

            if (cart == null)
                throw new Exception("Cart not found");

            var product = await _unitOfWork.Repository<Product>().GetById(productId);
            if (product == null)
                throw new Exception("Product not found");

            var cartProduct = await _unitOfWork.Repository<CartProduct>()
                .FindAsync(cp => cp.CartId == cart.Id && cp.ProductId == productId);

            if (cartProduct != null)
            {
                cartProduct.Quantity += quantity;
                await _unitOfWork.Repository<CartProduct>().UpdateAsync(cartProduct);
            }
            else
            {
                cartProduct = new CartProduct
                {
                    CartId = cart.Id,
                    ProductId = productId,
                    Quantity = quantity
                };
                await _unitOfWork.Repository<CartProduct>().InsertAsync(cartProduct);
            }

            cart.Quantity += quantity;
            await _unitOfWork.Repository<Cart>().UpdateAsync(cart);

            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<CartResponseModel>(cart);
        }

        // ✏️Cập nhật số lượng sản phẩm trong giỏ hàng
        public async Task<CartResponseModel> UpdateCartProduct(int userId, int productId, int quantity)
        {
            var cart = await _unitOfWork.Repository<Cart>()
                .FindAsync(c => c.UserID == userId);

            if (cart == null)
                throw new Exception("Cart not found");

            var cartProduct = await _unitOfWork.Repository<CartProduct>()
                .FindAsync(cp => cp.CartId == cart.Id && cp.ProductId == productId);

            if (cartProduct == null)
                throw new Exception("Product not found in cart");

            cart.Quantity -= cartProduct.Quantity; // Trừ số lượng cũ
            cartProduct.Quantity = quantity;
            cart.Quantity += quantity; // Cộng số lượng mới

            await _unitOfWork.Repository<CartProduct>().UpdateAsync(cartProduct);
            await _unitOfWork.Repository<Cart>().UpdateAsync(cart);

            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<CartResponseModel>(cart);
        }

        // ❌ Xóa sản phẩm khỏi giỏ hàng
        public async Task<CartResponseModel> RemoveProductFromCart(int userId, int productId)
        {
            var cart = await _unitOfWork.Repository<Cart>()
                .FindAsync(c => c.UserID == userId);

            if (cart == null)
                throw new Exception("Cart not found");

            var cartProduct = await _unitOfWork.Repository<CartProduct>()
                .FindAsync(cp => cp.CartId == cart.Id && cp.ProductId == productId);

            if (cartProduct == null)
                throw new Exception("Product not found in cart");

            cart.Quantity -= cartProduct.Quantity;
            _unitOfWork.Repository<CartProduct>().Delete(cartProduct);
            await _unitOfWork.Repository<Cart>().UpdateAsync(cart);

            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<CartResponseModel>(cart);
        }

        // 🗑️ Xóa toàn bộ giỏ hàng
        public async Task<bool> ClearCart(int userId)
        {
            var cart = await _unitOfWork.Repository<Cart>()
                .FindAsync(c => c.UserID == userId);

            if (cart == null)
                return false;

            var cartProducts = _unitOfWork.Repository<CartProduct>()
                .GetQueryable()
                .Where(cp => cp.CartId == cart.Id);

            if (!await cartProducts.AnyAsync())
                return false;

            _unitOfWork.Repository<CartProduct>().DeleteRange(cartProducts);
            cart.Quantity = 0;
            await _unitOfWork.Repository<Cart>().UpdateAsync(cart);

            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        //public async Task<bool> Checkout(int userId)
        //{
        //    // Lấy thông tin giỏ hàng của User
        //    var cart = await _unitOfWork.Repository<Cart>()
        //        .GetQueryable()
        //        .Where(c => c.UserID == userId)
        //        .Include(c => c.CartProducts)         // Include danh sách CartProducts
        //            .ThenInclude(cp => cp.Product)    // Include tiếp Product bên trong CartProduct
        //        .FirstOrDefaultAsync();

        //    if (cart == null || !cart.CartProducts.Any())
        //        throw new Exception("Cart is empty or not found");

        //    // Tính tổng số lượng và tổng tiền đơn hàng
        //    var totalAmount = cart.CartProducts.Sum(cp => cp.Quantity * cp.Product.Price);
        //    var totalQuantity = cart.CartProducts.Sum(cp => cp.Quantity);

        //    // Tạo một đơn hàng mới
        //    var order = new Order
        //    {
        //        UserID = userId,
        //        OrderDate = DateTime.UtcNow,
        //        UpdateDate = DateTime.UtcNow,
        //        Status = "Processing",
        //        TotalAmount = totalAmount,
        //        Quantity = totalQuantity,
        //        Details = cart.CartProducts.Select(cp => new OrderDetail
        //        {
        //            ProductId = cp.ProductId,
        //            Quantity = cp.Quantity,
        //            UnitPrice = cp.Product.Price
        //        }).ToList()
        //    };

        //    await _unitOfWork.Repository<Order>().InsertAsync(order);

        //    // Xóa giỏ hàng sau khi thanh toán
        //    await ClearCart(userId);

        //    await _unitOfWork.SaveChangesAsync();
        //    return true;
        //}
    }
}
