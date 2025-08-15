using DianaShop.Service.Interfaces;
using DianaShop.Service.RequestModel;
using DianaShop.Service.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DianaShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartProductController : ControllerBase
    {
        private readonly ICartProductService _cartProductService;

        public CartProductController(ICartProductService cartProductService)
        {
            _cartProductService = cartProductService;
        }

        /// <summary>
        /// Thêm sản phẩm vào giỏ hàng
        /// </summary>
        [HttpPost("add")]
        public async Task<IActionResult> AddProductToCart([FromBody] CartProductRequest request)
        {
            try
            {
                var result = await _cartProductService.AddProductToCart(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Lấy danh sách sản phẩm trong giỏ hàng
        /// </summary>
        [HttpGet("{cartId}")]
        public async Task<IActionResult> GetCartProducts(int cartId)
        {
            var result = await _cartProductService.GetCartProducts(cartId);
            return Ok(result);
        }

        /// <summary>
        /// Cập nhật số lượng sản phẩm trong giỏ hàng
        /// </summary>
        [HttpPut("{cartId}/{productId}")]
        public async Task<IActionResult> UpdateCartProduct(int cartId, int productId, [FromBody] int quantity)
        {
            try
            {
                var result = await _cartProductService.UpdateCartProduct(cartId, productId, quantity);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Xóa sản phẩm khỏi giỏ hàng
        /// </summary>
        [HttpDelete("{cartId}/{productId}")]
        public async Task<IActionResult> RemoveProductFromCart(int cartId, int productId)
        {
            var success = await _cartProductService.RemoveProductFromCart(cartId, productId);
            if (!success) return NotFound(new { message = "Product not found in cart" });

            return NoContent();
        }

        /// <summary>
        /// Xóa tất cả sản phẩm trong giỏ hàng
        /// </summary>
        [HttpDelete("{cartId}/clear")]
        public async Task<IActionResult> ClearCart(int cartId)
        {
            var success = await _cartProductService.ClearCart(cartId);
            if (!success) return NotFound(new { message = "Cart is already empty" });

            return NoContent();
        }
    }
}
