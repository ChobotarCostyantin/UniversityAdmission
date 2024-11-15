using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using UniversityAdmission.Models.Entities;

namespace UniversityAdmission.Models.DTO.InfoResponces
{
    public class SixthResponse
    {
        public List<SelectOptionSixth> ResponseOptions { get; set; } = new();
        public List<Faculty> Faculties { get; set; } = new();
        public List<Speciality> Specialities { get; set; } = new();
        public ObjectId? SelectedObjectId { get; set; }
        public List<Applicant> Applicants { get; set; } = new();
    }
    
    public class SelectOptionSixth
    {
        public string Value { get; set; } = string.Empty;
        public bool IsResponseSelected { get; set; }
    }
}