using DentistApp.Models.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DentistApp.Models
{
    public class Doctor
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Doctor is Required")]
        public EnumDoc Name { get; set; }
        public List<Client> Clients { get; set; }
    }
}
