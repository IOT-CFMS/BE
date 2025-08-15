using AutoMapper;
using DianaShop.Data.Entities;
using DianaShop.Repository.UnitOfWork;
using DianaShop.Service.Interfaces;
using DianaShop.Service.ReponseModel;
using DianaShop.Service.RequestModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianaShop.Service.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PaymentService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PaymentResponseModel>> GetAllPayments()
        {
            var orders = await _unitOfWork.Repository<Payment>()
                .GetQueryable()
                .Include(p => p.PaymentMethod)
                .Include(p => p.StageStatus)
                .ToListAsync();
            return _mapper.Map<IEnumerable<PaymentResponseModel>>(orders);
        }

        public async Task<PaymentResponseModel> GetPaymentById(int paymentId)
        {
            var payment = await _unitOfWork.Repository<Payment>()
                .GetQueryable()
                .Where(cp => cp.Id == paymentId)
                .Include(p => p.PaymentMethod)
                .Include(p => p.StageStatus)
                .FirstOrDefaultAsync();
            return _mapper.Map<PaymentResponseModel>(payment);
        }

        public async Task<IEnumerable<PaymentResponseModel>> GetAllPaymentByUser(int userId)
        {
            var payment = await _unitOfWork.Repository<Payment>()
                .GetQueryable()
                .Include(p => p.PaymentMethod)
                .Include(p => p.StageStatus)
                .ToListAsync();
            return _mapper.Map<IEnumerable<PaymentResponseModel>>(payment);
        }

        public async Task<PaymentResponseModel> GetPaymentByOrder(int orderId)
        {
            var payment = await _unitOfWork.Repository<Payment>()
                .GetQueryable()
                .Where(cp => cp.OrderID == orderId)
                .FirstOrDefaultAsync();
            return _mapper.Map<PaymentResponseModel>(payment);
        }

        public async Task<PaymentResponseModel> CreatePayment(PaymentRequestModel request)
        {
            if (request == null)
                throw new ArgumentException("Invalid request data.");
            if (request.OrderID <= 0)
                throw new ArgumentException("OrderID must be greater than 0.");

            // Kiểm tra xem User đã có đơn chưa
            var existingOrder = await _unitOfWork.Repository<Order>()
                .FindAsync(c => c.Id == request.OrderID);
            if (existingOrder == null)
                throw new InvalidOperationException("Order is not exist.");

            //Kiểm tra User có thanh toán chưa hoàn thành không
            var existingPayment = await _unitOfWork.Repository<Payment>()
                .FindAsync(c => c.Id == request.OrderID);
            if (existingPayment != null)
                throw new InvalidOperationException($"You have a payment of this order.");

            var user = await _unitOfWork.Repository<User>()
                .FindAsync(u => u.Id == existingOrder.UserID);
            if (user == null)
                throw new InvalidOperationException("User is not exist.");

            var payment = new Payment
            {
                OrderID = request.OrderID,
                PaymentMethodID = request.PaymentMethodID,
                FinalAmount = existingOrder.FinalAmount,
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
                StatusId = 1
            };

            await _unitOfWork.Repository<Payment>().InsertAsync(payment);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<PaymentResponseModel>(payment);
        }

        public async Task<PaymentResponseModel> UpdatePaymentWithOrder(int paymentID)
        {
            var payment = await _unitOfWork.Repository<Payment>()
                .GetQueryable()
                .Where(cp => cp.Id == paymentID)
                .FirstOrDefaultAsync();

            if (payment == null)
                throw new Exception("Payment not found");
            if (payment.StatusId != 1)
                throw new Exception("Payment is unchangable");


            var order = await _unitOfWork.Repository<Order>()
                .GetQueryable()
                .Where(cp => cp.Id == payment.OrderID)
                .FirstOrDefaultAsync();
            if (order == null)
                throw new Exception("Order not found");



            payment.FinalAmount = order.TotalAmount;
            payment.UpdatedDate = DateTime.Now;
            await _unitOfWork.Repository<Payment>().UpdateAsync(payment);

            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<PaymentResponseModel>(payment);
        }

        public async Task<PaymentResponseModel> UpdatePaymentStatus(int paymentId, int statusID)
        {
            var payment = await _unitOfWork.Repository<Payment>()
                .GetQueryable()
                .Where(cp => cp.Id == paymentId)
                .FirstOrDefaultAsync();

            if (payment == null)
                throw new Exception("Payment not found");

            payment.StatusId = statusID;
            payment.UpdatedDate = DateTime.Now;
            await _unitOfWork.Repository<Payment>().UpdateAsync(payment);

            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<PaymentResponseModel>(payment);
        }

        public async Task<PaymentResponseModel> UpdatePaymentMethod(int paymentId, int paymentMethodID)
        {
            var payment = await _unitOfWork.Repository<Payment>()
                .GetQueryable()
                .Where(cp => cp.Id == paymentId)
                .FirstOrDefaultAsync();

            if (payment == null)
                throw new Exception("Payment not found");

            var paymentmethod = await _unitOfWork.Repository<PaymentMethod>()
                .GetQueryable()
                .Where(cp => cp.Id == paymentMethodID)
                .FirstOrDefaultAsync();
            if (paymentmethod == null)
                throw new Exception("Payment method not exist");

            payment.PaymentMethodID = paymentMethodID;
            payment.UpdatedDate = DateTime.Now;
            await _unitOfWork.Repository<Payment>().UpdateAsync(payment);

            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<PaymentResponseModel>(payment);
        }

        public async Task<bool> RemovePayment(int paymentId)
        {
            var payment = await _unitOfWork.Repository<Payment>()
                .FindAsync(c => c.Id == paymentId);
            if (payment == null)
            {
                return false;
            }

            _unitOfWork.Repository<Payment>().Delete(payment);

            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
