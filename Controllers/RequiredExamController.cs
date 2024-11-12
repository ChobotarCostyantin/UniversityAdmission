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
    public class RequiredExamController : Controller
    {
        private readonly RequiredExamRepository _requiredExamRepository;
        private readonly SpecialityRepository _specialityRepository;

        public RequiredExamController(RequiredExamRepository requiredExamRepository, SpecialityRepository specialityRepository)
        {
            _requiredExamRepository = requiredExamRepository;
            _specialityRepository = specialityRepository;
        }

        [Route("Speciality/{id}/RequiredExam")]
        public async Task<IActionResult> Index(ObjectId id)
        {
            if (id == ObjectId.Empty)
            {
                return NotFound();
            }
            
            var selectedSpeciality = await _specialityRepository.GetById(id);
            if (selectedSpeciality == null)
            {
                return NotFound();
            }

            var requiredExams = await _requiredExamRepository.GetRequiredExamsFromSpeciality(id);

            var model = new SpecialityAndRequiredExamsViewModel
            {
                Speciality = selectedSpeciality,
                RequiredExams = requiredExams
            };

            return View(model);
        }

        [HttpGet]
        [Route("Speciality/{id}/RequiredExam/Add")]
        public IActionResult Add(ObjectId id)
        {
            var requiredExam = new RequiredExamDTO { SpecialityId = id };
            return View(requiredExam);
        }

        [HttpPost]
        [Route("Speciality/{id}/RequiredExam/Add")]
        public async Task<IActionResult> Add(ObjectId id, RequiredExamDTO dto)
        {
            await _requiredExamRepository.Create(dto);
            return RedirectToAction("Index", new { id = id });
        }

        [HttpPost]
        [Route("Speciality/{specialityId}/RequiredExam/Delete/{requiredExamId}")]
        public async Task<IActionResult> Delete(ObjectId specialityId, ObjectId requiredExamId)
        {
            await _requiredExamRepository.DeleteByIdAsync(requiredExamId);
            return RedirectToAction("Index", new { id = specialityId });
        }
    }
}