using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace UniversityAdmission.Models.DTO
{
    public class TeacherDTO
    {
        public ObjectId Id { get; set; }

        [Required(ErrorMessage = "Заповніть поле.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Довжина повинна бути від 2 до 100 символів")]
        public required string FullName { get; set; }

        [Required(ErrorMessage = "Виберіть дату народження")]
        [Remote(action: "ValidateDateOfBirth", controller: "Validation", ErrorMessage = "Вік викладача повинен бути від 16 до 80 років")]
        public required DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Заповніть поле.")]
        [RegularExpression(@"^[\+]?[(]?[0-9]{3}[)]?[-\s\.]?[0-9]{3}[-\s\.]?[0-9]{4,6}$", ErrorMessage = "Введіть номер телефону у форматі +380XXXXXXXXX")]
        [DataType(DataType.PhoneNumber)]
        public required string PhoneNumber { get; set; }
    }
}