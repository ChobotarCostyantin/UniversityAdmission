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

namespace UniversityAdmission.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly FacultyRepository _facultyRepository;
        private readonly DepartmentRepository _departmentRepository;

        public DepartmentController(FacultyRepository facultyRepository, DepartmentRepository departmentRepository)
        {
            _facultyRepository = facultyRepository;
            _departmentRepository = departmentRepository;
        }

        public async Task<IActionResult> Index()
        {
            var departments = await _departmentRepository.GetAll();
            var faculties = await _facultyRepository.GetAll();

            // Словник для швидкого доступу до факультетів по Id
            var facultyDict = faculties.ToDictionary(f => f.Id, f => f);

            // Додаємо інформацію про факультет в ViewBag
            ViewBag.Faculties = facultyDict;

            return View(departments);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(DepartmentDTO dto)
        {
            await _departmentRepository.Create(dto);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(ObjectId id)
        {
            if (id == ObjectId.Empty)
            {
                return NotFound();
            }

            var selectedDepartment = await _departmentRepository.GetById(id);
            if (selectedDepartment == null)
            {
                return NotFound();
            }

            var department = new DepartmentDTO
            {
                Id = selectedDepartment.Id,
                Name = selectedDepartment.Name,
                Description = selectedDepartment.Description,
                Faculty = selectedDepartment.Faculty
            };
            return View(department);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(DepartmentDTO dto)
        {
            await _departmentRepository.Update(dto);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(ObjectId id)
        {
            await _departmentRepository.DeleteByIdAsync(id);
            return RedirectToAction("Index");
        }
    }
}