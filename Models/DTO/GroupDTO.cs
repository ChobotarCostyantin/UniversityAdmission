using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace UniversityAdmission.Models.DTO
{
    public class GroupDTO
    {
        public ObjectId Id { get; set; }

        [Required(ErrorMessage = "Заповніть поле.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Довжина повинна бути від 3 до 100 символів")]
        public required string Name { get; set; }

        [Required(ErrorMessage = "Виберіть дату проведення.")]
        [Remote(action: "ValidateExamDate", controller: "Validation", ErrorMessage = "Дата не може бути в минулому.")]
        public required DateTime ExamDate { get; set; }
    }
}