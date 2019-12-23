using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Heavy.Web.ViewModels
{
    public class RoleUserViewModel
    {
        public RoleUserViewModel()
        {
            Users = new List<IdentityUser>();
        }

        [Required]
        public string RoleId { get; set; }
        [Required]
        public string UserId { get; set; }
        public List<IdentityUser> Users { get; set; }
    }
}
