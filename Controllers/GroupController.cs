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
    public class GroupController : Controller
    {
        public readonly GroupRepository _groupRepository;

        public GroupController(GroupRepository groupRepository)
        {
            _groupRepository = groupRepository;
        }

        public async Task<IActionResult> Index(string searchString, string sortOrder)
        {
            ViewData["NameSortParam"] = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DateSortParam"] = sortOrder == "date" ? "date_desc" : "date";
            ViewData["CurrentFilter"] = searchString;

            var groups = await _groupRepository.GetAll();

            if (!string.IsNullOrEmpty(searchString))
            {
                groups = groups.Where(s => s.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList();
            };

            groups = sortOrder switch
            {
                "name_desc" => groups.OrderByDescending(s => s.Name).ToList(),
                "date" => groups.OrderBy(s => s.ExamDate).ToList(),
                "date_desc" => groups.OrderByDescending(s => s.ExamDate).ToList(),
                _ => groups.OrderBy(s => s.Name).ToList()
            };

            return View(groups);
        }

        [HttpGet]
        [Authorize(Policy = "Operator")]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Policy = "Operator")]
        public async Task<IActionResult> Add(GroupDTO dto)
        {
            await _groupRepository.Create(dto);
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

            var selectedGroup = await _groupRepository.GetById(id);
            if (selectedGroup == null)
            {
                return NotFound();
            }

            var group = new GroupDTO
            {
                Id = selectedGroup.Id,
                Name = selectedGroup.Name,
                ExamDate = selectedGroup.ExamDate
            };

            return View(group);
        }

        [HttpPost]
        [Authorize(Policy = "Operator")]
        public async Task<IActionResult> Edit(GroupDTO dto)
        {
            await _groupRepository.Update(dto);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize(Policy = "Operator")]
        public async Task<IActionResult> Delete(ObjectId id)
        {
            await _groupRepository.DeleteByIdAsync(id);
            return RedirectToAction("Index");
        }
    }
}