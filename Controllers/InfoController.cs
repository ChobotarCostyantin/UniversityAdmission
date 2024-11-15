using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using UniversityAdmission.Data.Repos;
using UniversityAdmission.Models.DTO.InfoResponces;
using UniversityAdmission.Models.Entities;

namespace UniversityAdmission.Controllers
{
    [Authorize]
    public class InfoController : Controller
    {
        private readonly GroupRepository _groupRepository;
        private readonly GroupExamRepository _groupExamRepository;
        private readonly ExamRepository _examRepository;
        private readonly GroupApplicantRepository _groupApplicantRepository;
        private readonly ApplicantRepository _applicantRepository;
        private readonly RequiredExamRepository _requiredExamRepository;
        private readonly SpecialityRepository _specialityRepository;
        private readonly ExamResultRepository _examResultRepository;
        private readonly FacultyRepository _facultyRepository;
        private readonly DepartmentRepository _departmentRepository;
        private readonly TeacherRepository _teacherRepository;
        public InfoController(GroupRepository groupRepository, GroupExamRepository groupExamRepository,
            ExamRepository examRepository, GroupApplicantRepository groupApplicantRepository, ApplicantRepository applicantRepository,
            RequiredExamRepository requiredExamRepository, SpecialityRepository specialityRepository,
            ExamResultRepository examResultRepository, FacultyRepository facultyRepository, DepartmentRepository departmentRepository,
            TeacherRepository teacherRepository)
        {
            _groupExamRepository = groupExamRepository;
            _groupRepository = groupRepository;
            _examRepository = examRepository;
            _groupApplicantRepository = groupApplicantRepository;
            _applicantRepository = applicantRepository;
            _requiredExamRepository = requiredExamRepository;
            _specialityRepository = specialityRepository;
            _examResultRepository = examResultRepository;
            _facultyRepository = facultyRepository;
            _departmentRepository = departmentRepository;
            _teacherRepository = teacherRepository;
        }

        [HttpGet]
        public async Task<IActionResult> First(string responseType = "3 екзамени")
        {
            var groups = await _groupRepository.GetAll();
            var groupExams = await _groupExamRepository.GetAll();
            var exams = await _examRepository.GetAll();
            var response = new FirstResponse
            {
                ResponseOptions =
                [
                    new() { Value = "3 екзамени", IsSelected = responseType == "3 екзамени" },
                    new() { Value = "більше 3 екзаменів", IsSelected = responseType == "більше 3 екзаменів" },
                    new() { Value = "творчий конкурс", IsSelected = responseType == "творчий конкурс" }
                ]
            };

            switch (responseType)
            {
                case "3 екзамени":
                    response.Groups = groups
                        .Where(g => groupExams.Count(ge => ge.GroupId == g.Id) == 3)
                        .ToList();
                    break;
                case "більше 3 екзаменів":
                    response.Groups = groups
                        .Where(g => groupExams.Count(ge => ge.GroupId == g.Id) > 3)
                        .ToList();
                    break;
                case "творчий конкурс":
                    response.Groups = [];
                    foreach (var group in groups)
                    {
                        var examsFromGroup = await _groupExamRepository.GetGroupExamsFromGroup(group.Id);
                        if (examsFromGroup.Any(ge => ge.Exam?.IsCreativeContest == true))
                        {
                            response.Groups.Add(group);
                        }
                    }
                    break;
                default:
                    response.Groups = groups;
                    break;
            }
            return View(response);
        }

        [HttpGet]
        public async Task<IActionResult> Second()
        {
            var groups = await _groupRepository.GetAll();
            var groupApplicants = await _groupApplicantRepository.GetAll();
            var applicants = await _applicantRepository.GetAll();

            var result = new List<Group>();

            foreach (var group in groups)
            {
                var groupApplicantsFromGroup = await _groupApplicantRepository.GetGroupApplicantsFromGroup(group.Id);

                var distinctSpecialties = groupApplicantsFromGroup
                    .Select(ga => applicants.FirstOrDefault(a => a.Id == ga.ApplicantId)?.SpecialityId)
                    .Distinct()
                    .ToList();

                if (distinctSpecialties.Count > 1)
                {
                    result.Add(group);
                }
            }

            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> Third(ObjectId? specialityId = null)
        {
            var specialities = await _specialityRepository.GetAll();
            if (specialityId == null && specialities.Any())
            {
                specialityId = specialities.First().Id;
            }

            var selectOption = new List<SelectOptionObject>();
            foreach (var speciality in specialities)
            {
                selectOption.Add(new SelectOptionObject()
                {
                    Id = speciality.Id,
                    Value = speciality.Name,
                    IsSelected = speciality.Id == specialityId
                });
            }

            var requiredExams = new List<RequiredExam>();
            if (specialityId.HasValue)
            {
                requiredExams = await _requiredExamRepository.GetRequiredExamsFromSpeciality(specialityId.Value);
            }

            var response = new ThirdResponse()
            {
                ResponseOptions = selectOption,
                RequiredExams = requiredExams
            };
            return View(response);
        }

        [HttpGet]
        public async Task<IActionResult> Fourth(ObjectId? specialityId = null)
        {
            var specialities = await _specialityRepository.GetAll();
            if (specialityId == null && specialities.Any())
            {
                specialityId = specialities.First().Id;
            }

            var selectOption = new List<SelectOptionObject>();
            foreach (var speciality in specialities)
            {
                selectOption.Add(new SelectOptionObject()
                {
                    Id = speciality.Id,
                    Value = speciality.Name,
                    IsSelected = speciality.Id == specialityId!.Value
                });
            }

            var examAverages = new List<ExamAverageScoreViewModel>();
            if (specialityId.HasValue)
            {
                var requiredExams = await _requiredExamRepository.GetRequiredExamsFromSpeciality(specialityId.Value);
                var averageScores = await _examResultRepository.GetAverageScoresBySpeciality(specialityId.Value);

                examAverages = requiredExams.Select(exam =>
                {
                    if (averageScores.TryGetValue(exam.ExamId, out double averageScore))
                    {
                        return new ExamAverageScoreViewModel
                        {
                            RequiredExam = exam,
                            AverageScore = averageScore
                        };
                    }
                    else
                    {
                        return new ExamAverageScoreViewModel
                        {
                            RequiredExam = exam,
                            AverageScore = 0
                        };
                    }
                }).ToList();
            }


            var response = new FourthResponse()
            {
                ResponseOptions = selectOption,
                ExamAverages = examAverages
            };
            return View(response);
        }

        [HttpGet]
        public async Task<IActionResult> Fifth(string responseType = "по факультету")
        {
            var applicants = await _applicantRepository.GetAll();
            var faculties = await _facultyRepository.GetAll();
            var departments = await _departmentRepository.GetAll();
            var specialities = await _specialityRepository.GetAll();

            var response = new FifthResponce
            {
                ResponseOptions =
                [
                    new() { Value = "по факультету", IsSelected = responseType == "по факультету" },
                    new() { Value = "за спеціальностями", IsSelected = responseType == "за спеціальностями" },
                    new() { Value = "за спеціальностями на які було подано менше 10 заяв", IsSelected = responseType == "за спеціальностями на які було подано менше 10 заяв" }
                ]
            };

            var result = new Dictionary<string, int>();
            switch (responseType)
            {
                case "по факультету":
                    foreach (var faculty in faculties)
                    {
                        var facultyDepartments = departments.Where(x => x.FacultyId == faculty.Id).ToList();
                        var facultySpecialities = specialities.Where(x => facultyDepartments.Any(y => y.Id == x.DepartmentId)).ToList();
                        var numberOfApplicants = applicants.Where(x => facultySpecialities.Any(y => y.Id == x.SpecialityId)).Count();
                        result.Add(faculty.Name, numberOfApplicants);
                    }
                    response.Result = result;
                    break;
                case "за спеціальностями":
                    foreach (var speciality in specialities)
                    {
                        var numberOfApplicants = applicants.Where(x => x.SpecialityId == speciality.Id).Count();
                        result.Add(speciality.Name, numberOfApplicants);
                    }
                    response.Result = result;
                    break;
                case "за спеціальностями на які було подано менше 10 заяв":
                    foreach (var speciality in specialities)
                    {
                        var numberOfApplicants = applicants.Where(x => x.SpecialityId == speciality.Id).Count();
                        if (numberOfApplicants < 10)
                        {
                            result.Add(speciality.Name, numberOfApplicants);
                        }
                    }
                    response.Result = result;
                    break;
            }

            return View(response);
        }

        [HttpGet]
        public async Task<IActionResult> Sixth(ObjectId? objectId = null, string responseType = "абітурієнти-пільговики")
        {
            var applicants = await _applicantRepository.GetAll();
            var faculties = await _facultyRepository.GetAll();
            var departments = await _departmentRepository.GetAll();
            var specialities = await _specialityRepository.GetAll();

            // If no objectId is selected, set default based on responseType
            if (objectId == null)
            {
                if (responseType == "заданий факультет" && faculties.Any())
                {
                    objectId = faculties.First().Id;
                }
                else if (responseType == "задану спеціальність" && specialities.Any())
                {
                    objectId = specialities.First().Id;
                }
            }

            var response = new SixthResponse
            {
                ResponseOptions =
                [
                    new() { Value = "заданий факультет", IsResponseSelected = responseType == "заданий факультет" },
                    new() { Value = "задану спеціальність", IsResponseSelected = responseType == "задану спеціальність" },
                    new() { Value = "абітурієнти-пільговики", IsResponseSelected = responseType == "абітурієнти-пільговики" }
                ],
                Faculties = faculties,
                Specialities = specialities,
                SelectedObjectId = objectId
            };

            var result = new List<Applicant>();
            switch (responseType)
            {
                case "заданий факультет":
                    foreach (var applicant in applicants)
                    {
                        var speciality = specialities.FirstOrDefault(x => x.Id == applicant.SpecialityId);
                        var department = departments.FirstOrDefault(x => x.Id == speciality!.DepartmentId);
                        var faculty = faculties.FirstOrDefault(x => x.Id == department!.FacultyId);
                        if (faculty!.Id == objectId)
                        {
                            result.Add(applicant);
                        }
                    }
                    response.Applicants = result;
                    break;
                case "задану спеціальність":
                    foreach (var applicant in applicants)
                    {
                        var speciality = specialities.FirstOrDefault(x => x.Id == applicant.SpecialityId);
                        if (speciality!.Id == objectId)
                        {
                            result.Add(applicant);
                        }
                    }
                    response.Applicants = result;
                    break;
                case "абітурієнти-пільговики":
                    foreach (var applicant in applicants)
                    {
                        if (applicant.IsBeneficiary)
                        {
                            result.Add(applicant);
                        }
                    }
                    response.Applicants = result;
                    break;
            }
            return View(response);
        }

        [HttpGet]
        public async Task<IActionResult> Seventh(string responseType = "загальна кількість")
        {
            var applicants = await _applicantRepository.GetAll();
            var requiredExams = await _requiredExamRepository.GetAll();

            var response = new SeventhResponse
            {
                ResponseOptions =
                [
                    new() { Value = "загальна кількість", IsSelected = responseType == "загальна кількість" },
                    new() { Value = "кількість де у спеціальностей співпадають вступні іспити", IsSelected = responseType == "кількість де у спеціальностей співпадають вступні іспити" }
                ]
            };

            switch (responseType)
            {
                case "загальна кількість":
                    response.Applicants = applicants.Where(a => a.TransferMechanism).ToList();
                    response.Count = applicants.Count(a => a.TransferMechanism);
                    break;
                case "кількість де у спеціальностей співпадають вступні іспити":
                    response.Applicants = applicants
                        .Where(a => a.TransferMechanism)
                        .Where(a =>
                        {
                            var applicantExams = _requiredExamRepository
                                .GetRequiredExamsFromSpeciality(a.SpecialityId)
                                .Result
                                .Select(r => r.ExamId)
                                .ToHashSet();

                            // Перевірити, чи є інші спеціальності з хоча б одним спільним іспитом
                            return requiredExams
                                .Where(r => r.SpecialityId != a.SpecialityId) // Інші спеціальності
                                .Select(r => r.ExamId)
                                .Any(examId => applicantExams.Contains(examId));
                        })
                        .ToList();
                    response.Count = response.Applicants.Count;
                    break;
            }

            return View(response);
        }

        [HttpGet]
        public async Task<IActionResult> Eighth(string responseType = "провалили вступні іспити")
        {
            var applicants = await _applicantRepository.GetAll();

            var response = new EighthResponse
            {
                ResponseOptions =
                [
                    new() { Value = "провалили вступні іспити", IsSelected = responseType == "провалили вступні іспити" },
                    new() { Value = "провалили один предмет", IsSelected = responseType == "провалили один предмет" },
                    new() { Value = "провалили два предмета", IsSelected = responseType == "провалили два предмета" },
                    new() { Value = "провалили творчий конкурс", IsSelected = responseType == "провалили творчий конкурс" }
                ]
            };

            var result = new List<Applicant>();
            switch (responseType)
            {
                case "провалили вступні іспити":
                    foreach (var applicant in applicants)
                    {
                        var examResultsFromApplicant = _examResultRepository.GetExamResultsFromApplicant(applicant.Id).Result;
                        if (examResultsFromApplicant.Count > 0 && examResultsFromApplicant.All(er => !er.Status))
                        {
                            result.Add(applicant);
                        }
                    }
                    response.Applicants = result;
                    break;
                case "провалили один предмет":
                    foreach (var applicant in applicants)
                    {
                        var examResultsFromApplicant = _examResultRepository.GetExamResultsFromApplicant(applicant.Id).Result;
                        if (examResultsFromApplicant.Count > 0 && examResultsFromApplicant.Where(x => !x.Status).Count() == 1)
                        {
                            result.Add(applicant);
                        }
                    }
                    response.Applicants = result;
                    break;
                case "провалили два предмета":
                    foreach (var applicant in applicants)
                    {
                        var examResultsFromApplicant = _examResultRepository.GetExamResultsFromApplicant(applicant.Id).Result;
                        if (examResultsFromApplicant.Count > 0 && examResultsFromApplicant.Where(x => !x.Status).Count() == 2)
                        {
                            result.Add(applicant);
                        }
                    }
                    response.Applicants = result;
                    break;
                case "провалили творчий конкурс":
                    foreach (var applicant in applicants)
                    {
                        var examResultsFromApplicant = _examResultRepository.GetExamResultsFromApplicant(applicant.Id).Result;
                        if (examResultsFromApplicant.Count > 0 && examResultsFromApplicant.Any(er => er.Exam!.IsCreativeContest && !er.Status))
                        {
                            result.Add(applicant);
                        }
                    }
                    response.Applicants = result;
                    break;
            }

            return View(response);
        }

        [HttpGet]
        public async Task<IActionResult> Ninth(ObjectId? examId = null)
        {
            var exams = await _examRepository.GetAll();
            var teachers = await _teacherRepository.GetAll();
            if (examId == null && exams.Any())
            {
                examId = exams.First().Id;
            }

            var selectOption = new List<SelectOptionObject>();
            foreach (var exam in exams)
            {
                selectOption.Add(new SelectOptionObject()
                {
                    Id = exam.Id,
                    Value = exam.Name,
                    IsSelected = exam.Id == examId!.Value
                });
            }

            var selectedExam = exams.FirstOrDefault(x => x.Id == examId);
            var result = teachers.Where(t => t.Subject == selectedExam!.Subject).ToList();

            var response = new NinthResponse
            {
                ResponseOptions = selectOption,
                Teachers = result
            };

            return View(response);
        }

        [HttpGet]
        public async Task<IActionResult> Tenth()
        {
            var requiredExams = await _requiredExamRepository.GetAll();
            var exams = await _examRepository.GetAll();
            var specialities = await _specialityRepository.GetAll();
            var departments = await _departmentRepository.GetAll();
            var faculties = await _facultyRepository.GetAll();

            var specialitiesWithMathExam = new List<TenthResponse>();
            foreach (var speciality in specialities)
            {
                var response = new TenthResponse();
                foreach (var requiredExam in requiredExams)
                {
                    if (requiredExam.SpecialityId == speciality.Id)
                    {
                        foreach (var exam in exams)
                        {
                            if (exam.Id == requiredExam.ExamId && exam.Subject == Subjects.Математика)
                            {
                                response.SpecialityName = speciality.Name;
                                response.SpecialityDescription = speciality.Description;
                                var department = departments.FirstOrDefault(d => d.Id == speciality.DepartmentId);
                                if (department != null)
                                {
                                    response.DepartmentName = department.Name;
                                    var faculty = faculties.FirstOrDefault(f => f.Id == department.FacultyId);
                                    if (faculty != null)
                                    {
                                        response.FacultyName = faculty.Name;
                                    }
                                }
                                break;
                            }
                        }
                        break;
                    }
                }
                if (!string.IsNullOrEmpty(response.DepartmentName))
                {
                    specialitiesWithMathExam.Add(response);
                }
            }

            return View(specialitiesWithMathExam);
        }
    }
}