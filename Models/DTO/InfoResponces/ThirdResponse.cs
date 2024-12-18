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
        public List<SelectOptionObject> ResponseOptions { get; set; } = new();
        public List<RequiredExam> RequiredExams { get; set; } = new();
    }
}