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
    public class FacultyController : Controller
    {
        private readonly FacultyRepository _facultyrepository;

        public FacultyController(FacultyRepository facultyService)
        {
            _facultyrepository = facultyService;
        }

        public async Task<IActionResult> Index(string searchString, string sortOrder)
    {
        ViewData["NameSortParam"] = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
        ViewData["DescriptionSortParam"] = sortOrder == "description" ? "description_desc" : "description";
        ViewData["CurrentFilter"] = searchString;

        var faculties = await _facultyrepository.GetAll();

        if (!string.IsNullOrEmpty(searchString))
        {
            faculties = faculties.Where(f => 
                f.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase) || 
                f.Description.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        faculties = sortOrder switch
        {
            "name_desc" => faculties.OrderByDescending(f => f.Name).ToList(),
            "description" => faculties.OrderBy(f => f.Description).ToList(),
            "description_desc" => faculties.OrderByDescending(f => f.Description).ToList(),
            _ => faculties.OrderBy(f => f.Name).ToList()
        };

        return View(faculties);
    }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(FacultyDTO dto)
        {
            await _facultyrepository.Create(dto);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(ObjectId id)
        {
            if (id == ObjectId.Empty)
            {
                return NotFound();
            }

            var selectedFaculty = await _facultyrepository.GetById(id);
            if (selectedFaculty == null)
            {
                return NotFound();
            }

            var facultyDTO = new FacultyDTO
            {
                Id = selectedFaculty.Id,
                Name = selectedFaculty.Name,
                Description = selectedFaculty.Description
            };

            return View(facultyDTO);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(FacultyDTO dto)
        {
            await _facultyrepository.Update(dto);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(ObjectId id)
        {
            await _facultyrepository.DeleteByIdAsync(id);
            return RedirectToAction("Index");
        }
    }
}