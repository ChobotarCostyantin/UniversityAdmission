using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MongoDB.EntityFrameworkCore;
using UniversityAdmission.Data;
using UniversityAdmission.Models;

namespace UniversityAdmission.Services
{
    public class UniversityAdmissionDBContext : DbContext
    {
        public DbSet<User> Users { get; init; }
        public DbSet<Faculty> Faculties { get; init; }
        public DbSet<Department> Departments { get; init; }

        public UniversityAdmissionDBContext(DbContextOptions options)
            : base(options)
        {
            
        }

        public UniversityAdmissionDBContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
        }
    }
}