using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
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
        public DbSet<Exam> Exams { get; init; }
        public DbSet<RequiredExam> RequiredExams { get; init; }
        public DbSet<ExamResult> ExamResults { get; init; }
        public DbSet<Applicant> Applicants { get; init; }
        public DbSet<Teacher> Teachers { get; init; }
        public DbSet<TeacherExam> TeacherExams { get; init; }
        public DbSet<Group> Groups { get; init; }
        public DbSet<GroupExam> GroupExams { get; init; }
        public DbSet<GroupTeacher> GroupTeachers { get; init; }
        public DbSet<GroupApplicant> GroupApplicants { get; init; }

        public UniversityAdmissionDBContext(DbContextOptions options)
            : base(options)
        {
            Database.AutoTransactionBehavior = AutoTransactionBehavior.Never;
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
            modelBuilder.Entity<Exam>();
            modelBuilder.Entity<RequiredExam>();
            modelBuilder.Entity<ExamResult>();
            modelBuilder.Entity<Applicant>();
            modelBuilder.Entity<Teacher>();
            modelBuilder.Entity<TeacherExam>();
            modelBuilder.Entity<Group>();
            modelBuilder.Entity<GroupExam>();
            modelBuilder.Entity<GroupTeacher>();
            modelBuilder.Entity<GroupApplicant>();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseMongoDB("mongodb://localhost:27017", "UniversityAdmission");
        }
    }
}