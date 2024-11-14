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

        public GroupRepository(UniversityAdmissionDBContext context)
        {
            _context = context;
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
            // TODO: groupApplicants, groupExams, groupTeachers deletion
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