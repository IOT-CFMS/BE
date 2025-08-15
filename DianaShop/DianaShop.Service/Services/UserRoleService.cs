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
    public class UserRoleService : IUserRoleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserRoleService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserRoleResponseModel>> GetRolesOfUserAsync(int userId)
        {
            var userRoles = await _unitOfWork.Repository<UserRole>().GetAll()
                .Where(s => s.UserId == userId && s.Status == true)
                .Include(s => s.Role)
                .ToListAsync();
            return _mapper.Map<IEnumerable<UserRoleResponseModel>>(userRoles);
        }

        public bool UserHasRole(int userId, string roleName)
        {
            return _unitOfWork.Repository<UserRole>()
                .AsQueryable()
                .Where(s => s.UserId == userId && s.Status == true)
                .Include(s => s.Role)
                .Any(s => s.Role != null && s.Role.Name == roleName);
        }

        public async Task AddRoleAsync(UserRoleRequestModels role)
        {
            var newRole = new UserRole { UserId = role.UserId, RoleId = role.RoleId, Status = true };
            await _unitOfWork.Repository<UserRole>().InsertAsync(newRole);
            await _unitOfWork.CommitAsync();
        }

        public async Task UpdateRolesAsync(int userId, List<int> newRoleIds)
        {
            var existingRoles = await _unitOfWork.Repository<UserRole>()
                                                  .GetWhere(ur => ur.UserId == userId);

            // Remove roles that are not in the newRoleIds list
            var rolesToRemove = existingRoles.Where(er => !newRoleIds.Contains(er.RoleId)).ToList();
            foreach (var role in rolesToRemove)
            {
                _unitOfWork.Repository<UserRole>().Delete(role);
            }

            // Add new roles that the user doesn't already have
            var rolesToAdd = newRoleIds.Where(nr => !existingRoles.Any(er => er.RoleId == nr)).ToList();
            foreach (var roleId in rolesToAdd)
            {
                var newRole = new UserRole { UserId = userId, RoleId = roleId, Status = true };
                await _unitOfWork.Repository<UserRole>().InsertAsync(newRole);
            }

            await _unitOfWork.CommitAsync();
        }



        public async Task RemoveRoleAsync(int userId, int roleId)
        {
            var userRole = await _unitOfWork.Repository<UserRole>().FindAsync(ur => ur.UserId == userId && ur.RoleId == roleId);

            if (userRole != null)
            {
                _unitOfWork.Repository<UserRole>().Delete(userRole);
                await _unitOfWork.CommitAsync();
            }
            else
            {
                throw new Exception($"UserRole with UserID {userId} and RoleID {roleId} not found.");
            }
        }

        public async Task RequestRoleAsync(int userId, int roleId)
        {
            var existingRoleRequest = await _unitOfWork.Repository<UserRole>()
                                                        .FindAsync(ur => ur.UserId == userId && ur.RoleId == roleId);

            if (existingRoleRequest != null)
            {
                throw new Exception("Role request already exists.");
            }

            var userRole = new UserRole
            {
                UserId = userId,
                RoleId = roleId,
                Status = false
            };

            await _unitOfWork.Repository<UserRole>().InsertAsync(userRole);
            await _unitOfWork.CommitAsync();
        }

        public async Task AcceptRoleRequestAsync(int userId, int roleId)
        {
            var roleRequest = await _unitOfWork.Repository<UserRole>()
                                                .FindAsync(ur => ur.UserId == userId && ur.RoleId == roleId && ur.Status == false);

            if (roleRequest == null)
            {
                throw new Exception("No pending role request found.");
            }

            roleRequest.Status = true;

            await _unitOfWork.Repository<UserRole>().Update(roleRequest, roleRequest.RoleId);
            await _unitOfWork.CommitAsync();
        }

        public async Task<IEnumerable<UserRole>> GetAllPendingRoleRequestsAsync()
        {
            var pendingRequests = await _unitOfWork.Repository<UserRole>()
                                                    .AsQueryable()
                                                    .Where(ur => ur.Status == false)
                                                    .Include(ur => ur.User)
                                                    .Include(ur => ur.Role)
                                                    .ToListAsync();

            return pendingRequests;
        }
    }
}
