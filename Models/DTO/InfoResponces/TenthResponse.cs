using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniversityAdmission.Models.Entities;

namespace UniversityAdmission.Models.DTO.InfoResponces
{
    public class TenthResponse
    {
        public string SpecialityName { get; set; } = string.Empty;
        public string SpecialityDescription { get; set; } = string.Empty;
        public string DepartmentName { get; set; } = string.Empty;
        public string FacultyName { get; set; } = string.Empty;
    }
}