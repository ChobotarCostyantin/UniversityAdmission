using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using UniversityAdmission.Data.Repos;
using UniversityAdmission.Models.DTO.InfoResponces;
using UniversityAdmission.Models.Entities;

namespace UniversityAdmission.Controllers
{
    public class InfoController : Controller
    {
        private readonly GroupRepository _groupRepository;
        private readonly GroupExamRepository _groupExamRepository;
        private readonly ExamRepository _examRepository;
        private readonly GroupApplicantRepository _groupApplicantRepository;
        private readonly ApplicantRepository _applicantRepository;
        private readonly RequiredExamRepository _requiredExamRepository;
        private readonly SpecialityRepository _specialityRepository;
        public InfoController(GroupRepository groupRepository, GroupExamRepository groupExamRepository,
            ExamRepository examRepository, GroupApplicantRepository groupApplicantRepository, ApplicantRepository applicantRepository,
            RequiredExamRepository requiredExamRepository, SpecialityRepository specialityRepository)
        {
            _groupExamRepository = groupExamRepository;
            _groupRepository = groupRepository;
            _examRepository = examRepository;
            _groupApplicantRepository = groupApplicantRepository;
            _applicantRepository = applicantRepository;
            _requiredExamRepository = requiredExamRepository;
            _specialityRepository = specialityRepository;
        }

        [HttpGet]
        public async Task<IActionResult> First(string responseType)
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
        public async Task<IActionResult> Third(ObjectId specialityId)
        {
            var specialities = await _specialityRepository.GetAll();
            var selectOption = new List<SelectOptionThird>();
            foreach (var speciality in specialities)
            {
                selectOption.Add(new SelectOptionThird()
                {
                    Id = speciality.Id,
                    Value = speciality.Name,
                    IsSelected = speciality.Id == specialityId
                });
            }
            var requiredExams = await _requiredExamRepository.GetRequiredExamsFromSpeciality(specialityId);

            var response = new ThirdResponse()
            {
                ResponseOptions = selectOption,
                RequiredExams = requiredExams
            };
            return View(response);
        }
    }
}