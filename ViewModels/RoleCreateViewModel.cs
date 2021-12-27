using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DentistApp.ViewModels
{
    public class RoleCreateViewModel
    {
        public RoleCreateViewModel()
        {
            Users = new List<User>();
        }
        public string? RoleId { get; set; }
        [Display(Name = "Role Name")]
        [Required]
        public string RoleName { get; set; }

        public List<User> Users { get; set; }

    }
}
