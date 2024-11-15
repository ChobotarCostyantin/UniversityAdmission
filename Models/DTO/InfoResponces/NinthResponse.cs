using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniversityAdmission.Models.Entities;

namespace UniversityAdmission.Models.DTO.InfoResponces
{
    public class NinthResponse
    {
        public List<SelectOptionObject> ResponseOptions { get; set; } = new();
        public List<Teacher> Teachers { get; set; } = new();
    }
}