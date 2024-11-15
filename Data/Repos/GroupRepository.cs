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
    public class GroupRepository
    {
        private readonly UniversityAdmissionDBContext _context;
        private readonly GroupExamRepository _groupExamRepository;
        private readonly GroupTeacherRepository _groupTeacherRepository;
        private readonly GroupApplicantRepository _groupApplicantRepository;

        public GroupRepository(UniversityAdmissionDBContext context, GroupExamRepository groupExamRepository,
            GroupTeacherRepository groupTeacherRepository, GroupApplicantRepository groupApplicantRepository)
        {
            _context = context;
            _groupExamRepository = groupExamRepository;
            _groupTeacherRepository = groupTeacherRepository;
            _groupApplicantRepository = groupApplicantRepository;
        }

        public async Task Create(GroupDTO dto)
        {
            Group group = new()
            {
                Id = dto.Id,
                Name = dto.Name,
                ExamDate = dto.ExamDate.AddHours(12)
            };
            _context.Groups.Add(group);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteByIdAsync(ObjectId id)
        {
            var groupExams = _context.GroupExams.Where(x => x.GroupId == id);
            foreach (var groupExam in groupExams)
            {
                await _groupExamRepository.DeleteByIdAsync(groupExam.Id);
            }

            var groupTeachers = _context.GroupTeachers.Where(x => x.GroupId == id);
            foreach (var groupTeacher in groupTeachers)
            {
                await _groupTeacherRepository.DeleteByIdAsync(groupTeacher.Id);
            }
            
            var groupApplicants = _context.GroupApplicants.Where(x => x.GroupId == id);
            foreach (var groupApplicant in groupApplicants)
            {
                await _groupApplicantRepository.DeleteByIdAsync(groupApplicant.Id);
            }

            var group = await _context.Groups.FindAsync(id);
            if (group != null)
            {
                _context.Groups.Remove(group);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Group>> GetAll()
        {
            return await _context.Groups.ToListAsync();
        }

        public async Task<Group?> GetById(ObjectId id)
        {
            return await _context.Groups.FirstOrDefaultAsync(g => g.Id == id);
        }

        public async Task Update(GroupDTO dto)
        {
            var group = await _context.Groups.FirstOrDefaultAsync(g => g.Id == dto.Id);
            if (group != null)
            {
                group.Name = dto.Name;
                group.ExamDate = dto.ExamDate.AddHours(12);
                
                _context.Groups.Update(group);
                await _context.SaveChangesAsync();
            }
        }
    }
}