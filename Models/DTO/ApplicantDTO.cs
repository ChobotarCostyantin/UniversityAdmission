using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace UniversityAdmission.Models.DTO
{
    public class ApplicantDTO
    {
        public ObjectId Id { get; set; }

        [Required(ErrorMessage = "Заповніть поле.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "ПІБ має містити від 3 до 100 символів.")]
        public required string FullName { get; set; }

        [Required(ErrorMessage = "Виберіть дату народження")]
        [Remote(action: "ValidateDateOfBirth", controller: "Applicant", ErrorMessage = "Вік абітурієнта повинен бути від 16 до 80 років")]
        public required DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Заповніть поле.")]
        [RegularExpression(@"^[\+]?[(]?[0-9]{3}[)]?[-\s\.]?[0-9]{3}[-\s\.]?[0-9]{4,6}$", ErrorMessage = "Введіть номер телефону у форматі +380XXXXXXXXX")]
        [DataType(DataType.PhoneNumber)]
        public required string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Заповніть поле.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Адреса має містити від 3 до 100 символів.")]
        public required string Address { get; set; }
        public bool IsBeneficiary { get; set; } = false;

        [Required(ErrorMessage = "Виберіть спеціальність.")]
        public required ObjectId SpecialityId { get; set; }
    }
}