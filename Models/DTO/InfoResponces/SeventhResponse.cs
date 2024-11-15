using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniversityAdmission.Models.Entities;

namespace UniversityAdmission.Models.DTO.InfoResponces
{
    public class SeventhResponse
    {
        public List<SelectOptionResponse> ResponseOptions { get; set; } = new();
        public List<Applicant> Applicants { get; set; } = new();
        public int Count { get; set; }
    }
}