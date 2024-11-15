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
    public class GroupApplicantRepository
    {
        private readonly UniversityAdmissionDBContext _context;

        public GroupApplicantRepository(UniversityAdmissionDBContext context)
        {
            _context = context;
        }

        public async Task<List<GroupApplicant>> GetAll()
        {
            var groupApplicants = await _context.GroupApplicants.ToListAsync();
            foreach (var groupApplicant in groupApplicants)
            {
                groupApplicant.Applicant = await _context.Applicants.FirstOrDefaultAsync(x => x.Id == groupApplicant.ApplicantId);
            }
            return groupApplicants;
        }

        public async Task<List<GroupApplicant>> GetGroupApplicantsFromGroup(ObjectId groupId)
        {
            var groupApplicants = await _context.GroupApplicants.Where(x => x.GroupId == groupId).ToListAsync();
            foreach (var groupApplicant in groupApplicants)
            {
                groupApplicant.Applicant = await _context.Applicants.FirstOrDefaultAsync(x => x.Id == groupApplicant.ApplicantId);
            }
            return groupApplicants;
        }

        public async Task Create(GroupApplicantDTO dto)
        {
            GroupApplicant groupApplicant = new()
            {
                Id = dto.Id,
                GroupId = dto.GroupId,
                ApplicantId = dto.ApplicantId
            };
            _context.GroupApplicants.Add(groupApplicant);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteByIdAsync(ObjectId id)
        {
            var groupApplicant = await _context.GroupApplicants.FindAsync(id);
            if (groupApplicant != null)
            {
                _context.GroupApplicants.Remove(groupApplicant);
                await _context.SaveChangesAsync();
            }
        }
    }
}