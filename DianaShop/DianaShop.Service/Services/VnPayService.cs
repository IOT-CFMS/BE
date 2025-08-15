
using DianaShop.Repository.UnitOfWork;
using DianaShop.Service.Interfaces;
using DianaShop.Service.ReponseModel;
using DianaShop.Service.RequestModel;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using DianaShop.Data.Entities;
using DianaShop.Service.Helpers;



namespace DianaShop.Service.Services
{
    public class VnPayService : IVnPayService
    {
        private IConfiguration _config;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IServiceProvider _serviceProvider;
        public VnPayService(IConfiguration config, IUnitOfWork unitOfWork, IServiceProvider serviceProvider)
        {
            _config = config;
            _unitOfWork = unitOfWork;
            _serviceProvider = serviceProvider;
        }
        private static Dictionary<string, (int UserId, int PaymentId, decimal Amount)> _paymentTracker
       = new Dictionary<string, (int UserId, int PaymentId, decimal Amount)>();
        public async Task<string> CreatePaymentUrl(HttpContext context, VnPaymentRequestModel model)
        {
            var tick = DateTime.Now.Ticks.ToString();
            var vnpay = new VnPayLibrary();

            _paymentTracker[tick] = (model.UserId, model.PaymentId, model.Amount);

            vnpay.AddRequestData("vnp_Version", _config["VnPay:Version"]);
            vnpay.AddRequestData("vnp_Command", _config["VnPay:Command"]);
            vnpay.AddRequestData("vnp_TmnCode", _config["VnPay:TmnCode"]);
            vnpay.AddRequestData("vnp_Amount", ((int)(model.Amount * 25557.50m * 100)).ToString()); //Số tiền thanh toán. Số tiền không mang các ký tự phân tách thập phân, phần nghìn, ký tự tiền tệ. Để gửi số tiền thanh toán là 100,000 VND(một trăm nghìn VNĐ) thì merchant cần nhân thêm 100 lần(khử phần thập phân), sau đó gửi sang VNPAY là: 10000000


            vnpay.AddRequestData("vnp_CreateDate", model.CreatedDate.ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData("vnp_CurrCode", _config["VnPay:CurrCode"]);
            vnpay.AddRequestData("vnp_IpAddr", Utils.GetIpAddress(context));
            vnpay.AddRequestData("vnp_Locale", _config["VnPay:Locale"]);

            vnpay.AddRequestData("vnp_OrderInfo", "Thanh toán cho đơn hàng:" + model.OrderId);
            vnpay.AddRequestData("vnp_OrderType", "other"); //default value: other
            //vnpay.AddRequestData("vnp_UserId", model.UserId.ToString());
            //vnpay.AddRequestData("vnp_PaymentId", model.PaymentId.ToString());

            var callback = _config["VnPay:PaymentBackReturnUrl"];
            vnpay.AddRequestData("vnp_ReturnUrl", callback);
            vnpay.AddRequestData("vnp_TxnRef", tick); // Mã tham chiếu của giao dịch tại hệ thống của merchant. Mã này là duy nhất dùng để phân biệt các đơn hàng gửi sang VNPAY. Không được trùng lặp trong ngày

            var paymentUrl = vnpay.CreateRequestUrl(_config["VnPay:BaseUrl"], _config["VnPay:HashSecret"]);

            return paymentUrl;
        }

        public VNPaymentResponseModel PaymentExecute(IQueryCollection collections)
        {
            var vnpay = new VnPayLibrary();
            foreach (var (key, value) in collections)
            {
                if (!string.IsNullOrEmpty(key) && key.StartsWith("vnp_"))
                {
                    vnpay.AddResponseData(key, value.ToString());
                }
            }
            var responseTicket = vnpay.GetResponseData("vnp_TxnRef");
            var (userId, paymentId, amount) = _paymentTracker.GetValueOrDefault(responseTicket);
            _paymentTracker.Remove(responseTicket);


            var vnp_orderId = Convert.ToInt64(responseTicket);


            var vnp_TransactionId = Convert.ToInt64(vnpay.GetResponseData("vnp_TransactionNo"));
            var vnp_SecureHash = collections.FirstOrDefault(p => p.Key == "vnp_SecureHash").Value;
            var vnp_ResponseCode = vnpay.GetResponseData("vnp_ResponseCode");
            var vnp_OrderInfo = vnpay.GetResponseData("vnp_OrderInfo");

            bool checkSignature = vnpay.ValidateSignature(vnp_SecureHash, _config["VnPay:HashSecret"]);
            if (!checkSignature)
            {
                return new VNPaymentResponseModel
                {
                    Success = false
                };
            }

            //int _userId;
            //if (!int.TryParse(vnpay.GetResponseData("vnp_UserId"), out _userId))
            //{
            //    _userId = 0; // Hoặc xử lý nếu không thể chuyển đổi
            //}

            //int _paymentId;
            //if (!int.TryParse(vnpay.GetResponseData("vnp_PaymentId"), out _paymentId))
            //{
            //    _paymentId = 0; // Hoặc xử lý nếu không thể chuyển đổi
            //}

            return new VNPaymentResponseModel
            {
                Success = true,
                PaymentMethod = "VnPay",
                OrderDescription = vnp_OrderInfo,
                OrderId = vnp_orderId.ToString(),
                TransactionId = vnp_TransactionId.ToString(),
                Token = vnp_SecureHash,
                VnPayResponseCode = vnp_ResponseCode,
                UserId = userId,
                PaymentId = paymentId,
                Amount = amount
            };

        }

        public async Task SaveTransactionAsync(VNPaymentResponseModel response)
        {
            var transaction = new VnPayTransaction
            {
                UserId = response.UserId,
                Amount = response.Amount,
                PaymentId = response.PaymentId,
                PaymentMethod = response.PaymentMethod,
                OrderDescription = response.OrderDescription,
                OrderId = response.OrderId,
                Token = response.Token,
                VnPayResponseCode = response.VnPayResponseCode,
                CreatedDate = DateTime.UtcNow
            };

            //_context.VnPayTransactions.Add(transaction);
            //await _context.SaveChangesAsync
            await _unitOfWork.Repository<VnPayTransaction>().InsertAsync(transaction);
            await _unitOfWork.CommitAsync();
        }

        public async Task<IEnumerable<VnPayTransaction>> GetTransactionsAsync(int? userId = null)
        {
            var query = _unitOfWork.Repository<VnPayTransaction>().AsQueryable();

            if (userId.HasValue)
            {
                query = query.Where(t => t.UserId == userId.Value);
            }

            return await query.ToListAsync();
        }

    }
}
