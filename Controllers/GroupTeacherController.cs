using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
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
    public class GroupTeacherController : Controller
    {
        private readonly GroupTeacherRepository _groupTeacherRepository;
        private readonly GroupRepository _groupRepository;

        public GroupTeacherController(GroupTeacherRepository groupTeacherRepository, GroupRepository groupRepository)
        {
            _groupTeacherRepository = groupTeacherRepository;
            _groupRepository = groupRepository;
        }

        [Route("Group/{groupId}/GroupTeacher")]
        public async Task<IActionResult> Index(ObjectId groupId)
        {
            if (groupId == ObjectId.Empty)
            {
                return NotFound();
            }

            var selectedGroup = await _groupRepository.GetById(groupId);
            if (selectedGroup == null)
            {
                return NotFound();
            }

            var groupTeachers = await _groupTeacherRepository.GetGroupTeachersFromGroup(groupId);

            var model = new GroupAndGroupTeachersViewModel
            {
                Group = selectedGroup,
                GroupTeachers = groupTeachers
            };
            
            return View(model);
        }

        [HttpGet]
        [Authorize(Policy = "Operator")]
        [Route("Group/{groupId}/GroupTeacher/Add")]
        public IActionResult Add(ObjectId groupId)
        {
            var groupTeacher = new GroupTeacherDTO { GroupId = groupId };
            return View(groupTeacher);
        }

        [HttpPost]
        [Authorize(Policy = "Operator")]
        [Route("Group/{groupId}/GroupTeacher/Add")]
        public async Task<IActionResult> Add(ObjectId groupId, GroupTeacherDTO dto)
        {
            await _groupTeacherRepository.Create(dto);
            return RedirectToAction("Index", new { groupId });
        }

        [HttpPost]
        [Authorize(Policy = "Operator")]
        [Route("Group/{groupId}/GroupTeacher/Delete/{groupTeacherId}")] 
        public async Task<IActionResult> Delete(ObjectId groupId, ObjectId groupTeacherId)
        {
            await _groupTeacherRepository.DeleteByIdAsync(groupTeacherId);
            return RedirectToAction("Index", new { groupId });
        }
    }
}