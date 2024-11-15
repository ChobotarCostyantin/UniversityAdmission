using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using UniversityAdmission.Data.Repos;
using UniversityAdmission.Models.DTO;
using UniversityAdmission.ViewModels;

namespace UniversityAdmission.Controllers
{
    // [Route("Group/{groupId}/GroupApplicant")]
    public class GroupApplicantController : Controller
    {
        private readonly ApplicantRepository _applicantRepository;
        private readonly GroupExamRepository _groupExamRepository;
        private readonly RequiredExamRepository _requiredExamRepository;
        private readonly GroupApplicantRepository _groupApplicantRepository;
        private readonly SpecialityRepository _specialityRepository;
        private readonly GroupRepository _groupRepository;

        public GroupApplicantController(GroupApplicantRepository groupApplicantRepository, GroupRepository groupRepository,
            SpecialityRepository specialityRepository, ApplicantRepository applicantRepository, RequiredExamRepository requiredExamRepository,
            GroupExamRepository groupExamRepository)
        {
            _groupApplicantRepository = groupApplicantRepository;
            _groupRepository = groupRepository;
            _specialityRepository = specialityRepository;
            _applicantRepository = applicantRepository;
            _requiredExamRepository = requiredExamRepository;
            _groupExamRepository = groupExamRepository;
        }

        [Route("Group/{groupId}/GroupApplicant")]
        public async Task<IActionResult> Index(ObjectId groupId, string searchString, string sortOrder, ObjectId? specialityFilter)
        {
            if (groupId == ObjectId.Empty)
            {
                return NotFound();
            }

            var selectedGroup = await _groupRepository.GetById(groupId);
            if (selectedGroup == null)
            {
                return NotFound();
            }

            ViewData["NameSortParam"] = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DateOfBirthSortParam"] = sortOrder == "dateOfBirth" ? "dateOfBirth_desc" : "dateOfBirth";
            ViewData["CurrentFilter"] = searchString;
            ViewData["CurrentSpecialityFilter"] = specialityFilter;

            var groupApplicants = await _groupApplicantRepository.GetGroupApplicantsFromGroup(groupId);
            var specialities = await _specialityRepository.GetAll();
            var specialityDict = specialities.ToDictionary(s => s.Id, s => s);
            ViewBag.Specialities = specialityDict;
            ViewBag.SpecialitiesList = specialities;

            if (specialityFilter.HasValue && specialityFilter.Value != ObjectId.Empty)
            {
                groupApplicants = groupApplicants.Where(a => a.Applicant!.SpecialityId == specialityFilter.Value).ToList();
            }

            if (!string.IsNullOrEmpty(searchString))
            {
                groupApplicants = groupApplicants.Where(a =>
                    a.Applicant!.FullName.Contains(searchString, StringComparison.OrdinalIgnoreCase) ||
                    a.Applicant!.PhoneNumber.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            groupApplicants = sortOrder switch
            {
                "name_desc" => groupApplicants.OrderByDescending(a => a.Applicant!.FullName).ToList(),
                "dateOfBirth" => groupApplicants.OrderBy(a => a.Applicant!.DateOfBirth).ToList(),
                "dateOfBirth_desc" => groupApplicants.OrderByDescending(a => a.Applicant!.DateOfBirth).ToList(),
                _ => groupApplicants.OrderBy(a => a.Applicant!.FullName).ToList()
            };

            var model = new GroupAndGroupApplicantsViewModel
            {
                Group = selectedGroup,
                GroupApplicants = groupApplicants
            };
            
            return View(model);
        }

        [HttpGet]
        [Route("Group/{groupId}/GroupApplicant/Add")]
        public async Task<IActionResult> Add(ObjectId groupId)
        {
            var groupExams = await _groupExamRepository.GetGroupExamsFromGroup(groupId);
            var groupExamIds = groupExams.Select(x => x.ExamId).ToHashSet();

            var applicants = await _applicantRepository.GetAll();
            var requiredExams = await _requiredExamRepository.GetAll();

            var availableApplicants = applicants.Where(applicant =>
            {
                var applicantRequiredExams = requiredExams
                    .Where(re => re.SpecialityId == applicant.SpecialityId)
                    .Select(re => re.ExamId)
                    .ToHashSet();

                return applicantRequiredExams.Overlaps(groupExamIds);
            }).ToList();

            var groupApplicant = new GroupApplicantDTO
            {
                GroupId = groupId,
                AvailableApplicants = availableApplicants
            };

            return View(groupApplicant);
        }

        [HttpPost]
        [Route("Group/{groupId}/GroupApplicant/Add")]
        public async Task<IActionResult> Add(ObjectId groupId, GroupApplicantDTO dto)
        {
            await _groupApplicantRepository.Create(dto);
            return RedirectToAction("Index", new { groupId }); 
        }

        [Route("Group/{groupId}/GroupApplicant/Delete/{groupApplicantId}")]
        public async Task<IActionResult> Delete(ObjectId groupId, ObjectId groupApplicantId)
        {
            await _groupApplicantRepository.DeleteByIdAsync(groupApplicantId);
            return RedirectToAction("Index", new { groupId });
        }
    }
}