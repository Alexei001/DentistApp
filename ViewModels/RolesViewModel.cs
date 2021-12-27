using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentistApp.ViewModels
{
    public class RolesViewModel
    {
        public RolesViewModel()
        {
            Users = new List<User>();

        }

        public string RoleId { get; set; }
        public string RoleName { get; set; }

        public List<User> Users { get; set; }

    }
}
