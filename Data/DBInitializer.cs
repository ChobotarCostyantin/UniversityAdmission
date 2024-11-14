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
                Name = "ННІФТКН",
                Description = "Навчально-науковий інститут фізико-технічних та комп'ютерних наук"
            };
            context.Faculties.Add(faculty1);
            context.SaveChanges();

            var faculty2 = new Faculty
            {
                Name = "Факультет математики та інформатики",
                Description = "Опис факультета"
            };
            context.Faculties.Add(faculty2);
            context.SaveChanges();

            // Add departments
            var department = new Department
            {
                Name = "Кафедра програмного забезпечення комп’ютерних систем",
                Description = "Опис кафедри",
                FacultyId = faculty1.Id,
            };
            context.Departments.Add(department);
            context.SaveChanges();

            var department2 = new Department
            {
                Name = "Кафедра математичного аналізу",
                Description = "Опис кафедри",
                FacultyId = faculty2.Id,
            };
            context.Departments.Add(department2);
            context.SaveChanges();

            // Add specialities
            var speciality1 = new Speciality
            {
                Name = "ІПЗ",
                Description = "Інженерія програмного забезпечення",
                DepartmentId = department.Id,
            };
            context.Specialities.Add(speciality1);
            context.SaveChanges();

            var speciality2 = new Speciality
            {
                Name = "Системний аналіз",
                Description = "Опис спеціальності",
                DepartmentId = department2.Id,
            };
            context.Specialities.Add(speciality2);
            context.SaveChanges();

            // Add exams
            var exam1 = new Exam
            {
                Name = "Математика",
                Subject = Subjects.Математика,
                MinScore = 65,
                IsCreativeContest = false
            };
            context.Exams.Add(exam1);
            context.SaveChanges();

            var exam2 = new Exam
            {
                Name = "Інформатика",
                Subject = Subjects.Інформатика,
                MinScore = 50,
                IsCreativeContest = true
            };
            context.Exams.Add(exam2);
            context.SaveChanges();

            // Add required exams
            var requiredExam1 = new RequiredExam
            {
                SpecialityId = speciality1.Id,
                ExamId = exam1.Id
            };
            context.RequiredExams.Add(requiredExam1);
            context.SaveChanges();

            var requiredExam2 = new RequiredExam
            {
                SpecialityId = speciality1.Id,
                ExamId = exam2.Id
            };
            context.RequiredExams.Add(requiredExam2);
            context.SaveChanges();

            // Add applicants
            var applicant1 = new Applicant
            {
                FullName = "Максимчук Олег Святославович",
                DateOfBirth = new DateTime(2005, 01, 14).AddHours(12),
                PhoneNumber = "+380501234567",
                Address = "Перебиківці",
                IsBeneficiary = true,
                SpecialityId = speciality1.Id
            };
            context.Applicants.Add(applicant1);
            context.SaveChanges();

            var applicant2 = new Applicant
            {
                FullName = "Олегчук Максим Валерійович",
                DateOfBirth = new DateTime(2004, 6, 16).AddHours(12),
                PhoneNumber = "+380501234567",
                Address = "Керевипівці",
                IsBeneficiary = false,
                SpecialityId = speciality2.Id
            };
            context.Applicants.Add(applicant2);
            context.SaveChanges();

            // Add teachers
            var teacher1 = new Teacher
            {
                FullName = "Комісарчук Володимир Васильович",
                DateOfBirth = new DateTime(1982, 11, 12).AddHours(12),
                PhoneNumber = "+380501234567"
            };
            context.Teachers.Add(teacher1);
            context.SaveChanges();

            var teacher2 = new Teacher
            {
                FullName = "Валь Олександр Олександрович",
                DateOfBirth = new DateTime(1989, 5, 23).AddHours(12),
                PhoneNumber = "+380501234567"
            };
            context.Teachers.Add(teacher2);
            context.SaveChanges();
        }
    }
}