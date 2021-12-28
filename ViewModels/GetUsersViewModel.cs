using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentistApp.ViewModels
{
    public class GetUsersViewModel
    {
        public GetUsersViewModel()
        {
            RoleNames = new List<string>();
        }
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public IList<string> RoleNames { get; set; }
    }
}
