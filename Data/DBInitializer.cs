using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using MongoDB.Bson;
using UniversityAdmission.Models;
using UniversityAdmission.Models.Entities;
using UniversityAdmission.Services;

namespace UniversityAdmission.Data
{
    public class DBInitializer
    {
        public static void Initialize()
        {
            using UniversityAdmissionDBContext context = new();
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            // Add users
            var user = new User
            {
                Login = "root",
                Email = "root@example.com",
                Password = "1234",
                Role = Roles.Owner
            };
            context.Users.Add(user);
            context.SaveChanges();

            var user2 = new User
            {
                Login = "admin",
                Email = "admin@example.com",
                Password = "1234",
                Role = Roles.Administrator
            };
            context.Users.Add(user2);
            context.SaveChanges();

            var user3 = new User
            {
                Login = "operator",
                Email = "operator@example.com",
                Password = "1234",
                Role = Roles.Operator
            };
            context.Users.Add(user3);
            context.SaveChanges();

            var user4 = new User
            {
                Login = "user",
                Email = "user@example.com",
                Password = "1234",
                Role = Roles.Default
            };
            context.Users.Add(user4);
            context.SaveChanges();

            // Add faculties
            var faculty1 = new Faculty
            {
                Name = "Faculty of Science",
                Description = "Faculty description"
            };
            context.Faculties.Add(faculty1);
            context.SaveChanges();

            var faculty2 = new Faculty
            {
                Name = "Faculty of Arts",
                Description = "Faculty description"
            };
            context.Faculties.Add(faculty2);
            context.SaveChanges();

            // Add departments
            var department = new Department
            {
                Name = "Computer Science",
                Description = "Department description",
                FacultyId = faculty1.Id,
                Faculty = faculty1
            };
            context.Departments.Add(department);
            context.SaveChanges();

            var department2 = new Department
            {
                Name = "Arts",
                Description = "Department description",
                FacultyId = faculty2.Id,
                Faculty = faculty2
            };
            context.Departments.Add(department2);
            context.SaveChanges();
        }
    }
}