using DentistApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentistApp.Data
{
    public class ClientTrigger
    {
        public ClientTrigger()
        {
            Clients = new List<Client>();
        }
        public List<Client> Clients { get; set; }

    }
}
