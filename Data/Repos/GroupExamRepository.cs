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
    public class GroupExamRepository
    {
        private readonly UniversityAdmissionDBContext _context;

        public GroupExamRepository(UniversityAdmissionDBContext context)
        {
            _context = context;
        }

        public async Task<List<GroupExam>> GetAll()
        {
            var groupExams = await _context.GroupExams.ToListAsync();
            foreach (var groupExam in groupExams)
            {
                groupExam.Exam = await _context.Exams.FirstOrDefaultAsync(x => x.Id == groupExam.ExamId);
            }
            return groupExams;
        }

        public async Task<List<GroupExam>> GetGroupExamsFromGroup(ObjectId groupId)
        {
            var groupExams = await _context.GroupExams.Where(x => x.GroupId == groupId).ToListAsync();
            foreach (var groupExam in groupExams)
            {
                groupExam.Exam = await _context.Exams.FirstOrDefaultAsync(x => x.Id == groupExam.ExamId);
            }
            return groupExams;
        }

        public async Task Create(GroupExamDTO dto)
        {
            GroupExam groupExam = new()
            {
                Id = dto.Id,
                GroupId = dto.GroupId,
                ExamId = dto.ExamId
            };
            _context.GroupExams.Add(groupExam);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteByIdAsync(ObjectId id)
        {
            var groupExam = await _context.GroupExams.FindAsync(id);
            if (groupExam != null)
            {
                _context.GroupExams.Remove(groupExam);
                await _context.SaveChangesAsync();
            }
        }
    }
}