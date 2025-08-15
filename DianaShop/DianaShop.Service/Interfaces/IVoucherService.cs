using DianaShop.Service.ReponseModel;
using DianaShop.Service.RequestModel;

namespace DianaShop.Service.Interfaces
{
    public interface IVoucherService
    {
        Task<VoucherResponseModel> CreateVoucherAsync(VoucherRequestModel request);
        Task<bool> DeleteVoucherAsync(int id);
        Task<List<VoucherResponseModel>> GetAllVouchersAsync();
        Task<VoucherResponseModel> GetVoucherByIdAsync(int id);
        Task<VoucherResponseModel> UpdateVoucherAsync(int id, VoucherRequestModel request);
    }
}