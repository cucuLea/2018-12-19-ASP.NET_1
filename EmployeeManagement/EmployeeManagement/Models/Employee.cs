using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;


namespace EmployeeManagement.Models
{
    public enum Gender {F,M}
    public class Employee
    {
        
        public int ID { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public Gender? Gender { get; set; }

        [DataType(DataType.Date)]
        public DateTime Birth { get; set; }

        public string Department { get; set; }

        public string Address { get; set; }

        [RegularExpression(@"^\d{3,11}$",
            ErrorMessage = "please input with number and the length should between 3 and 11")]
        public string Phone { get; set;}

        [EmailAddress]
        [Remote(action: "VerifyEmail", controller: "Validation", AdditionalFields = nameof(ID))]
        public string Email { get; set; }
    }
}
