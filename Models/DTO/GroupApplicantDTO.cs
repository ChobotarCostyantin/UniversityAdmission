using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using UniversityAdmission.Models.Entities;

namespace UniversityAdmission.Models.DTO
{
    public class GroupApplicantDTO
    {
        public ObjectId Id { get; set; }
        public ObjectId GroupId { get; set; }

        public List<Applicant> AvailableApplicants { get; set; } = new();

        [Required(ErrorMessage = "Виберіть абітурієнта.")]
        public ObjectId ApplicantId { get; set; }
    }
}