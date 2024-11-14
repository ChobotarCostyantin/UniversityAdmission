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
    public class GroupExamController : Controller
    {
        private readonly GroupExamRepository _groupExamRepository;
        private readonly GroupRepository _groupRepository;

        public GroupExamController(GroupExamRepository groupExamRepository, GroupRepository groupRepository)
        {
            _groupExamRepository = groupExamRepository;
            _groupRepository = groupRepository;
        }

        [Route("Group/{groupId}/GroupExam")]
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

            var groupExams = await _groupExamRepository.GetGroupExamsFromGroup(groupId);

            var model = new GroupAndGroupExamsViewModel
            {
                Group = selectedGroup,
                GroupExams = groupExams
            };

            return View(model);
        }

        [HttpGet]
        [Route("Group/{groupId}/GroupExam/Add")]
        public IActionResult Add(ObjectId groupId)
        {
            var groupExam = new GroupExamDTO { GroupId = groupId };
            return View(groupExam);
        }

        [HttpPost]
        [Route("Group/{groupId}/GroupExam/Add")]
        public async Task<IActionResult> Add(ObjectId groupId, GroupExamDTO dto)
        {
            await _groupExamRepository.Create(dto);
            return RedirectToAction("Index", new { groupId }); 
        }

        [HttpPost]
        [Route("Group/{groupId}/GroupExam/Delete/{groupExamId}")]
        public async Task<IActionResult> Delete(ObjectId groupId, ObjectId groupExamId)
        {
            await _groupExamRepository.DeleteByIdAsync(groupExamId);
            return RedirectToAction("Index", new { groupId });
        }
    }
}