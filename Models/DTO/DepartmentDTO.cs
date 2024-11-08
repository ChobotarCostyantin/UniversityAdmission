using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using UniversityAdmission.Models.Entities;

namespace UniversityAdmission.Models.DTO
{
    public class DepartmentDTO
    {
        public ObjectId Id { get; set; }

        [Required(ErrorMessage = "Назва обов'язкова")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Назва має містити від 3 до 100 символів")]
        public required string Name { get; set; }

        [Required(ErrorMessage = "Опис обов'язковий")]
        [StringLength(1000, MinimumLength = 3, ErrorMessage = "Опис має містити від 3 до 1000 символів")]
        public required string Description { get; set; }

        [Required(ErrorMessage = "Факультет обов'язковий")]
        public required Faculty Faculty { get; set; }
    }
}