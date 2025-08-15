using DianaShop.Service.ReponseModel;
using DianaShop.Service.RequestModel;

namespace DianaShop.Service.Interfaces
{
    public interface ICartProductService
    {
        Task<CartProductResponse> AddProductToCart(CartProductRequest request);
        Task<bool> ClearCart(int cartId);
        Task<IEnumerable<CartProductResponse>> GetCartProducts(int cartId);
        Task<bool> RemoveProductFromCart(int cartId, int productId);
        Task<CartProductResponse> UpdateCartProduct(int cartId, int productId, int quantity);
    }
}