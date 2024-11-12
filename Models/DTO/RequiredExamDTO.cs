using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace UniversityAdmission.Models.DTO
{
    public class RequiredExamDTO
    {
        public ObjectId Id { get; set; }
        public ObjectId SpecialityId { get; set; }

        [Required(ErrorMessage = "Виберіть екзамен.")]
        public ObjectId ExamId { get; set; }
    }
}