using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniversityAdmission.Models.Entities;

namespace UniversityAdmission.ViewModels
{
    public class ApplicantAndExamResultsViewModel
    {
        public Applicant Applicant { get; set; } = null!;
        public List<ExamResult> ExamResults { get; set; } = new();
    }
}