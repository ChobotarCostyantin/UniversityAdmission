using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UniversityAdmission.Dtos
{
    public class RegisterRequest
    {
        [Required]
        public string Login { get; set; } = string.Empty;

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required, DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
        
        [Required, DataType(DataType.Password), Compare(nameof(Password), ErrorMessage = "Паролі не співпадають." )]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}