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
    public class DepartmentController : Controller
    {
        private readonly FacultyRepository _facultyRepository;
        private readonly DepartmentRepository _departmentRepository;

        public DepartmentController(FacultyRepository facultyRepository, DepartmentRepository departmentRepository)
        {
            _facultyRepository = facultyRepository;
            _departmentRepository = departmentRepository;
        }

        public async Task<IActionResult> Index(string searchString, string sortOrder, ObjectId? facultyFilter)
        {
            ViewData["NameSortParam"] = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DescriptionSortParam"] = sortOrder == "description" ? "description_desc" : "description";
            ViewData["FacultySortParam"] = sortOrder == "faculty" ? "faculty_desc" : "faculty";
            ViewData["CurrentFilter"] = searchString;
            ViewData["CurrentFacultyFilter"] = facultyFilter;

            var departments = await _departmentRepository.GetAll();
            var faculties = await _facultyRepository.GetAll();
            var facultyDict = faculties.ToDictionary(f => f.Id, f => f);
            ViewBag.Faculties = facultyDict;
            ViewBag.FacultiesList = faculties; // Для випадаючого списка

            // Виконуємо фільтрацію по факультету
            if (facultyFilter.HasValue && facultyFilter.Value != ObjectId.Empty)
            {
                departments = departments.Where(d => d.FacultyId == facultyFilter.Value).ToList();
            }

            if (!string.IsNullOrEmpty(searchString))
            {
                departments = departments.Where(d =>
                    d.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase) ||
                    d.Description.Contains(searchString, StringComparison.OrdinalIgnoreCase) ||
                    facultyDict[d.FacultyId].Name.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            departments = sortOrder switch
            {
                "name_desc" => departments.OrderByDescending(d => d.Name).ToList(),
                "description" => departments.OrderBy(d => d.Description).ToList(),
                "description_desc" => departments.OrderByDescending(d => d.Description).ToList(),
                "faculty" => departments.OrderBy(d => facultyDict[d.FacultyId].Name).ToList(),
                "faculty_desc" => departments.OrderByDescending(d => facultyDict[d.FacultyId].Name).ToList(),
                _ => departments.OrderBy(d => d.Name).ToList()
            };

            return View(departments);
        }

        [HttpGet]
        [Authorize(Policy = "Operator")]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Policy = "Operator")]
        public async Task<IActionResult> Add(DepartmentDTO dto)
        {
            await _departmentRepository.Create(dto);
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
                FacultyId = selectedDepartment.FacultyId
            };
            return View(department);
        }

        [HttpPost]
        [Authorize(Policy = "Operator")]
        public async Task<IActionResult> Edit(DepartmentDTO dto)
        {
            await _departmentRepository.Update(dto);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize(Policy = "Operator")]
        public async Task<IActionResult> Delete(ObjectId id)
        {
            await _departmentRepository.DeleteByIdAsync(id);
            return RedirectToAction("Index");
        }
    }
}