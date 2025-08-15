using DianaShop.Data.Entities;
using DianaShop.Service.ReponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DianaShop.Repository.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using DianaShop.Service.RequestModel;
using System.Security.Cryptography;
using DianaShop.Service.Interfaces;
using DianaShop.Repository.Utils;
using System.Text.RegularExpressions;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace DianaShop.Service.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IImageService _imageService; // Inject ImageService
        private readonly IBlobService _blobService; // Inject BlobService

        public UserService(IUnitOfWork unitOfWork, IMapper mapper,
                          IImageService imageService, IBlobService blobService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _imageService = imageService;
            _blobService = blobService;
        }

        // Lấy tất cả người dùng
        public async Task<IEnumerable<UserResponseModel>> GetAllUsersAsync()
        {
            return await _unitOfWork.Repository<User>().AsQueryable()
                .Where(u => !u.IsDelete)
                .ProjectTo<UserResponseModel>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        // Lấy tất cả người dùng với bộ lọc
        public async Task<DynamicResponse<UserResponseModel>> GetAllUsersAsync(RequestModel.QueryRequest.UserQueryRequest queryRequest)
        {
            var query = _unitOfWork.Repository<User>().AsQueryable().Where(u => !u.IsDelete);

            // Apply filters
            if (!string.IsNullOrEmpty(queryRequest.Username))
            {
                query = query.Where(u => u.Username.Contains(queryRequest.Username));
            }

            if (!string.IsNullOrEmpty(queryRequest.Email))
            {
                query = query.Where(u => u.Email.Contains(queryRequest.Email));
            }

            if (!string.IsNullOrEmpty(queryRequest.Phone))
            {
                query = query.Where(u => u.Phone.Contains(queryRequest.Phone));
            }

            if (!string.IsNullOrEmpty(queryRequest.Location))
            {
                query = query.Where(u => u.Location.Contains(queryRequest.Location));
            }

            if (queryRequest.Status.HasValue)
            {
                query = query.Where(u => u.Status == queryRequest.Status.Value);
            }
            
            if (queryRequest.EmailVerified.HasValue)
            {
                query = query.Where(u => u.EmailVerified == queryRequest.EmailVerified.Value);
            }

            if (queryRequest.RoleId.HasValue)
            {
                query = query.Where(u => u.UserRoles.Any(ur => ur.RoleId == queryRequest.RoleId.Value));
            }

            // Get total count for pagination
            var totalCount = await query.CountAsync();

            // Apply sorting
            if (!string.IsNullOrEmpty(queryRequest.Sort))
            {
                bool isDescending = !string.IsNullOrEmpty(queryRequest.Order) && 
                                   queryRequest.Order.ToLower() == "desc";

                switch (queryRequest.Sort.ToLower())
                {
                    case "username":
                        query = isDescending 
                            ? query.OrderByDescending(u => u.Username) 
                            : query.OrderBy(u => u.Username);
                        break;
                    case "email":
                        query = isDescending 
                            ? query.OrderByDescending(u => u.Email) 
                            : query.OrderBy(u => u.Email);
                        break;
                    case "location":
                        query = isDescending 
                            ? query.OrderByDescending(u => u.Location) 
                            : query.OrderBy(u => u.Location);
                        break;
                    case "phone":
                        query = isDescending 
                            ? query.OrderByDescending(u => u.Phone) 
                            : query.OrderBy(u => u.Phone);
                        break;
                    case "status":
                        query = isDescending 
                            ? query.OrderByDescending(u => u.Status) 
                            : query.OrderBy(u => u.Status);
                        break;
                    case "emailverified":
                        query = isDescending 
                            ? query.OrderByDescending(u => u.EmailVerified) 
                            : query.OrderBy(u => u.EmailVerified);
                        break;
                    default:
                        query = query.OrderByDescending(u => u.Id);
                        break;
                }
            }
            else
            {
                query = query.OrderByDescending(u => u.Id);
            }

            // Apply pagination
            var page = queryRequest.PageNumber <= 0 ? 1 : queryRequest.PageNumber;
            var size = queryRequest.PageSize <= 0 ? 10 : queryRequest.PageSize;
            
            var pagedQuery = query
                .Skip((page - 1) * size)
                .Take(size);

            // Get users from database
            var users = await pagedQuery
                .ProjectTo<UserResponseModel>(_mapper.ConfigurationProvider)
                .ToListAsync();

            // Create and return response
            return new DynamicResponse<UserResponseModel>
            {
                Code = StatusCodes.Status200OK,
                Message = "Get users successfully",
                Data = users.ToList(),
                MetaData = new PagingMetaData
                {
                    Page = page,
                    Size = size,
                    Total = totalCount
                }
            };
        }

        // Lấy người dùng theo ID
        public async Task<UserResponseModel?> GetUserByIdAsync(int id)
        {
            // Get the user with basic information
            var user = await _unitOfWork.Repository<User>().AsQueryable()
                .Where(u => u.Id == id)
                .ProjectTo<UserResponseModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
            
            if (user == null)
            {
                throw new Exception("User not found");
            }

            return user;
        }

        // Tạo người dùng mới
        public async Task<UserResponseModel> CreateUserAsync(UserCreateRequest request)
        {
            if (!Regex.IsMatch(request.Phone, "^\\+?[0-9]{10,15}$"))
            {
                throw new ArgumentException("Invalid phone number format.");
            }

            var existingUser = await _unitOfWork.Repository<User>().FindAsync(u => u.Email == request.Email);
            if (existingUser != null)
            {
                throw new InvalidOperationException("Email already exists.");
            }

            var user = _mapper.Map<User>(request);
            user.Password = PasswordTools.HashPassword(user.Password);
            user.EmailVerified = true;


            // Lưu user vào database trước khi gán UserRoles để tránh lỗi vòng lặp
            await _unitOfWork.Repository<User>().InsertAsync(user);
            await _unitOfWork.SaveChangesAsync();

            // Gán quyền mặc định (tránh ánh xạ trực tiếp trong UserCreateRequest)
            var userRole = new UserRole
            {
                UserId = user.Id,
                RoleId = 2,
                Status = true
            };
            await _unitOfWork.Repository<UserRole>().InsertAsync(userRole);
            await _unitOfWork.SaveChangesAsync();
            

            var createdUser = await _unitOfWork.Repository<User>()
                .AsQueryable()
                .Where(u => u.Id == user.Id)
                .ProjectTo<UserResponseModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();

            if (createdUser == null)
            {
                throw new Exception("Failed to retrieve created user.");
            }

            return createdUser;
        }

        public async Task<UserResponseModel?> UpdateUserAsync(int id, UserUpdateRequest request)
        {
            var existingUser = await _unitOfWork.Repository<User>()
                .AsQueryable()
                .Where(u => u.Id == id && !u.IsDelete)
                .FirstOrDefaultAsync();

            if (existingUser == null) return null;

            // Kiểm tra xem có bất kỳ trường nào được cập nhật không
            bool hasUpdates = false;

            if (!string.IsNullOrEmpty(request.Email))
            {
                var emailUser = await _unitOfWork.Repository<User>()
                    .FindAsync(u => u.Email == request.Email && u.Id != id);

                if (emailUser != null)
                {
                    throw new InvalidOperationException("Email already exists.");
                }
                existingUser.Email = request.Email;
                hasUpdates = true;
            }

            // Cập nhật từng trường nếu có giá trị mới
            if (!string.IsNullOrEmpty(request.Username))
            {
                existingUser.Username = request.Username;
                hasUpdates = true;
            }

            if (!string.IsNullOrEmpty(request.Phone))
            {
                existingUser.Phone = request.Phone;
                hasUpdates = true;
            }
            if (!string.IsNullOrEmpty(request.Location))
            {
                existingUser.Location = request.Location;
                hasUpdates = true;
            }
            if (request.Status.HasValue)
            {
                existingUser.Status = request.Status.Value;
                hasUpdates = true;
            }

            if (hasUpdates)
            {
                await _unitOfWork.Repository<User>().Update(existingUser, id);
                await _unitOfWork.SaveChangesAsync();
            }

            return _mapper.Map<UserResponseModel>(existingUser);
        }

        // Thêm/cập nhật hình ảnh hồ sơ
        public async Task<UserResponseModel?> AddProfileImageAsync(int id, IFormFile imageFile)
        {
            await _imageService.AddProfileImageAsync(id, imageFile);
            return await _unitOfWork.Repository<User>()
                .AsQueryable().Where(u => u.Id == id)
                .ProjectTo<UserResponseModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
        }

        // Cập nhật mật khẩu người dùng
        public async Task<bool> UpdatePasswordAsync(int id, string oldPassword, string newPassword)
        {
            var user = await _unitOfWork.Repository<User>().GetById(id);
            if (user == null)
            {
                throw new KeyNotFoundException("User not found.");
            }

            if (!PasswordTools.VerifyPassword(oldPassword, user.Password))
            {
                throw new UnauthorizedAccessException("Old password is incorrect.");
            }

            if (newPassword.Length < 8 ||
                !Regex.IsMatch(newPassword, @"[A-Z]") ||
                !Regex.IsMatch(newPassword, @"\d"))
            {
                throw new ArgumentException("New password must be at least 8 characters long and contain at least one uppercase letter and one number.");
            }

            user.Password = PasswordTools.HashPassword(newPassword);
            await _unitOfWork.Repository<User>().Update(user, id);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        // Xóa người dùng
        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _unitOfWork.Repository<User>().GetById(id);
            if (user == null) return false;

            user.IsDelete = true;
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        // Lấy người dùng theo username
        public async Task<UserResponseModel?> GetUserByUsernameAsync(string username)
        {
            // Get the user with basic information
            var user = await _unitOfWork.Repository<User>()
                .AsQueryable()
                .Where(u => u.Username == username && !u.IsDelete)
                .ProjectTo<UserResponseModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();

            return user;
        }

        // Lấy người dùng theo email
        public async Task<UserResponseModel?> GetUserByEmailAsync(string email)
        {
            // Get the user with basic information
            var user = await _unitOfWork.Repository<User>()
                .AsQueryable()
                .Where(u => u.Email == email)
                .ProjectTo<UserResponseModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();

            return user;
        }
    }
}
