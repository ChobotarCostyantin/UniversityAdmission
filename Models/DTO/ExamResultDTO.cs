using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using UniversityAdmission.Models.Entities;

namespace UniversityAdmission.Models.DTO
{
    public class ExamResultDTO
    {
        public ObjectId Id { get; set; }
        public ObjectId ApplicantId { get; set; }

        [Required(ErrorMessage = "Виберіть екзамен.")]
        public ObjectId ExamId { get; set; }

        [Required(ErrorMessage = "Заповніть поле.")]
        [Range(0, 100, ErrorMessage = "Введіть число від 0 до 100.")]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "Введть число від 0 до 100")]
        public uint Score { get; set; }
    }
}