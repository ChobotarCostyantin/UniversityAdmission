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
        private readonly GroupApplicantRepository _groupApplicantRepository;
        private readonly ExamResultRepository _examResultRepository;

        public ApplicantRepository(UniversityAdmissionDBContext context, GroupApplicantRepository groupApplicantRepository,
            ExamResultRepository examResultRepository)
        {
            _context = context;
            _groupApplicantRepository = groupApplicantRepository;
            _examResultRepository = examResultRepository;
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
            var groupApplicants = _context.GroupApplicants.Where(x => x.ApplicantId == id);
            foreach (var groupApplicant in groupApplicants)
            {
                await _groupApplicantRepository.DeleteByIdAsync(groupApplicant.Id);
            }

            var examResults = _context.ExamResults.Where(x => x.ApplicantId == id);
            foreach (var examResult in examResults)
            {
                await _examResultRepository.DeleteByIdAsync(examResult.Id);
            }

            var applicant = await _context.Applicants.FindAsync(id);
            if (applicant != null)
            {
                _context.Applicants.Remove(applicant);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Applicant>> GetAll()
        {
            var applicants = await _context.Applicants.ToListAsync();
            foreach (var applicant in applicants)
            {
                applicant.Speciality = await _context.Specialities.FirstOrDefaultAsync(x => x.Id == applicant.SpecialityId);
            }
            return applicants;
        }

        public async Task<Applicant?> GetById(ObjectId id)
        {
            var applicant = await _context.Applicants.FirstOrDefaultAsync(x => x.Id == id);
            if (applicant != null)
            {
                applicant.Speciality = await _context.Specialities.FirstOrDefaultAsync(x => x.Id == applicant.SpecialityId);
            }
            return applicant;
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