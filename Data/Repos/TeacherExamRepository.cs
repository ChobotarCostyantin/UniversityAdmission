using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using UniversityAdmission.Models.DTO;
using UniversityAdmission.Models.Entities;
using UniversityAdmission.Services;

namespace UniversityAdmission.Data.Repos
{
    public class TeacherExamRepository
    {
        private readonly UniversityAdmissionDBContext _context;
        
        public TeacherExamRepository(UniversityAdmissionDBContext context)
        {
            _context = context;
        }

        public async Task<List<TeacherExam>> GetAll()
        {
            var teacherExams = await _context.TeacherExams.ToListAsync();
            foreach (var teacherExam in teacherExams)
            {
                teacherExam.Exam = await _context.Exams.FirstOrDefaultAsync(x => x.Id == teacherExam.ExamId);
            }
            return teacherExams;
        }

        public async Task<List<TeacherExam>> GetTeacherExamsFromTeacher(ObjectId teacherId)
        {
            var teacherExams = await _context.TeacherExams.Where(x => x.TeacherId == teacherId).ToListAsync();
            foreach (var teacherExam in teacherExams)
            {
                teacherExam.Exam = await _context.Exams.FirstOrDefaultAsync(x => x.Id == teacherExam.ExamId);
            }
            return teacherExams;
        }

        public async Task Create(TeacherExamDTO dto)
        {
            TeacherExam teacherExam = new()
            {
                Id = dto.Id,
                TeacherId = dto.TeacherId,
                ExamId = dto.ExamId
            };
            _context.TeacherExams.Add(teacherExam);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteByIdAsync(ObjectId id)
        {
            var teacherExam = await _context.TeacherExams.FindAsync(id);
            if (teacherExam != null)
            {
                _context.TeacherExams.Remove(teacherExam);
                await _context.SaveChangesAsync();
            }
        }
    }
}