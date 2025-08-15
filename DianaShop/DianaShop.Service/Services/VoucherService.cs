using AutoMapper;
using DianaShop.Data;
using DianaShop.Data.Entities;
using DianaShop.Service.Interfaces;
using DianaShop.Service.ReponseModel;
using DianaShop.Service.RequestModel;
using Google;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianaShop.Service.Services
{
    public class VoucherService : IVoucherService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public VoucherService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<VoucherResponseModel>> GetAllVouchersAsync()
        {
            var vouchers = await _context.Vouchers.ToListAsync();
            return _mapper.Map<List<VoucherResponseModel>>(vouchers);
        }

        public async Task<VoucherResponseModel> GetVoucherByIdAsync(int id)
        {
            var voucher = await _context.Vouchers.FindAsync(id);
            return _mapper.Map<VoucherResponseModel>(voucher);
        }

        public async Task<VoucherResponseModel> CreateVoucherAsync(VoucherRequestModel request)
        {
            var voucher = _mapper.Map<Voucher>(request);
            _context.Vouchers.Add(voucher);
            await _context.SaveChangesAsync();
            return _mapper.Map<VoucherResponseModel>(voucher);
        }

        public async Task<VoucherResponseModel> UpdateVoucherAsync(int id, VoucherRequestModel request)
        {
            var voucher = await _context.Vouchers.FindAsync(id);
            if (voucher == null) return null;

            _mapper.Map(request, voucher);
            await _context.SaveChangesAsync();
            return _mapper.Map<VoucherResponseModel>(voucher);
        }

        public async Task<bool> DeleteVoucherAsync(int id)
        {
            var voucher = await _context.Vouchers.FindAsync(id);
            if (voucher == null) return false;

            _context.Vouchers.Remove(voucher);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
