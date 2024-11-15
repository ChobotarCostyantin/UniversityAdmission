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
using UniversityAdmission.ViewModels;

namespace UniversityAdmission.Controllers
{
    [Authorize]
    public class RequiredExamController : Controller
    {
        private readonly RequiredExamRepository _requiredExamRepository;
        private readonly SpecialityRepository _specialityRepository;

        public RequiredExamController(RequiredExamRepository requiredExamRepository, SpecialityRepository specialityRepository)
        {
            _requiredExamRepository = requiredExamRepository;
            _specialityRepository = specialityRepository;
        }

        [Route("Speciality/{specialityId}/RequiredExam")]
        public async Task<IActionResult> Index(ObjectId specialityId)
        {
            if (specialityId == ObjectId.Empty)
            {
                return NotFound();
            }
            
            var selectedSpeciality = await _specialityRepository.GetById(specialityId);
            if (selectedSpeciality == null)
            {
                return NotFound();
            }

            var requiredExams = await _requiredExamRepository.GetRequiredExamsFromSpeciality(specialityId);

            var model = new SpecialityAndRequiredExamsViewModel
            {
                Speciality = selectedSpeciality,
                RequiredExams = requiredExams
            };

            return View(model);
        }

        [HttpGet]
        [Authorize(Policy = "Operator")]
        [Route("Speciality/{specialityId}/RequiredExam/Add")]
        public IActionResult Add(ObjectId specialityId)
        {
            var requiredExam = new RequiredExamDTO { SpecialityId = specialityId };
            return View(requiredExam);
        }

        [HttpPost]
        [Authorize(Policy = "Operator")]
        [Route("Speciality/{specialityId}/RequiredExam/Add")]
        public async Task<IActionResult> Add(ObjectId specialityId, RequiredExamDTO dto)
        {
            await _requiredExamRepository.Create(dto);
            return RedirectToAction("Index", new { specialityId });
        }

        [HttpPost]
        [Authorize(Policy = "Operator")]
        [Route("Speciality/{specialityId}/RequiredExam/Delete/{requiredExamId}")]
        public async Task<IActionResult> Delete(ObjectId specialityId, ObjectId requiredExamId)
        {
            await _requiredExamRepository.DeleteByIdAsync(requiredExamId);
            return RedirectToAction("Index", new { specialityId });
        }
    }
}