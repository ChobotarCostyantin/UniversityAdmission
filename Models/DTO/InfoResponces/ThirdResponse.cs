using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using UniversityAdmission.Models.Entities;

namespace UniversityAdmission.Models.DTO.InfoResponces
{
    public class ThirdResponse
    {
        public List<SelectOptionThird> ResponseOptions { get; set; } = new();
        public List<RequiredExam> RequiredExams { get; set; } = new();
    }

    public class SelectOptionThird
    {
        public ObjectId Id { get; set; }
        public string Value { get; set; } = string.Empty;
        public bool IsSelected { get; set; }
    }
}