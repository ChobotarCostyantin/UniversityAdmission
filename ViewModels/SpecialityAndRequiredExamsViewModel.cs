using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniversityAdmission.Models.Entities;

namespace UniversityAdmission.ViewModels
{
    public class SpecialityAndRequiredExamsViewModel
    {
        public Speciality Speciality { get; set; }
        public List<RequiredExam> RequiredExams { get; set; }
    }
}