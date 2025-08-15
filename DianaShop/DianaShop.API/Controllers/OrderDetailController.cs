using DianaShop.Service.Interfaces;
using DianaShop.Service.RequestModel;
using DianaShop.Service.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DianaShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderDetailController : Controller
    {
        private readonly IOrderDetailService _orderDetailService;

        public OrderDetailController(IOrderDetailService orderDetailService)
        {
            _orderDetailService = orderDetailService;
        }


        /// 🛒 Lấy order detail theo order
        [HttpGet("order/{orderId}")]
        public async Task<IActionResult> GetOrderDetailByOrder(int orderId)
        {
            var response = await _orderDetailService.GetAllOrderDetailsByOrderId(orderId);
            return Ok(response);
        }

        /// ➕ Thêm sản phẩm vào hóa đơn
        [HttpPost("add")]
        public async Task<IActionResult> AddOrderDetail([FromBody] OrderDetailRequestModel request)
        {
            try
            {
                var result = await _orderDetailService.AddOrderDetail(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Cập nhật số lượng sản phẩm trong đơn hàng
        /// </summary>
        [HttpPut("updateQuantity")]
        public async Task<IActionResult> UpdateCartProduct([FromBody] OrderDetailRequestModel request)
        {
            try
            {
                var result = await _orderDetailService.UpdateOrderDetail(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Xóa sản phẩm khỏi đơn hàng
        /// </summary>
        [HttpDelete("{orderId}/{productId}")]
        public async Task<IActionResult> RemoveProductFromCart(int orderId, int productId)
        {
            var result = await _orderDetailService.RemoveOrderDetail(orderId, productId);
            if (!result) return NotFound(new { message = "Remove Fail" });

            return Ok(new { Message = "Order detail delete successfully!" });
        }

        /// 🗑️ Xóa tất cả sản phẩm trong hóa đơn
        [HttpDelete("clear/{orderId}")]
        public async Task<IActionResult> ClearOrder(int orderId)
        {
            var result = await _orderDetailService.ClearOrderDetail(orderId);
            if (!result)
                return BadRequest("Order is already empty or does not exist.");
            return Ok(new { Message = "Order cleared successfully!" });
        }

    }
}
