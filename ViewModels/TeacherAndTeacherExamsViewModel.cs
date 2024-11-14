using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniversityAdmission.Models.Entities;

namespace UniversityAdmission.ViewModels
{
    public class TeacherAndTeacherExamsViewModel
    {
        public Teacher Teacher { get; set; } = null!;
        public List<TeacherExam> TeacherExams { get; set; } = new();
    }
}