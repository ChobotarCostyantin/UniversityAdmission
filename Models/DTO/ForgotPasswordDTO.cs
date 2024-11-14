using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace UniversityAdmission.Models.DTO
{
    public class ForgotPasswordDTO
    {
        [Required(ErrorMessage = "Заповніть поле.")]
        [StringLength(20, ErrorMessage = "Логін повинен містити не більше 20 символів.")]
        [Remote(action: "ValidateIfLoginExists", controller: "Validation", ErrorMessage = "Такого логіну не існує.")]
        public required string Login { get; set; }
        public string Password = string.Empty;
    }
}