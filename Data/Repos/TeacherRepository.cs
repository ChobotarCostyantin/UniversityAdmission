using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniversityAdmission.Services;

namespace UniversityAdmission.Data.Repos
{
    public class TeacherRepository
    {
        private readonly UniversityAdmissionDBContext _context;

        public TeacherRepository(UniversityAdmissionDBContext context)
        {
            _context = context;
        }

        // public async Task Create(TeacherDTO dto)
    }
}