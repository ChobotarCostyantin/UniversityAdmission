using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniversityAdmission.Models.Entities;

namespace UniversityAdmission.Models.DTO.InfoResponces
{
    public class FirstResponse
    {
        public List<SelectOptionFirst> ResponseOptions { get; set; } = new();
        public List<Group> Groups { get; set; } = new();
    }

    public class SelectOptionFirst
    {
        public string Value { get; set; } = string.Empty;
        public bool IsSelected { get; set; }
    }
}