using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using UniversityAdmission.Models.Entities;

namespace UniversityAdmission.Models.DTO
{
    public class EditRequest
    {
        public ObjectId Id { get; set; }
        
        [Required(ErrorMessage = "Заповніть поле.")]
        [StringLength(20, ErrorMessage = "Логін повинен містити не більше 20 символів.")]
        public required string Login { get; set; }

        [Required(ErrorMessage = "Заповніть поле.") ]
        [EmailAddress(ErrorMessage = "Некоректна адреса електронної пошти.")]
        [StringLength(50, ErrorMessage = "Адреса електронної пошти повинна містити не більше 50 символів.")]
        [DataType(DataType.EmailAddress)]
        [Remote(action: "ValidateEmailExceptUser", controller: "Validation", ErrorMessage = "Така адреса електронної пошти вже зареєстрована.")]
        public required string Email { get; set; }
        
        [Required(ErrorMessage = "Виберіть роль.")]
        [EnumDataType(typeof(Roles), ErrorMessage = "Некоректна роль.")]
        public Roles Role { get; set; }

        [Required(ErrorMessage = "Заповніть поле.")]
        [DataType(DataType.Password)]
        public required string Password { get; set; }
    }
}