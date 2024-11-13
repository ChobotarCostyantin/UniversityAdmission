using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using UniversityAdmission.Models.Entities;

namespace UniversityAdmission.Models.DTO
{
    public class ExamDTO
    {
        public ObjectId Id { get; set; }

        [Required(ErrorMessage = "Заповніть поле.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Назва має містити від 3 до 100 символів.")]
        public required string Name { get; set; }

        [Required(ErrorMessage = "Виберіть предмет.")]
        public required Subjects Subject { get; set; }

        [Required(ErrorMessage = "Заповніть поле.")]
        [Range(1, 100, ErrorMessage = "Мінімальний бал має бути від 1 до 100")]
        public required uint MinScore { get; set; }

        public bool IsCreativeContest { get; set; } = false;
    }
}