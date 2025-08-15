using DianaShop.Service.ReponseModel;
using DianaShop.Service.RequestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianaShop.Service.Interfaces
{
    public interface IPaymentService
    {
        Task<IEnumerable<PaymentResponseModel>> GetAllPayments();
        Task<PaymentResponseModel> GetPaymentById(int paymentId);
        Task<IEnumerable<PaymentResponseModel>> GetAllPaymentByUser(int userId);
        Task<PaymentResponseModel> GetPaymentByOrder(int orderId);
        Task<PaymentResponseModel> CreatePayment(PaymentRequestModel request);
        Task<PaymentResponseModel> UpdatePaymentWithOrder(int orderId);
        Task<PaymentResponseModel> UpdatePaymentStatus(int paymentId, int statusID);
        Task<PaymentResponseModel> UpdatePaymentMethod(int paymentId, int paymentMethodID);
        Task<bool> RemovePayment(int paymentId);
    }
}
