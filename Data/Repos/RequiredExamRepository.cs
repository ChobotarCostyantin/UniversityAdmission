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
    public class RequiredExamRepository
    {
        private readonly UniversityAdmissionDBContext _context;

        public RequiredExamRepository(UniversityAdmissionDBContext context)
        {
            _context = context;
        }

        public async Task<List<RequiredExam>> GetAll()
        {
            var requiredExams = await _context.RequiredExams.ToListAsync();
            foreach (var requiredExam in requiredExams)
            {
                requiredExam.Exam = await _context.Exams.FirstOrDefaultAsync(x => x.Id == requiredExam.ExamId);
            }
            return requiredExams;
        }

        public async Task<List<RequiredExam>> GetRequiredExamsFromSpeciality(ObjectId specialityId)
        {
            var requiredExams = await _context.RequiredExams.Where(x => x.SpecialityId == specialityId).ToListAsync();
            foreach (var requiredExam in requiredExams)
            {
                requiredExam.Exam = await _context.Exams.FirstOrDefaultAsync(x => x.Id == requiredExam.ExamId);
            }
            return requiredExams;
        }

        public async Task Create(RequiredExamDTO dto)
        {
            RequiredExam requiredExam = new()
            {
                Id = dto.Id,
                SpecialityId = dto.SpecialityId,
                ExamId = dto.ExamId
            };
            _context.RequiredExams.Add(requiredExam);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteByIdAsync(ObjectId id)
        {
            var requiredExam = await _context.RequiredExams.FindAsync(id);
            if (requiredExam != null)
            {
                _context.RequiredExams.Remove(requiredExam);
                await _context.SaveChangesAsync();
            }
        }
    }
}