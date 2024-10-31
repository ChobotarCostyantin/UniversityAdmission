using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MongoDB.Bson;
using UniversityAdmission.Models;
using UniversityAdmission.Models.Entities;
using UniversityAdmission.Services;

namespace UniversityAdmission.Data
{
    public class DBInitializer
    {
        public static void ReCreate(bool recreate = true)
        {
            using (UniversityAdmissionDBContext context = new UniversityAdmissionDBContext())
            {
                // Recreate the database
                if (recreate)
                    context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
            }
        }

        public static void Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Faculty>(f =>
            {
                f.HasMany(faculty => faculty.Departments)
                    .WithOne(department => department.Faculty)
                    .HasForeignKey(department => department.FacultyId)
                    .HasPrincipalKey(faculty => faculty.Id)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<User>().HasData(
                    new User 
                    { 
                        Login = "Admin", 
                        Email = "admin@example.com", 
                        Password = "admin", 
                        Role = Roles.Administrator 
                    },
                    new User 
                    {
                        Login = "Student", 
                        Email = "student@example.com", 
                        Password = "student", 
                        Role = Roles.Default 
                    }
                );

            modelBuilder.Entity<Department>().HasData(
                    new Department 
                    {
                        Name = "Department of Physics", 
                    },
                    new Department 
                    {
                        Name = "Department of Chemistry",
                    }
                );
        }

        public static void SeedDataInCreatedDb(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<UniversityAdmissionDBContext>();
                    // Add users
                var users = new List<User>
                {
                    new User 
                    { 
                        Login = "Admin", 
                        Email = "admin@example.com", 
                        Password = "admin", 
                        Role = Roles.Administrator 
                    },
                    new User 
                    { 
                        Login = "Student", 
                        Email = "student@example.com", 
                        Password = "student", 
                        Role = Roles.Default 
                    }
                };
                context.Users.AddRange(users);
                context.SaveChanges();

                // Add faculties
                var faculties = new List<Faculty>
                {
                    new Faculty
                    {
                        Name = "Faculty of Science"
                    }
                };
                context.Faculties.AddRange(faculties);
                context.SaveChanges();

                // Add departments                    
                var departments = new List<Department>
                {
                    new Department 
                    { 
                        Name = "Department of Physics", 
                    },
                    new Department 
                    { 
                        Name = "Department of Chemistry", 
                    }
                };
                context.Departments.AddRange(departments);
                context.SaveChanges();
            }
        }
    }
}