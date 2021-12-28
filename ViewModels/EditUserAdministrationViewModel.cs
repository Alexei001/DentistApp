using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DentistApp.ViewModels
{
    public class EditUserAdministrationViewModel
    {
        public EditUserAdministrationViewModel()
        {
            Roles = new List<string>();

        }
        public string Id { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string City { get; set; }

        public List<string> Roles { get; set; }

    }
}
