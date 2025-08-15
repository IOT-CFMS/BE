using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianaShop.Data.Entities
{
    public class User : BaseEntity
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string? Phone { get; set; }
        public string? Location { get; set; }
        public string? Token { get; set; }
        public string? Image { get; set; }
        public bool? Status { get; set; }
        public bool EmailVerified { get; set; } = false;

        public virtual Cart Cart { get; set; }  
        public virtual ICollection<UserRole>? UserRoles { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<VnPayTransaction>? VnPay { get; set; }
        public virtual ICollection<Schedule>? Schedules { get; set; }
    }
}
