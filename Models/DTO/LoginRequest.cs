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
        [Required(ErrorMessage = "Заповніть поле.")]
        public string Login { get; set; } = string.Empty;

        [Required(ErrorMessage = "Заповніть поле."),]
        [DataType(DataType.Password)]
        [Remote(action: "ValidatePassword", controller: "Validation", AdditionalFields = nameof(Login), ErrorMessage = "Невірний логін або пароль.")]
        public string Password { get; set; } = string.Empty;
    }
}