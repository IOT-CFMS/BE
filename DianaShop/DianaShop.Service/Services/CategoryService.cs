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
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CategoryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CategoryResponseModel>> GetAllAsync()
        {
            return await _unitOfWork.Repository<Category>().GetAll()
                .Where(c => !c.IsDelete)
                .ProjectTo<CategoryResponseModel>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<CategoryResponseModel?> GetByIdAsync(int id)
        {
            return await _unitOfWork.Repository<Category>().AsQueryable()
                .Where(c => c.Id == id)
                .ProjectTo<CategoryResponseModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
        }

        public async Task<CategoryResponseModel> CreateAsync(CategoryRequestModel request)
        {
            var category = _mapper.Map<Category>(request);
            await _unitOfWork.Repository<Category>().InsertAsync(category);
            var result = await _unitOfWork.SaveChangesAsync();
            if (result <= 0) throw new Exception("Failed to create category");
            return _mapper.Map<CategoryResponseModel>(category);
        }

        public async Task<CategoryResponseModel?> UpdateAsync(int id, CategoryRequestModel request)
        {
            var existingCategory = await _unitOfWork.Repository<Category>().GetById(id);
            if (existingCategory == null) return null;

            _mapper.Map(request, existingCategory);
            await _unitOfWork.Repository<Category>().Update(existingCategory, id);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<CategoryResponseModel>(existingCategory);
        }

        public async Task SoftDeleteAsync(int id)
        {
            var category = await _unitOfWork.Repository<Category>().GetById(id);
            if (category == null) return;
            category.IsDelete = true;
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task HardDeleteAsync(int id)
        {
            await _unitOfWork.Repository<Category>().HardDelete(id);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
