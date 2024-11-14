using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniversityAdmission.Models.Entities;

namespace UniversityAdmission.ViewModels
{
    public class GroupAndGroupExamsViewModel
    {
        public Group Group { get; set; } = null!;
        public List<GroupExam> GroupExams { get; set; } = new();
    }
}