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
using UniversityAdmission.Models.Entities;
using UniversityAdmission.Services;

namespace UniversityAdmission.Controllers
{
    public class ExamController : Controller
    {
        private readonly ExamRepository _examRepository;
        private readonly RequiredExamRepository _requiredExamRepository;

        public ExamController(ExamRepository examRepository, RequiredExamRepository requiredExamRepository)
        {
            _examRepository = examRepository;
            _requiredExamRepository = requiredExamRepository;
        }

        public async Task<IActionResult> Index(string searchString, string sortOrder)
        {
            ViewData["NameSortParam"] = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["SubjectSortParam"] = sortOrder == "subject" ? "subject_desc" : "subject";
            ViewData["MinScoreSortParam"] = sortOrder == "minscore" ? "minscore_desc" : "minscore";
            ViewData["CurrentFilter"] = searchString;

            var exams = await _examRepository.GetAll();

            if (!string.IsNullOrEmpty(searchString))
            {
                exams = exams.Where(s =>
                    s.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase) ||
                    s.Subject.ToString().Contains(searchString, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            };

            exams = sortOrder switch
            {
                "name_desc" => exams.OrderByDescending(s => s.Name).ToList(),
                "subject" => exams.OrderBy(s => s.Subject).ToList(),
                "subject_desc" => exams.OrderByDescending(s => s.Subject).ToList(),
                "minscore" => exams.OrderBy(s => s.MinScore).ToList(),
                "minscore_desc" => exams.OrderByDescending(s => s.MinScore).ToList(),
                _ => exams.OrderBy(s => s.Name).ToList()
            };

            return View(exams);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(ExamDTO dto)
        {
            await _examRepository.Create(dto);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(ObjectId id)
        {
            if (id == ObjectId.Empty)
            {
                return NotFound();
            }

            var selectedExam = await _examRepository.GetById(id);
            if (selectedExam == null)
            {
                return NotFound();
            }

            var examDTO = new ExamDTO
            {
                Id = selectedExam.Id,
                Name = selectedExam.Name,
                Subject = selectedExam.Subject,
                MinScore = selectedExam.MinScore,
                IsCreativeContest = selectedExam.IsCreativeContest
            };

            return View(examDTO);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ExamDTO dto)
        {
            await _examRepository.Update(dto);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(ObjectId id)
        {
            await _examRepository.DeleteByIdAsync(id);
            return RedirectToAction("Index");
        }
    }
}