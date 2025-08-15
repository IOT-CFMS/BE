using DianaShop.Data.Entities;
using DianaShop.Service.Interfaces;
using DianaShop.Service.ReponseModel;
using DianaShop.Service.RequestModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DianaShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserRoleController : ControllerBase
    {
        private readonly IUserRoleService _userRoleService;

        public UserRoleController(IUserRoleService userRoleService)
        {
            _userRoleService = userRoleService;
        }

        // GET: api/UserRole/Role/{roleId}
        [HttpGet("Role/{userId}/Roles")]
        //[Authorize(Roles = "Manager")]
        public async Task<ActionResult<UserRoleResponseModel>> GetRoles(int userId)
        {
            if (userId <= 0)
            {
                return BadRequest("Invalid user ID");
            }

            var response = await _userRoleService.GetRolesOfUserAsync(userId);

            if (response == null || !response.Any())
            {
                return NotFound($"No roles found for UserId {userId}.");
            };

            return Ok(response);
        }


        [HttpGet("User/{userId}/HasRole/{roleName}")]
        //[Authorize(Roles = "Manager")]
        public ActionResult<bool> UserHasRole(int userId, string roleName)
        {
            if (userId <= 0)
            {
                return BadRequest("Invalid user ID");
            }
            if (string.IsNullOrEmpty(roleName))
            {
                return BadRequest("Role name cannot be null or empty");
            }
            var hasRole = _userRoleService.UserHasRole(userId, roleName);
            return Ok(hasRole);
        }

        [HttpPost]
        //[Authorize(Roles = "Manager")]
        public async Task<IActionResult> AddRole([FromBody] UserRoleRequestModels role)
        {
            if (role == null)
            {
                return BadRequest("Role cannot be null.");
            }

            if (role.UserId <= 0 || role.RoleId <= 0)
            {
                return BadRequest("Invalid UserId or RoleId.");
            }

            try
            {
                await _userRoleService.AddRoleAsync(role);
                return Ok("Role successfully added.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while adding the role: {ex.Message}");
            }
        }
        [HttpDelete("{userId}/{roleId}")]
        //[Authorize(Roles = "Manager")]
        public async Task<IActionResult> RemoveRole(int userId, int roleId)
        {
            if (userId <= 0 || roleId <= 0)
            {
                return BadRequest("Invalid role ID.");
            }
            try
            {
                await _userRoleService.RemoveRoleAsync(userId, roleId);
                return Ok($"Role with UserID: {userId}, RoleID: {roleId} successfully removed.");
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost("RequestRole")]
        //[Authorize(Roles = "Manager")]
        public async Task<IActionResult> RequestRole([FromBody] UserRoleRequestModels roleRequest)
        {
            if (roleRequest == null || roleRequest.UserId <= 0 || roleRequest.RoleId <= 0)
            {
                return BadRequest("Invalid role request.");
            }

            try
            {
                await _userRoleService.RequestRoleAsync(roleRequest.UserId, roleRequest.RoleId);
                return Ok("Role request successfully submitted.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while requesting the role: {ex.Message}");
            }
        }

        // NEW: Manager accepting a role request
        [HttpPut("AcceptRole/{userId}/{roleId}")]
        //[Authorize(Roles = "Manager")]
        public async Task<IActionResult> AcceptRoleRequest(int userId, int roleId)
        {
            if (userId <= 0 || roleId <= 0)
            {
                return BadRequest("Invalid user ID or role ID.");
            }

            try
            {
                await _userRoleService.AcceptRoleRequestAsync(userId, roleId);
                return Ok("Role request successfully accepted.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while accepting the role request: {ex.Message}");
            }
        }

        [HttpGet("PendingRequests")]
        //[Authorize(Roles = "Manager")]
        public async Task<ActionResult<IEnumerable<PendingRoles>>> GetAllPendingRoleRequests()
        {
            var pendingRequests = await _userRoleService.GetAllPendingRoleRequestsAsync();

            if (pendingRequests == null || !pendingRequests.Any())
            {
                return NotFound("No pending role requests found.");
            }

            var pendingRoleDtos = pendingRequests.Select(ur => new PendingRoles
            {
                UserId = ur.UserId,
                RoleId = ur.RoleId,
                CreatedDate = ur.CreatedDate
            }).ToList();

            return Ok(pendingRoleDtos);
        }
    }
}
