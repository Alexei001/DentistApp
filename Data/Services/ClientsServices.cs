using DentistApp.Models;
using DentistApp.Models.Enum;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentistApp.Data.Services
{
    public class ClientsServices : IClientsServices
    {
        private readonly AplicationContext context;

        public ClientsServices(AplicationContext context)
        {
            this.context = context;
        }

        public bool AddNewClient(Client model)
        {
            model.Notify = false;
            context.Clients.Add(model);
            context.SaveChanges();
            var client = context.Clients.FirstOrDefault(c => c.Email == model.Email);
            if (client == null)
            {
                return false;
            }
            return true;

        }

        public async Task<AllClientsModel> GetAllClients(SortState sortOrder, string searchString, int pageSize, int pageNumber)
        {
            IQueryable<Client> clients = context.Clients.Include(c => c.Doctor).Include(c => c.Procedure);

            if (!String.IsNullOrEmpty(searchString))
            {
                clients = clients.Where(c => c.Name.Contains(searchString));
            }

            clients = sortOrder switch
            {
                SortState.NamdeDesc => clients.OrderByDescending(c => c.Name),
                SortState.DoctorAsc => clients.OrderBy(c => c.Doctor.Name),
                SortState.DoctorDesc => clients.OrderByDescending(c => c.Doctor.Name),
                SortState.ProcedureAsc => clients.OrderBy(c => c.Procedure.Name),
                SortState.ProcedureDesc => clients.OrderByDescending(c => c.Procedure.Name),
                SortState.AvailableAsc => clients.OrderBy(c => c.Available),
                SortState.AvailableDesc => clients.OrderByDescending(c => c.Available),
                _ => clients.OrderBy(c => c.Name)
            };

            var count = await clients.CountAsync();
            var items = await clients.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            PaginatedList paginatedList = new PaginatedList(count, pageNumber, pageSize);

            AllClientsModel allClientsModel = new AllClientsModel()
            {
                Clients = items,
                PaginatedList = paginatedList
            };

            return (allClientsModel);
        }

        public async Task<IEnumerable<Client>> GetAllClientsByUserId(string userId)
        {
            var clients = await context.Clients.Where(c => c.UserId == userId).ToListAsync();
            return clients;
        }

        public Client GetClientById(int id)
        {
            var client = context.Clients
                .Include(c => c.Doctor)
                .Include(c => c.Procedure)
                .FirstOrDefault(c => c.Id == id);

            return client;
        }

        public bool DeleteClient(int Id)
        {
            var client = context.Clients.FirstOrDefault(c => c.Id == Id);
            if (client != null)
            {
                context.Clients.Remove(client);
                context.SaveChanges();
                return true;
            }
            return false;
        }

        public bool UpdateClient(Client model)
        {
            if (model != null)
            {
                context.Clients.Update(model);
                context.SaveChanges();
                return true;
            }

            return false;
        }
    }
}
