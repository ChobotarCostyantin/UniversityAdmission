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
    public class SpecialityRepository
    {
        private readonly UniversityAdmissionDBContext _context;
        private readonly RequiredExamRepository _requiredExamRepository;

        public SpecialityRepository(UniversityAdmissionDBContext context, RequiredExamRepository requiredExamRepository)
        {
            _context = context;
            _requiredExamRepository = requiredExamRepository;
        }

        public async Task Create(SpecialityDTO dto)
        {
            Speciality speciality = new()
            {
                Id = dto.Id,
                Name = dto.Name,
                Description = dto.Description,
                DepartmentId = dto.DepartmentId
            };
            _context.Specialities.Add(speciality);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteByIdAsync(ObjectId id)
        {
            // var applicants = _context.Applicants.Where(x => x.SpecialityId == id);
            // _context.Applicants.RemoveRange(applicants);
            // await _context.SaveChangesAsync();

            var requiredExams = await _requiredExamRepository.GetRequiredExamsFromSpeciality(id);
            foreach (var requiredExam in requiredExams)
            {
                await _requiredExamRepository.DeleteByIdAsync(requiredExam.Id);
            }

            var speciality = await _context.Specialities.FindAsync(id);
            if (speciality != null)
            {
                _context.Specialities.Remove(speciality);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Speciality>> GetAll()
        {
            return await _context.Specialities.ToListAsync();
        }

        public async Task<Speciality?> GetById(ObjectId id)
        {
            return await _context.Specialities.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task Update(SpecialityDTO dto)
        {
            var speciality = await _context.Specialities.FirstOrDefaultAsync(x => x.Id == dto.Id);
            if (speciality != null)
            {
                speciality.Name = dto.Name;
                speciality.Description = dto.Description;
                speciality.DepartmentId = dto.DepartmentId;

                _context.Specialities.Update(speciality);
                await _context.SaveChangesAsync();
            }

            var applicants = _context.Applicants.Where(x => x.SpecialityId == dto.Id);
            foreach (var applicant in applicants)
            {
                applicant.SpecialityId = dto.Id;
                _context.Applicants.Update(applicant);
            }
            await _context.SaveChangesAsync();

            var requiredExams = _context.RequiredExams.Where(x => x.SpecialityId == dto.Id);
            foreach (var requiredExam in requiredExams)
            {
                requiredExam.SpecialityId = dto.Id;
                _context.RequiredExams.Update(requiredExam);
            }
            await _context.SaveChangesAsync();
        }
    }
}