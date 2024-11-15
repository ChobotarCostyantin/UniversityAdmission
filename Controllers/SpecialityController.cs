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
    public class SpecialityController : Controller
    {
        private readonly RequiredExamRepository _requiredExamRepository;
        private readonly SpecialityRepository _specialityRepository;
        private readonly DepartmentRepository _departmentRepository;

        public SpecialityController(SpecialityRepository specialityRepository, DepartmentRepository departmentRepository, RequiredExamRepository requiredExamRepository)
        {
            _requiredExamRepository = requiredExamRepository;
            _specialityRepository = specialityRepository;
            _departmentRepository = departmentRepository;
        }

        public async Task<IActionResult> Index(string searchString, string sortOrder, ObjectId? departmentFilter)
        {
            ViewData["NameSortParam"] = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DescriptionSortParam"] = sortOrder == "description" ? "description_desc" : "description";
            ViewData["DepartmentSortParam"] = sortOrder == "department" ? "department_desc" : "department";
            ViewData["CurrentFilter"] = searchString;
            ViewData["CurrentDepartmentFilter"] = departmentFilter;

            var specialities = await _specialityRepository.GetAll();
            var departments = await _departmentRepository.GetAll();
            var departmentDict = departments.ToDictionary(d => d.Id, d => d);
            ViewBag.Departments = departmentDict;
            ViewBag.DepartmentsList = departments;

            if (departmentFilter.HasValue && departmentFilter.Value != ObjectId.Empty)
            {
                specialities = specialities.Where(s => s.DepartmentId == departmentFilter.Value).ToList();
            }

            if (!string.IsNullOrEmpty(searchString))
            {
                specialities = specialities.Where(s =>
                    s.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase) ||
                    s.Description.Contains(searchString, StringComparison.OrdinalIgnoreCase) ||
                    departmentDict[s.DepartmentId].Name.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            specialities = sortOrder switch
            {
                "name_desc" => specialities.OrderByDescending(s => s.Name).ToList(),
                "description" => specialities.OrderBy(s => s.Description).ToList(),
                "description_desc" => specialities.OrderByDescending(s => s.Description).ToList(),
                "department" => specialities.OrderBy(s => departmentDict[s.DepartmentId].Name).ToList(),
                "department_desc" => specialities.OrderByDescending(s => departmentDict[s.DepartmentId].Name).ToList(),
                _ => specialities.OrderBy(s => s.Name).ToList()
            };

            return View(specialities);
        }

        [HttpGet]
        [Authorize(Policy = "Operator")]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Policy = "Operator")]
        public async Task<IActionResult> Add(SpecialityDTO dto)
        {
            await _specialityRepository.Create(dto);
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

            var selectedSpeciality = await _specialityRepository.GetById(id);
            if (selectedSpeciality == null)
            {
                return NotFound();
            }

            var speciality = new SpecialityDTO
            {
                Id = selectedSpeciality.Id,
                Name = selectedSpeciality.Name,
                Description = selectedSpeciality.Description,
                DepartmentId = selectedSpeciality.DepartmentId
            };
            return View(speciality);
        }

        [HttpPost]
        [Authorize(Policy = "Operator")]
        public async Task<IActionResult> Edit(SpecialityDTO dto)
        {
            await _specialityRepository.Update(dto);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize(Policy = "Operator")]
        public async Task<IActionResult> Delete(ObjectId id)
        {
            await _specialityRepository.DeleteByIdAsync(id);
            return RedirectToAction("Index");
        }
    }
}