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
    public class ExamRepository
    {
        private readonly UniversityAdmissionDBContext _context;
        private readonly RequiredExamRepository _requiredExamRepository;

        public ExamRepository(UniversityAdmissionDBContext context, RequiredExamRepository requiredExamRepository)
        {
            _context = context;
            _requiredExamRepository = requiredExamRepository;
        }

        public async Task Create(ExamDTO dto)
        {
            Exam exam = new()
            {
                Id = dto.Id,
                Name = dto.Name,
                Subject = dto.Subject,
                MinScore = dto.MinScore,
                IsCreativeContest = dto.IsCreativeContest,
            };
            _context.Exams.Add(exam);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteByIdAsync(ObjectId id)
        {
            var requiredExams = _context.RequiredExams.Where(x => x.ExamId == id);
            foreach (var requiredExam in requiredExams)
            {
                await _requiredExamRepository.DeleteByIdAsync(requiredExam.Id);
            }

            var exam = await _context.Exams.FindAsync(id);
            if (exam != null)
            {
                _context.Exams.Remove(exam);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Exam>> GetAll()
        {
            return await _context.Exams.ToListAsync();
        }

        public async Task<Exam?> GetById(ObjectId id)
        {
            return await _context.Exams.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task Update(ExamDTO dto)
        {
            var exam = await _context.Exams.FirstOrDefaultAsync(x => x.Id == dto.Id);
            if (exam != null)
            {
                exam.Name = dto.Name;
                exam.Subject = dto.Subject;
                exam.MinScore = dto.MinScore;
                exam.IsCreativeContest = dto.IsCreativeContest;

                _context.Exams.Update(exam);
                await _context.SaveChangesAsync();
            }
        }
    }
}