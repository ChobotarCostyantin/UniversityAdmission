using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniversityAdmission.Models.Entities;

namespace UniversityAdmission.Models.DTO.InfoResponces
{
    public class FourthResponse
    {
        public List<SelectOptionObject> ResponseOptions { get; set; } = new();
        public List<ExamAverageScoreViewModel> ExamAverages { get; set; } = new();
    }

    public class ExamAverageScoreViewModel
    {
        public RequiredExam RequiredExam { get; set; } = null!;
        public double AverageScore { get; set; }
    }
}