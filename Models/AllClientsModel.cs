
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentistApp.Models
{
    public class AllClientsModel
    {
        public List<Client> Clients { get; set; }
        public PaginatedList PaginatedList { get; set; }
    }
}
