using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.EntityFrameworkCore;
using MongoDB.EntityFrameworkCore.Extensions;
using UniversityAdmission.Data;
using UniversityAdmission.Models;
using UniversityAdmission.Models.Entities;

namespace UniversityAdmission.Services
{
    public class UniversityAdmissionDBContext : DbContext
    {
        public DbSet<User> Users { get; init; }
        public DbSet<Faculty> Faculties { get; init; }
        public DbSet<Department> Departments { get; init; }
        public DbSet<Speciality> Specialities { get; init; }
        public DbSet<Applicant> Applicants { get; init; }

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
            
            modelBuilder.Entity<User>();
            modelBuilder.Entity<Faculty>();
            modelBuilder.Entity<Department>();
            modelBuilder.Entity<Speciality>();
            modelBuilder.Entity<Applicant>();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseMongoDB("mongodb://localhost:27017", "UniversityAdmission");
        }
    }
}