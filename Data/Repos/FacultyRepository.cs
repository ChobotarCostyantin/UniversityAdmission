using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using UniversityAdmission.Models.DTO;
using UniversityAdmission.Models.Entities;
using UniversityAdmission.Services;
using ZstdSharp.Unsafe;

namespace UniversityAdmission.Data.Repos
{
    public class FacultyRepository
    {
        private readonly UniversityAdmissionDBContext _context;
        private readonly DepartmentRepository _departmentRepository;
        public FacultyRepository(UniversityAdmissionDBContext context, DepartmentRepository departmentRepository)
        {
            _context = context;
            _departmentRepository = departmentRepository;
        }
        public async Task Create(FacultyDTO dto)
        {
            Faculty faculty = new()
            {
                Id = dto.Id,
                Name = dto.Name,
                Description = dto.Description
            };
            _context.Faculties.Add(faculty);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteByIdAsync(ObjectId id)
        {
            var departments = _context.Departments.Where(x => x.FacultyId == id);
            foreach (var department in departments)
            {
                await _departmentRepository.DeleteByIdAsync(department.Id);
            }

            var faculty = await _context.Faculties.FindAsync(id);
            if (faculty != null)
            {
                _context.Faculties.Remove(faculty!);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Faculty>> GetAll()
        {
            return await _context.Faculties.ToListAsync();
        }

        public async Task<Faculty?> GetById(ObjectId id)
        {
            return await _context.Faculties.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task Update(FacultyDTO dto)
        {
            var faculty = await _context.Faculties.FirstOrDefaultAsync(x => x.Id == dto.Id);
            if (faculty != null)
            {
                faculty.Name = dto.Name;
                faculty.Description = dto.Description;

                _context.Faculties.Update(faculty);
                await _context.SaveChangesAsync();
            }
        }
    }
}