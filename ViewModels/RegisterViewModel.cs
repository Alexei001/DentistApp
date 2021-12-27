using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DentistApp.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Email is required")]
        
        [Remote(action: "IsEmailInUse", controller: "Account")]
        [DataType(DataType.EmailAddress,ErrorMessage ="Invalid Email Adress")]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password,ErrorMessage ="Invalid Password")]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Invalid Confirm Password")]
        public string ConfirmPassword { get; set; }
    }


}
