using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianaShop.Service.ReponseModel
{
    public class UserResponseModel
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }    
        public string Email { get; set; }
        public string? Phone { get; set; }
        public string? Location { get; set; }
        public string? Image { get; set; }
        public bool? Status { get; set; }

        //public CartResponseModel? Cart { get; set; }
        public List<UserRoleResponseNameModel>? UserRoles { get; set; }
    }
}
