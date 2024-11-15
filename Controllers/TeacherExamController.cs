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
    public class TeacherExamController : Controller
    {
        private readonly TeacherExamRepository _teacherExamRepository;
        private readonly TeacherRepository _teacherRepository;
        public TeacherExamController(TeacherExamRepository teacherExamRepository, TeacherRepository teacherRepository)
        {
            _teacherExamRepository = teacherExamRepository;
            _teacherRepository = teacherRepository;
        }

        [Route("Teacher/{teacherId}/TeacherExam")]
        public async Task<IActionResult> Index(ObjectId teacherId)
        {
            if (teacherId == ObjectId.Empty)
            {
                return NotFound();
            }

            var selectedTeacher = await _teacherRepository.GetById(teacherId);
            if (selectedTeacher == null)
            {
                return NotFound();
            }

            var teacherExams = await _teacherExamRepository.GetTeacherExamsFromTeacher(teacherId);

            var model = new TeacherAndTeacherExamsViewModel
            {
                Teacher = selectedTeacher,
                TeacherExams = teacherExams
            };

            return View(model);
        }

        [HttpGet]
        [Authorize(Policy = "Operator")]
        [Route("Teacher/{teacherId}/TeacherExam/Add")]
        public IActionResult Add(ObjectId teacherId)
        {
            var teacherExam = new TeacherExamDTO { TeacherId = teacherId };
            return View(teacherExam);
        }

        [HttpPost]
        [Authorize(Policy = "Operator")]
        [Route("Teacher/{teacherId}/TeacherExam/Add")]
        public async Task<IActionResult> Add(ObjectId teacherId, TeacherExamDTO dto)
        {
            await _teacherExamRepository.Create(dto);
            return RedirectToAction("Index", new { teacherId });
        }

        [HttpPost]
        [Authorize(Policy = "Operator")]
        [Route("Teacher/{teacherId}/TeacherExam/Delete/{teacherExamId}")]
        public async Task<IActionResult> Delete(ObjectId teacherId, ObjectId teacherExamId)
        {
            await _teacherExamRepository.DeleteByIdAsync(teacherExamId);
            return RedirectToAction("Index", new { teacherId });
        }
    }
}