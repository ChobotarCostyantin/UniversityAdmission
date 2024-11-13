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
    public class ApplicantRepository
    {
        private readonly UniversityAdmissionDBContext _context;

        public ApplicantRepository(UniversityAdmissionDBContext context)
        {
            _context = context;
        }

        public async Task Create(ApplicantDTO dto)
        {
            Applicant applicant = new()
            {
                Id = dto.Id,
                FullName = dto.FullName,
                DateOfBirth = dto.DateOfBirth.AddHours(12),
                PhoneNumber = dto.PhoneNumber,
                Address = dto.Address,
                IsBeneficiary = dto.IsBeneficiary,
                SpecialityId = dto.SpecialityId
            };
            _context.Applicants.Add(applicant);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteByIdAsync(ObjectId id)
        {
            var applicant = await _context.Applicants.FindAsync(id);
            if (applicant != null)
            {
                _context.Applicants.Remove(applicant);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Applicant>> GetAll()
        {
            return await _context.Applicants.ToListAsync();
        }

        public async Task<Applicant?> GetById(ObjectId id)
        {
            return await _context.Applicants.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task Update(ApplicantDTO dto)
        {
            var applicant = await _context.Applicants.FirstOrDefaultAsync(x => x.Id == dto.Id);
            if (applicant != null)
            {
                applicant.FullName = dto.FullName;
                applicant.DateOfBirth = dto.DateOfBirth.AddHours(12);
                applicant.PhoneNumber = dto.PhoneNumber;
                applicant.Address = dto.Address;
                applicant.IsBeneficiary = dto.IsBeneficiary;
                if (applicant.SpecialityId != dto.SpecialityId)
                    applicant.TransferMechanism = true;
                applicant.SpecialityId = dto.SpecialityId;

                _context.Applicants.Update(applicant);
                await _context.SaveChangesAsync();
            }
        }
    }
}