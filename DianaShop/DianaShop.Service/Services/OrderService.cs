using AutoMapper;
using AutoMapper.QueryableExtensions;
using DianaShop.Data.Entities;
using DianaShop.Repository.UnitOfWork;
using DianaShop.Service.Interfaces;
using DianaShop.Service.ReponseModel;
using DianaShop.Service.RequestModel;
using DianaShop.Service.RequestModel.QueryRequest;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianaShop.Service.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OrderService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<OrderResponseModel>> GetAllOrder()
        {
            return await _unitOfWork.Repository<Order>()
                .GetQueryable()
                .Where(x => !x.IsDelete && x.StatusId != 6)
                .ProjectTo<OrderResponseModel>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<PaginatedResponse<OrderResponseModel>> SearchOrder(OrderQueryRequest request)
        {
            var orderQuery = _unitOfWork.Repository<Order>()
                .GetAll()
                .Where(o => o.StatusId == null || o.StatusId == request.StatusId)
                .Where(o => o.UserID == null || o.UserID == request.UserID)
                .Where(o => o.VoucherID == null || o.VoucherID == request.VoucherID)
                .Where(o => o.Quantity == null || o.Quantity == request.Quantity)
                .Where(o => request.MinAmount == null || o.FinalAmount >= request.MinAmount)
                .Where(o => request.MaxAmount == null || o.FinalAmount <= request.MaxAmount)
                .Where(o => request.MinCreatedDate == null || o.OrderDate <= request.MinCreatedDate)
                .Where(o => request.MaxCreatedDate == null || o.OrderDate <= request.MaxCreatedDate)
                .Where(o => request.MinUpdateDate == null || o.UpdateDate >= request.MinUpdateDate)
                .Where(o => request.MaxUpdateDate == null || o.UpdateDate <= request.MaxUpdateDate)
                .ProjectTo<OrderResponseModel>(_mapper.ConfigurationProvider);

            switch(request.SortBy)
            {
                case OrderSort.Default:
                    orderQuery = request.Ascending ? orderQuery.OrderBy(p => p.Id) : orderQuery.OrderByDescending(p => p.Id);
                    break;
                case OrderSort.Quantity: 
                    orderQuery = request.Ascending ? orderQuery.OrderBy(p => p.Quantity) : orderQuery.OrderByDescending(p => p.Quantity);
                    break;
                case OrderSort.FinalAmount:
                    orderQuery = request.Ascending ? orderQuery.OrderBy(p => p.FinalAmount) : orderQuery.OrderByDescending(p => p.FinalAmount);
                    break;
                default: 
                    break;
            }


            return await PaginatedResponse<OrderResponseModel>.CreateAsync(orderQuery, request.PageNumber, request.PageSize);
        }


        public async Task<IEnumerable<OrderResponseModel>> GetAllOrderByUser(int userId)
        {
            return await _unitOfWork.Repository<Order>()
                .AsQueryable()
                .Where(cp => cp.UserID == userId && !cp.IsDelete && cp.StatusId != 6)
                .ProjectTo<OrderResponseModel>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<OrderResponseModel?> GetOrderById(int orderId)
        {
            return await _unitOfWork.Repository<Order>()
                .GetQueryable()
                .Where(cp => cp.Id == orderId && !cp.IsDelete && cp.StatusId != 6)
                .ProjectTo<OrderResponseModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
        }

        public async Task<OrderResponseModel> CreateOrder(OrderRequestModel request)
        {
            if (request == null)
                throw new ArgumentException("Invalid request data.");
            if (request.UserID <= 0)
                throw new ArgumentException("UserID must be greater than 0.");

            // Kiểm tra xem User đã có giỏ hàng chưa
            var unpaidOrder = await _unitOfWork.Repository<Order>()
                .FindAsync(c => c.UserID == request.UserID && c.StatusId == 1);

            if (unpaidOrder != null)
                throw new InvalidOperationException("User already has an unpaid order.");

            var order = new Order
            {
                UserID = request.UserID,
                Quantity = 0,
                TotalAmount = 0,
                FinalAmount = 0,
                OrderDate = DateTime.Now,
                UpdateDate = DateTime.Now,
                StatusId = 1,
                Details = new List<OrderDetail>()
            };

            await _unitOfWork.Repository<Order>().InsertAsync(order);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<OrderResponseModel>(order);
        }

        public async Task<bool> RemoveOrder(int orderId)
        {
            var order = await _unitOfWork.Repository<Order>()
                .FindAsync(c => c.Id == orderId);
            if (order == null)
            {
                return false;
            }

            order.StatusId = 6;
            order.IsDelete = true;
            order.UpdateDate = DateTime.Now;
            await _unitOfWork.Repository<Order>().UpdateAsync(order);

            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        //public async Task<bool> RemoveOrder(int orderId)
        //{
        //    var order = await _unitOfWork.Repository<Order>()
        //        .FindAsync(c => c.Id == orderId);
        //    if (order == null)
        //    {
        //        return false;
        //    }

        //    var orderDetails = _unitOfWork.Repository<OrderDetail>()
        //        .GetQueryable()
        //        .Where(cp => cp.OrderId == orderId);
        //    if (await orderDetails.AnyAsync())
        //    {
        //        //_unitOfWork.Repository<OrderDetail>().DeleteRange(orderDetails);
        //        throw new ArgumentException("There are order detail isn't delete.");
        //    }

        //    _unitOfWork.Repository<Order>().Delete(order);

        //    await _unitOfWork.SaveChangesAsync();
        //    return true;
        //}

        public async Task<OrderResponseModel> UpdateOrder(int orderId, int status)
        {
            var order = await _unitOfWork.Repository<Order>()
                .GetQueryable()
                .Where(cp => cp.Id == orderId)
                .FirstOrDefaultAsync();
            if (order == null)
                throw new Exception("Order not found");

            var payment = await _unitOfWork.Repository<Payment>()
                .GetQueryable()
                .Where(cp => cp.OrderID == orderId)
                .FirstOrDefaultAsync();
            if (payment == null)
                throw new Exception("Payment not found");

            if (order.StatusId == 5)
            {
                throw new Exception("Order is complete, it can't be changed!");
            }

            if (order.StatusId == 3 && status != 6 && payment.StatusId == 1)
            {
                throw new Exception("Order is unpaid, it can't be changed!");
            }

            order.StatusId = status;
            order.UpdateDate = DateTime.Now;
            await _unitOfWork.Repository<Order>().UpdateAsync(order);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<OrderResponseModel>(order);
        }

        public async Task<OrderResponseModel> AddVoucherToOrder(int orderId, int voucherId)
        {
            var order = await _unitOfWork.Repository<Order>()
                .GetQueryable()
                .Where(cp => cp.Id == orderId)
                .FirstOrDefaultAsync();
            if (order == null)
                throw new Exception("Order not found");

            if (order.VoucherID > 0 && order.VoucherID != null)
            {
                throw new Exception("There is a voucher is applied before!");
            }

            var voucher = await _unitOfWork.Repository<Voucher>()
                    .FindAsync(cp => cp.Id == order.VoucherID);
            if (voucher == null)
            {
                throw new Exception("This Voucher isn't exist");
            }

            ///check time expirence
            var nowtime = DateTime.Now;
            if(nowtime < voucher.StartDate || nowtime > voucher.EndDate)
            {
                throw new Exception("This Voucher is not within expiry date");
            }

            order.VoucherID = voucherId;

            var reducedAmount = order.TotalAmount;
            if (order.TotalAmount >= (decimal)voucher.MinimumPurchase)
            {
                reducedAmount = (order.TotalAmount * (decimal)(voucher.DiscountPercentage/100));
            }
            order.FinalAmount = order.TotalAmount - reducedAmount;
            order.UpdateDate = DateTime.Now;
            await _unitOfWork.Repository<Order>().UpdateAsync(order);

            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<OrderResponseModel>(order);
        }
    }
}
