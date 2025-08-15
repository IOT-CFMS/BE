using DianaShop.Service.Interfaces;
using DianaShop.Service.RequestModel;
using DianaShop.Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DianaShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : Controller
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }


        // lấy tất cả
        [HttpGet("all")]
        [Authorize(Roles = "Manager, Staff")]
        public async Task<IActionResult> GetAllPayments()
        {
            var response = await _paymentService.GetAllPayments();
            return Ok(response);
        }

        /// 🛒 Lấy tất cả payment của User
        [HttpGet("user/{userId}")]
        [Authorize]
        public async Task<IActionResult> GetAllPaymentsByUser(int userId)
        {
            var response = await _paymentService.GetAllPaymentByUser(userId);
            return Ok(response);
        }

        /// 🛒 Lấy payment theo order
        [HttpGet("order/{orderId}")]
        [Authorize]
        public async Task<IActionResult> GetPaymentByOrder(int orderId)
        {
            var response = await _paymentService.GetPaymentByOrder(orderId);
            return Ok(response);
        }

        /// 🛒 Lấy payment theo ID
        [HttpGet("id/{paymentId}")]
        [Authorize]
        public async Task<IActionResult> GetPaymentById(int paymentId)
        {
            var response = await _paymentService.GetPaymentById(paymentId);
            return Ok(response);
        }

        /// 🛒 API tạo payment 
        [HttpPost("create payment")]
        [Authorize]
        public async Task<IActionResult> CreateOrder([FromBody] PaymentRequestModel request)
        {
            if (request == null || request.OrderID <= 0)
            {
                return BadRequest("Invalid order ID.");
            }

            var response = await _paymentService.CreatePayment(request);
            return Ok(response);
        }

        /// <summary>
        /// Cập nhật trạng thái trong thanh toán
        /// </summary>
        [HttpPut("status/{paymentID}/{statusId}")]
        [Authorize(Roles = "Manager, Staff")]
        public async Task<IActionResult> UpdatePaymentStatus(int paymentID, int statusId)
        {
            try
            {
                var result = await _paymentService.UpdatePaymentStatus(paymentID, statusId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPut("method/{paymentID}/{methodId}")]
        [Authorize]
        public async Task<IActionResult> UpdatePaymentMethod(int paymentID, int methodId)
        {
            try
            {
                var result = await _paymentService.UpdatePaymentMethod(paymentID, methodId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Cập nhật trạng thái theo đơn hàng
        /// </summary>
        [HttpPut("order/{paymentID}")]
        [Authorize]
        public async Task<IActionResult> UpdatePaymentByOrder(int paymentID)
        {
            try
            {
                var result = await _paymentService.UpdatePaymentWithOrder(paymentID);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        /// 🗑️ Xóa thanh toan
        [HttpDelete("Remove/{paymentId}")]
        [Authorize]
        public async Task<IActionResult> RemovePayment(int paymentId)
        {
            var result = await _paymentService.RemovePayment(paymentId);
            if (!result)
                return BadRequest("Payment does not exist.");
            return Ok(new { Message = "Payment removed successfully!" });
        }

    }
}
