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
    public class DepartmentRepository
    {
        private readonly UniversityAdmissionDBContext _context;
        public DepartmentRepository(UniversityAdmissionDBContext context)
        {
            _context = context;
        }

        public async Task Create(DepartmentDTO dto)
        {
            Department department = new()
            {
                Id = dto.Id,
                Name = dto.Name,
                Description = dto.Description,
                FacultyId = dto.FacultyId
            };
            _context.Departments.Add(department);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteByIdAsync(ObjectId id)
        {
            var specialities = _context.Specialities.Where(x => x.DepartmentId == id);
            _context.Specialities.RemoveRange(specialities);
            await _context.SaveChangesAsync();

            var department = await _context.Departments.FindAsync(id);
            if (department != null)
            {
                _context.Departments.Remove(department);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Department>> GetAll()
        {
            return await _context.Departments.ToListAsync();
        }

        public async Task<Department?> GetById(ObjectId id)
        {
            return await _context.Departments.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task Update(DepartmentDTO dto)
        {
            var department = await _context.Departments.FirstOrDefaultAsync(x => x.Id == dto.Id);
            if (department != null)
            {
                department.Name = dto.Name;
                department.Description = dto.Description;
                department.FacultyId = dto.FacultyId;

                _context.Departments.Update(department);
                await _context.SaveChangesAsync();
            }

            var specialities = _context.Specialities.Where(x => x.DepartmentId == dto.Id);
            foreach (var speciality in specialities)
            {
                speciality.DepartmentId = dto.Id;
                _context.Specialities.Update(speciality);
            }
            await _context.SaveChangesAsync();
        }
    }
}