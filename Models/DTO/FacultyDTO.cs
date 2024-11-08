using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace UniversityAdmission.Models.DTO
{
    public class FacultyDTO
    {
        public ObjectId Id { get; set; }
        
        [Required(ErrorMessage = "Назва обов'язкова")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Назва має містити від 3 до 100 символів")]
        public required string Name { get; set; }

        [Required(ErrorMessage = "Опис обов'язковий")]
        [StringLength(1000, MinimumLength = 3, ErrorMessage = "Опис має містити від 3 до 1000 символів")]
        public required string Description { get; set; }
    }
}