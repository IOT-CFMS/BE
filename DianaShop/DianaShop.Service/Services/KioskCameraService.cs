using AutoMapper;
using DianaShop.Data.Entities;
using DianaShop.Repository.UnitOfWork;
using DianaShop.Service.Interfaces;
using Firebase.Database;
using Microsoft.Extensions.Configuration;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianaShop.Service.Services
{
    public class KioskCameraService : IKioskCameraService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly FirebaseClient _firebaseClient;
        private readonly IConfiguration _config;

        public KioskCameraService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<string> ManualAddCamera(string path)
        {
            if(path != null)
            {

                var image = new KioskCamera { KiodID = 1, Base64Image = path };
                await _unitOfWork.Repository<KioskCamera>().InsertAsync(image);
                await _unitOfWork.SaveChangesAsync();
            }
            

            return path;
        }
    }
}
