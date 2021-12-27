using DentistApp.Models;
using DentistApp.Models.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentistApp.Data.Services
{
    public interface IClientsServices
    {
        Task<AllClientsModel> GetAllClients(SortState sortOrder, string searchString, int pageSize, int pageNumber);

        Client GetClientById(int id);

        bool AddNewClient(Client model);

        Task<IEnumerable<Client>> GetAllClientsByUserId(string userId);

        bool DeleteClient(int Id);
        bool UpdateClient(Client model);

    }
}
