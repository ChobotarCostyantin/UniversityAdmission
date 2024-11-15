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

namespace UniversityAdmission.Controllers
{
    [Authorize]
    public class TeacherController : Controller
    {
        private readonly TeacherRepository _teacherRepository;

        public TeacherController(TeacherRepository teacherRepository)
        {
            _teacherRepository = teacherRepository;
        }

        public async Task<IActionResult> Index(string searchString, string sortOrder)
        {
            ViewData["NameSortParam"] = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DateOfBirthSortParam"] = sortOrder == "dateOfBirth" ? "dateOfBirth_desc" : "dateOfBirth";
            ViewData["CurrentFilter"] = searchString;

            var teachers = await _teacherRepository.GetAll();

            if (!string.IsNullOrEmpty(searchString))
            {
                teachers = teachers.Where(a =>
                    a.FullName.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            teachers = sortOrder switch
            {
                "name_desc" => teachers.OrderByDescending(a => a.FullName).ToList(),
                "dateOfBirth" => teachers.OrderBy(a => a.DateOfBirth).ToList(),
                "dateOfBirth_desc" => teachers.OrderByDescending(a => a.DateOfBirth).ToList(),
                _ => teachers.OrderBy(a => a.FullName).ToList()
            };

            return View(teachers);
        }

        [HttpGet]
        [Authorize(Policy = "Operator")]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Policy = "Operator")]
        public async Task<IActionResult> Add(TeacherDTO dto)
        {
            await _teacherRepository.Create(dto);
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

            var selectedTeacher = await _teacherRepository.GetById(id);
            if (selectedTeacher == null)
            {
                return NotFound();
            }

            var teacher = new TeacherDTO
            {
                Id = selectedTeacher.Id,
                FullName = selectedTeacher.FullName,
                DateOfBirth = selectedTeacher.DateOfBirth,
                PhoneNumber = selectedTeacher.PhoneNumber,
                Subject = selectedTeacher.Subject
            };
            return View(teacher);
        }

        [HttpPost]
        [Authorize(Policy = "Operator")]
        public async Task<IActionResult> Edit(TeacherDTO dto)
        {
            await _teacherRepository.Update(dto);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize(Policy = "Operator")]
        public async Task<IActionResult> Delete(ObjectId id)
        {
            await _teacherRepository.DeleteByIdAsync(id);
            return RedirectToAction("Index");
        }
    }
}