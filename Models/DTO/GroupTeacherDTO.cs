using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace UniversityAdmission.Models.DTO
{
    public class GroupTeacherDTO
    {
        public ObjectId Id { get; set; }
        public ObjectId GroupId { get; set; }

        [Required(ErrorMessage = "Виберіть викладача.")]
        public ObjectId TeacherId { get; set; }
    }
}