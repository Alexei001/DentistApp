using DentistApp.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentistApp.Data
{
    public class InitialData
    {
        public static void Seed(IApplicationBuilder applicationBuilder)
        {

            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<AplicationContext>();
                //context.Database.EnsureDeleted();
                //context.Database.EnsureCreated();

                if (!context.Clients.Any())
                {
                    var clients = new List<Client>()
                    {
                    new Client(){ Name="John Smith",Email="JohnSmith@gmail.com",
                        PhoneNumber="3736255648",
                        Comment="Some Client Comment",
                        Available=new DateTime(2021,11,15,8,0,0),
                        Doctor=new Doctor (){Name=Models.Enum.EnumDoc.Doctor1},
                        Procedure=new Procedure(){Name=Models.Enum.EnumProcedure.A}},

                    new Client(){ Name="John Alexis",Email="John@gmail.com",
                     PhoneNumber="3736255648",
                        Comment="Some Client Comment",
                        Available=new DateTime(2021,11,1,10,0,0),
                        Procedure=new Procedure(){Name=Models.Enum.EnumProcedure.C},
                        Doctor=new Doctor (){Name=Models.Enum.EnumDoc.Doctor2}},

                    new Client(){ Name="Jim Kerry",Email="SomeEmail@gmail.com",
                     PhoneNumber="3736255648",
                        Comment="Some Client Comment",
                        Available=new DateTime(2021,11,18,11,0,0),
                        Procedure=new Procedure(){Name=Models.Enum.EnumProcedure.D},
                        Doctor=new Doctor (){Name=Models.Enum.EnumDoc.Doctor4}},
                    };
                    context.Clients.AddRange(clients);
                    context.SaveChanges();
                }
            }
        }
    }
}
