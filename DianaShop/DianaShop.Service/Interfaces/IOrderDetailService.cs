using DianaShop.Service.ReponseModel;
using DianaShop.Service.RequestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianaShop.Service.Interfaces
{
    public interface IOrderDetailService
    {
        Task<IEnumerable<OrderDetailResponseModel>> GetAllOrderDetailsByOrderId(int orderId);
        Task<OrderDetailResponseModel> AddOrderDetail(OrderDetailRequestModel request);
        Task<OrderDetailResponseModel> UpdateOrderDetail(OrderDetailRequestModel request);
        Task<bool> RemoveOrderDetail(int orderId, int productId);
        Task<bool> ClearOrderDetail(int orderId);
    }
}
