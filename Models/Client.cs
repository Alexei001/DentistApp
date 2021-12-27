using DentistApp.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DentistApp.Models
{
    public class Client
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is Required")]
        public string Name { get; set; }

        public string Email { get; set; }

        public string Comment { get; set; }
        [Required(ErrorMessage = "Phone Number is Required")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "Invalid phone number")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Doctor availability is Required")]
        public DateTime Available { get; set; }

        public int ProcedureId { get; set; }
        public Procedure Procedure { get; set; }

        public int DoctorId { get; set; }
        public Doctor Doctor { get; set; }


        public string UserId { get; set; }

        public bool Notify { get; set; }
    }
}
