using DentistApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentistApp.Data
{
    public class AplicationContext : IdentityDbContext<IdentityUser>
    {
        public DbSet<Client> Clients { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Procedure> Procedures { get; set; }
        public AplicationContext(DbContextOptions<AplicationContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder model)
        {
            base.OnModelCreating(model);
            var foreignKeys = model.Model.GetEntityTypes().SelectMany(fk => fk.GetForeignKeys());
            foreach (var foreignKey in foreignKeys)
            {
                foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
            }

        }
    }
}
