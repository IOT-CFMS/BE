using DianaShop.Data.Entities;
using DianaShop.Service.ReponseModel;
using DianaShop.Service.RequestModel;
using Microsoft.AspNetCore.Http;

namespace DianaShop.Service.Interfaces
{
    public interface IVnPayService
    {
        Task<string> CreatePaymentUrl(HttpContext context, VnPaymentRequestModel model);
        VNPaymentResponseModel PaymentExecute(IQueryCollection collections);
        Task SaveTransactionAsync(VNPaymentResponseModel response);

        Task<IEnumerable<VnPayTransaction>> GetTransactionsAsync(int? userId = null);

    }
}
