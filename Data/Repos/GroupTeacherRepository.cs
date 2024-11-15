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
    public class GroupTeacherRepository
    {
        private readonly UniversityAdmissionDBContext _context;

        public GroupTeacherRepository(UniversityAdmissionDBContext context)
        {
            _context = context;
        }

        public async Task<List<GroupTeacher>> GetAll()
        {
            var groupTeachers = await _context.GroupTeachers.ToListAsync();
            foreach (var groupTeacher in groupTeachers)
            {
                groupTeacher.Teacher = await _context.Teachers.FirstOrDefaultAsync(x => x.Id == groupTeacher.TeacherId);
            }
            return groupTeachers;
        }

        public async Task<List<GroupTeacher>> GetGroupTeachersFromGroup(ObjectId groupId)
        {
            var groupTeachers = await _context.GroupTeachers.Where(x => x.GroupId == groupId).ToListAsync();
            foreach (var groupTeacher in groupTeachers)
            {
                groupTeacher.Teacher = await _context.Teachers.FirstOrDefaultAsync(x => x.Id == groupTeacher.TeacherId);
            }
            return groupTeachers;
        }

        public async Task Create(GroupTeacherDTO dto)
        {
            GroupTeacher groupTeacher = new()
            {
                Id = dto.Id,
                GroupId = dto.GroupId,
                TeacherId = dto.TeacherId
            };
            _context.GroupTeachers.Add(groupTeacher);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteByIdAsync(ObjectId id)
        {
            var groupTeacher = await _context.GroupTeachers.FindAsync(id);
            if (groupTeacher != null)
            {
                _context.GroupTeachers.Remove(groupTeacher);
                await _context.SaveChangesAsync();
            }
        }
    }
}