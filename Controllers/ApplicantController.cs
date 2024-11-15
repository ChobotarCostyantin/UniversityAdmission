using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using UniversityAdmission.Data.Repos;
using UniversityAdmission.Models.DTO;
using UniversityAdmission.Models.Entities;

namespace UniversityAdmission.Controllers
{
    [Authorize]
    public class ApplicantController : Controller
    {
        private readonly ApplicantRepository _applicantRepository;
        private readonly SpecialityRepository _specialityRepository;

        public ApplicantController(ApplicantRepository applicantRepository, SpecialityRepository specialityRepository)
        {
            _applicantRepository = applicantRepository;
            _specialityRepository = specialityRepository;
        }

        public async Task<IActionResult> Index(string searchString, string sortOrder, ObjectId? specialityFilter)
        {
            ViewData["NameSortParam"] = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DateOfBirthSortParam"] = sortOrder == "dateOfBirth" ? "dateOfBirth_desc" : "dateOfBirth";
            ViewData["CurrentFilter"] = searchString;
            ViewData["CurrentSpecialityFilter"] = specialityFilter;

            var applicants = await _applicantRepository.GetAll();
            var specialities = await _specialityRepository.GetAll();
            var specialityDict = specialities.ToDictionary(s => s.Id, s => s);
            ViewBag.Specialities = specialityDict;
            ViewBag.SpecialitiesList = specialities;

            // Виконуємо фільтрацію по спеціальності
            if (specialityFilter.HasValue && specialityFilter.Value != ObjectId.Empty)
            {
                applicants = applicants.Where(a => a.SpecialityId == specialityFilter.Value).ToList();
            }

            if (!string.IsNullOrEmpty(searchString))
            {
                applicants = applicants.Where(a =>
                    a.FullName.Contains(searchString, StringComparison.OrdinalIgnoreCase) ||
                    a.PhoneNumber.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            applicants = sortOrder switch
            {
                "name_desc" => applicants.OrderByDescending(a => a.FullName).ToList(),
                "dateOfBirth" => applicants.OrderBy(a => a.DateOfBirth).ToList(),
                "dateOfBirth_desc" => applicants.OrderByDescending(a => a.DateOfBirth).ToList(),
                _ => applicants.OrderBy(a => a.FullName).ToList()
            };

            return View(applicants);
        }

        [HttpGet]
        [Authorize(Policy = "Operator")]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Policy = "Operator")]
        public async Task<IActionResult> Add(ApplicantDTO dto)
        {
            await _applicantRepository.Create(dto);
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(Policy = "Operator")]
        public async Task<IActionResult> Edit(ObjectId id)
        {
            if (id == ObjectId.Empty)
            {
                return NotFound();
            }

            var selectedApplicant = await _applicantRepository.GetById(id);
            if (selectedApplicant == null)
            {
                return NotFound();
            }

            var applicant = new ApplicantDTO
            {
                Id = selectedApplicant.Id,
                FullName = selectedApplicant.FullName,
                DateOfBirth = selectedApplicant.DateOfBirth,
                PhoneNumber = selectedApplicant.PhoneNumber,
                Address = selectedApplicant.Address,
                IsBeneficiary = selectedApplicant.IsBeneficiary,
                SpecialityId = selectedApplicant.SpecialityId
            };
            return View(applicant);
        }

        [HttpPost]
        [Authorize(Policy = "Operator")]
        public async Task<IActionResult> Edit(ApplicantDTO dto)
        {
            await _applicantRepository.Update(dto);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize(Policy = "Operator")]
        public async Task<IActionResult> Delete(ObjectId id)
        {
            await _applicantRepository.DeleteByIdAsync(id);
            return RedirectToAction("Index");
        }
    }
}