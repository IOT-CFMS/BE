using DianaShop.Service.Interfaces;
using DianaShop.Service.RequestModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DianaShop.API.Controllers
{
    public class VNPayController : Controller
    {
        private readonly IUserService _usersService;
        private readonly IVnPayService _vpnPayService;
        private readonly IPaymentService _paymentService;
        private readonly IOrderService _orderService;

        public VNPayController(IUserService usersService, IVnPayService vpnPayService, IPaymentService paymentService, IOrderService orderService)
        {
            _usersService = usersService;
            _vpnPayService = vpnPayService;
            _paymentService = paymentService;
            _orderService = orderService;
        }

        private static int currentOrderId = 1;

        [HttpPost("vnpay/{paymentId}")]
        [Authorize]
        public async Task<IActionResult> PaymentCalls(int paymentId, [FromBody] string description)
        {
            var payment = await _paymentService.GetPaymentById(paymentId);
            var order = await _orderService.GetOrderById(payment.OrderID);
            var user = await _usersService.GetUserByIdAsync(order.UserID);
            // Tăng OrderId mỗi khi có người mới
            currentOrderId += 1;

            // Khởi tạo payload với các giá trị từ requestModel
            var payload = new VnPaymentRequestModel
            {
                OrderId = currentOrderId,
                FullName = user.Username,
                Description = description,
                Amount = payment.FinalAmount,
                CreatedDate = DateTime.UtcNow.AddHours(7), // Đặt thời gian hiện tại (UTC+7)
                UserId = user.Id,  // Include UserId in payload
                PaymentId = paymentId

            };


            // Tạo URL thanh toán
            var url = _vpnPayService.CreatePaymentUrl(HttpContext, payload);
            Console.WriteLine($"Generated VNPay URL: {url}"); // Debug URL


            // Trả về link thanh toán
            return Ok(url.Result);

        }



        [HttpGet("vnpay/api")]
        public async Task<IActionResult> PaymentCallBack()
        {
            var response = _vpnPayService.PaymentExecute(Request.Query);

            if (response == null || response.VnPayResponseCode != "00")
            {
                return StatusCode(500, new { message = $"Lỗi thanh toán VNPay: {response?.VnPayResponseCode ?? "unknown error"}" });
            }

            try
            {
                var payment = await _paymentService.GetPaymentById(response.PaymentId);
                if (payment != null)
                {
                    //payment.StatusId = 2;
                    await _paymentService.UpdatePaymentStatus(payment.Id, 2);
                    await _vpnPayService.SaveTransactionAsync(response); // Save transaction to database
                                                                         //tạo 1 cái hàm lưu payload giống trên hàm post vào database,lưu giống kiểu của wallet dưới hàm get,tạo 1 api mới để get

                    return Ok(new
                    {
                        response
                    });
                }

                return NotFound(new { message = $"Payment not found: {response}" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error updating wallet: {ex.Message}" });
            }
        }

        [HttpGet("vnpay/transactions")]
        [Authorize]
        public async Task<IActionResult> GetTransactions(int? userId = null)
        {
            try
            {
                var transactions = await _vpnPayService.GetTransactionsAsync(userId);
                return Ok(transactions);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
