using AutoMapper;
using AutoMapper.QueryableExtensions;
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
    public class KioskService : IKioskService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public KioskService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<KioskRespondModel>> GetAllKiosks()
        {
            return await _unitOfWork.Repository<Kiosk>()
                .GetQueryable()
                .Where(x => !x.IsDelete)
                .ProjectTo<KioskRespondModel>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<KioskRespondModel> CreateKiosk(KioskRequestModel request)
        {
            if (request == null)
                throw new ArgumentException("Invalid request data.");

            var kiosk = new Kiosk()
            {
                KioskName = request.KioskName,
                Address = request.Address,
            };

            await _unitOfWork.Repository<Kiosk>().InsertAsync(kiosk);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<KioskRespondModel>(kiosk);
        }
    }
}
