using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UniversityAdmission.Models.DTO.InfoResponces
{
    public class SelectOptionResponse
    {
        public string Value { get; set; } = string.Empty;
        public bool IsSelected { get; set; }
    }
}