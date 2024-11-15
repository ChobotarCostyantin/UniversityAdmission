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
    public class ExamResultRepository
    {
        private readonly UniversityAdmissionDBContext _context;

        public ExamResultRepository(UniversityAdmissionDBContext context)
        {
            _context = context;          
        }

        public async Task<List<ExamResult>> GetAll()
        {
            var examResults = await _context.ExamResults.ToListAsync();
            foreach (var examResult in examResults)
            {
                examResult.Exam = await _context.Exams.FirstOrDefaultAsync(x => x.Id == examResult.ExamId);
            }
            return examResults;
        }

        public async Task<ExamResult?> GetById(ObjectId id)
        {
            var examResult = await _context.ExamResults.FirstOrDefaultAsync(x => x.Id == id);
            if (examResult != null)
            {
                examResult.Exam = await _context.Exams.FirstOrDefaultAsync(x => x.Id == examResult.ExamId);
            }
            return examResult;
        }

        public async Task<List<ExamResult>> GetExamResultsFromApplicant(ObjectId applicantId)
        {
            var examResults = await _context.ExamResults.Where(x => x.ApplicantId == applicantId).ToListAsync();
            foreach (var examResult in examResults)
            {
                examResult.Exam = await _context.Exams.FirstOrDefaultAsync(x => x.Id == examResult.ExamId);
            }
            return examResults;
        }

        public async Task Create(ExamResultDTO dto)
        {
            var exam = await _context.Exams.FirstOrDefaultAsync(x => x.Id == dto.ExamId) ?? throw new Exception("Екзамен не знайдено");
            ExamResult examResult = new()
            {
                Id = dto.Id,
                ApplicantId = dto.ApplicantId,
                ExamId = dto.ExamId,
                Score = dto.Score,
                Status = dto.Score >= exam.MinScore
            };
            _context.ExamResults.Add(examResult);
            await _context.SaveChangesAsync();
        }

        public async Task Update(ExamResultDTO dto, ObjectId examResultId)
        {
            var exam = await _context.Exams.FirstOrDefaultAsync(x => x.Id == dto.ExamId) ?? throw new Exception("Екзамен не знайдено");
            var examResult = await _context.ExamResults.FirstOrDefaultAsync(x => x.Id == examResultId);
            if (examResult != null)
            {
                examResult.Score = dto.Score;
                examResult.Status = dto.Score >= exam.MinScore;

                _context.ExamResults.Update(examResult);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteByIdAsync(ObjectId id)
        {
            var examResult = await _context.ExamResults.FindAsync(id);
            if (examResult != null)
            {
                _context.ExamResults.Remove(examResult);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Dictionary<ObjectId, double>> GetAverageScoresBySpeciality(ObjectId specialityId)
        {
            var applicants = await _context.Applicants
                .Where(a => a.SpecialityId == specialityId)
                .ToListAsync();

            var examResults = await _context.ExamResults
                .Where(er => applicants.Select(a => a.Id).Contains(er.ApplicantId))
                .ToListAsync();

            var averageScores = examResults
                .GroupBy(er => er.ExamId)
                .ToDictionary(
                    group => group.Key,
                    group => group.Average(er => er.Score)
                );

            return averageScores;
        }
    }
}