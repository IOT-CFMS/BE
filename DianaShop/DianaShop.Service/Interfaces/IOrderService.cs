using DianaShop.Service.ReponseModel;
using DianaShop.Service.RequestModel;
using DianaShop.Service.RequestModel.QueryRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianaShop.Service.Interfaces
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderResponseModel>> GetAllOrder();
        Task<PaginatedResponse<OrderResponseModel>> SearchOrder(OrderQueryRequest request);
        Task<IEnumerable<OrderResponseModel>> GetAllOrderByUser(int userId);
        Task<OrderResponseModel?> GetOrderById(int orderId);
        Task<OrderResponseModel> CreateOrder(OrderRequestModel request);
        Task<OrderResponseModel> UpdateOrder(int orderId, int status);
        Task<OrderResponseModel> AddVoucherToOrder(int orderId, int voucherId);
        Task<bool> RemoveOrder(int orderId);

    }

}
