using DianaShop.Service.Interfaces;
using DianaShop.Service.ReponseModel;
using DianaShop.Service.RequestModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DianaShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Manager, Staff")]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllCarts()
        {
            var carts = await _cartService.GetAllCarts();
            return Ok(carts);
        }

        /// 🛒 Lấy giỏ hàng của User
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetCart(int userId)
        {
            var response = await _cartService.GetCartByUser(userId);
            return Ok(response);
        }

        /// ➕ Thêm sản phẩm vào giỏ hàng
        [HttpPost("add-product")]
        public async Task<IActionResult> AddProductToCart([FromBody] AddProductToCartRequest request)
        {
            var response = await _cartService.AddProductToCart(request.UserId, request.ProductId, request.Quantity);
            return Ok(response);
        }

        /// 🛒 API tạo giỏ hàng mới cho User
        [HttpPost("create")]
        public async Task<IActionResult> CreateCart([FromBody] CartRequestModel request)
        {
            if (request == null || request.UserID <= 0)
            {
                return BadRequest("Invalid user ID.");
            }

            var cart = await _cartService.CreateCarts(request);
            return Ok(cart);
        }

        /// ✏️ Cập nhật số lượng sản phẩm trong giỏ hàng
        [HttpPut("update-product")]
        public async Task<IActionResult> UpdateCartProduct([FromBody] UpdateCartProductRequest request)
        {
            var response = await _cartService.UpdateCartProduct(request.UserId, request.ProductId, request.Quantity);
            return Ok(response);
        }

        /// ❌ Xóa sản phẩm khỏi giỏ hàng
        [HttpDelete("remove-product")]
        public async Task<IActionResult> RemoveProductFromCart([FromBody] RemoveProductFromCartRequest request)
        {
            var response = await _cartService.RemoveProductFromCart(request.UserId, request.ProductId);
            return Ok(response);
        }

        /// 🗑️ Xóa toàn bộ giỏ hàng
        [HttpDelete("clear/{userId}")]
        public async Task<IActionResult> ClearCart(int userId)
        {
            var success = await _cartService.ClearCart(userId);
            if (!success)
                return BadRequest("Cart is already empty or does not exist.");
            return Ok(new { Message = "Cart cleared successfully" });
        }

        ///// ✅ Thanh toán giỏ hàng
        //[HttpPost("checkout/{userId}")]
        //public async Task<IActionResult> Checkout(int userId)
        //{
        //    var success = await _cartService.Checkout(userId);
        //    if (!success)
        //        return BadRequest("Checkout failed.");
        //    return Ok(new { Message = "Order placed successfully" });
        //}
    }
}
