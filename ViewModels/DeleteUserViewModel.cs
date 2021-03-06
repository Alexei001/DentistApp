using DentistApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentistApp.ViewModels
{
    public class DeleteUserViewModel
    {
        public DeleteUserViewModel()
        {
            Clients = new List<Client>();
        }
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public List<Client> Clients { get; set; }
    }
}
