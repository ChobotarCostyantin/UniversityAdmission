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
    public class TeacherRepository
    {
        private readonly UniversityAdmissionDBContext _context;
        private readonly TeacherExamRepository _teacherExamRepository;
        private readonly GroupTeacherRepository _groupTeacherRepository;

        public TeacherRepository(UniversityAdmissionDBContext context, TeacherExamRepository teacherExamRepository,
            GroupTeacherRepository groupTeacherRepository)
        {
            _context = context;
            _teacherExamRepository = teacherExamRepository;
            _groupTeacherRepository = groupTeacherRepository;
        }

        public async Task Create(TeacherDTO dto)
        {
            Teacher teacher = new()
            {
                Id = dto.Id,
                FullName = dto.FullName,
                DateOfBirth = dto.DateOfBirth.AddHours(12),
                PhoneNumber = dto.PhoneNumber,
                Subject = dto.Subject
            };
            _context.Teachers.Add(teacher);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteByIdAsync(ObjectId id)
        {
            var groupTeachers = _context.GroupTeachers.Where(x => x.TeacherId == id);
            foreach (var groupTeacher in groupTeachers)
            {
                await _groupTeacherRepository.DeleteByIdAsync(groupTeacher.Id);
            }

            var teacherExams = _context.TeacherExams.Where(x => x.TeacherId == id);
            foreach (var teacherExam in teacherExams)
            {
                await _teacherExamRepository.DeleteByIdAsync(teacherExam.Id);
            }

            var teacher = await _context.Teachers.FindAsync(id);
            if (teacher != null)
            {
                _context.Teachers.Remove(teacher);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Teacher>> GetAll()
        {
            return await _context.Teachers.ToListAsync();
        }

        public async Task<Teacher?> GetById(ObjectId id)
        {
            return await _context.Teachers.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task Update(TeacherDTO dto)
        {
            var teacher = await _context.Teachers.FindAsync(dto.Id);
            if (teacher != null)
            {
                teacher.FullName = dto.FullName;
                teacher.DateOfBirth = dto.DateOfBirth.AddHours(12);
                teacher.PhoneNumber = dto.PhoneNumber;
                teacher.Subject = dto.Subject;

                _context.Teachers.Update(teacher);
                await _context.SaveChangesAsync();
            }
        }
    }
}