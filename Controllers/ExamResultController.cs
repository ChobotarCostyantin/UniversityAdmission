using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using UniversityAdmission.Data.Repos;
using UniversityAdmission.Models.DTO;
using UniversityAdmission.ViewModels;

namespace UniversityAdmission.Controllers
{
    [Authorize]
    public class ExamResultController : Controller
    {
        private readonly ExamResultRepository _examResultRepository;
        private readonly ApplicantRepository _applicantRepository;

        public ExamResultController(ExamResultRepository examResultRepository, ApplicantRepository applicantRepository)
        {
            _examResultRepository = examResultRepository;
            _applicantRepository = applicantRepository;
        }

        [Route("Applicant/{applicantId}/ExamResult")]
        public async Task<IActionResult> Index(ObjectId applicantId)
        {
            if (applicantId == ObjectId.Empty)
            {
                return NotFound();
            }

            var selectedApplicant = await _applicantRepository.GetById(applicantId);
            if (selectedApplicant == null)
            {
                return NotFound();
            }

            var examResults = await _examResultRepository.GetExamResultsFromApplicant(applicantId);

            var model = new ApplicantAndExamResultsViewModel
            {
                Applicant = selectedApplicant,
                ExamResults = examResults
            };
            
            return View(model);
        }

        [HttpGet]
        [Authorize(Policy = "Operator")]
        [Route("Applicant/{applicantId}/ExamResult/Add")]
        public IActionResult Add(ObjectId applicantId)
        {
            var examResult = new ExamResultDTO { ApplicantId = applicantId };
            return View(examResult);
        }

        [HttpPost]
        [Authorize(Policy = "Operator")]
        [Route("Applicant/{applicantId}/ExamResult/Add")]
        public async Task<IActionResult> Add(ObjectId applicantId, ExamResultDTO dto)
        {
            await _examResultRepository.Create(dto);
            return RedirectToAction("Index", new { applicantId });
        }

        [HttpGet]
        [Authorize(Policy = "Operator")]
        [Route("Applicant/{applicantId}/ExamResult/Edit/{examResultId}")] 
        public async Task<IActionResult> Edit(ObjectId examResultId, ObjectId applicantId)
        {
            if (examResultId == ObjectId.Empty)
            {
                return NotFound();
            }

            var selectedExamResult = await _examResultRepository.GetById(examResultId);
            if (selectedExamResult == null)
            {
                return NotFound();
            }

            var examResultDTO = new ExamResultDTO
            {
                Id = selectedExamResult.Id,
                ApplicantId = applicantId,
                ExamId = selectedExamResult.ExamId,
                Score = selectedExamResult.Score
            };
            
            return View(examResultDTO);
        }

        [HttpPost]
        [Authorize(Policy = "Operator")]
        [Route("Applicant/{applicantId}/ExamResult/Edit/{examResultId}")]
        public async Task<IActionResult> Edit(ObjectId applicantId, ObjectId examResultId, ExamResultDTO dto)
        {
            await _examResultRepository.Update(dto, examResultId);
            return RedirectToAction("Index", new { applicantId });
        }

        [HttpPost]
        [Authorize(Policy = "Operator")]
        [Route("Applicant/{applicantId}/ExamResult/Delete/{examResultId}")]
        public async Task<IActionResult> Delete(ObjectId applicantId, ObjectId examResultId)
        {
            await _examResultRepository.DeleteByIdAsync(examResultId);
            return RedirectToAction("Index", new { applicantId });
        }
    }
}