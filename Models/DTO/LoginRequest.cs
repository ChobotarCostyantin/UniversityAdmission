using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace UniversityAdmission.Models.DTO
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "Заповніть поле."),]
        [EmailAddress(ErrorMessage = "Некоректна адреса електронної пошти.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Заповніть поле."),]
        [DataType(DataType.Password)]
        [Remote(action: "ValidatePassword", controller: "Validation", AdditionalFields = nameof(Email), ErrorMessage = "Невірний пароль.")]
        public string Password { get; set; } = string.Empty;
    }
}