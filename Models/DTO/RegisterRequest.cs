using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UniversityAdmission.Models.Entities;

namespace UniversityAdmission.Models.DTO
{
    public class RegisterRequest
    {
        [Required(ErrorMessage = "Заповніть поле.")]
        [StringLength(20, ErrorMessage = "Логін повинен містити не більше 20 символів.")]
        [Remote("ValidateLogin", "Validation", ErrorMessage = "Такий логін вже зареєстрований.")]
        public string Login { get; set; } = string.Empty;

        [Required(ErrorMessage = "Заповніть поле.") ]
        [EmailAddress(ErrorMessage = "Некоректна адреса електронної пошти.")]
        [StringLength(50, ErrorMessage = "Адреса електронної пошти повинна містити не більше 50 символів.")]
        [DataType(DataType.EmailAddress)]
        [Remote("ValidateEmail", "Validation", ErrorMessage = "Така адреса електронної пошти вже зареєстрована.")]
        public string Email { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Виберіть роль.")]
        public Roles Role { get; set; }

        [Required(ErrorMessage = "Заповніть поле.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Заповніть поле.") ]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Паролі не співпадають." )]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}