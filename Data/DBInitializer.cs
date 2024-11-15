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

            var exam3 = new Exam
            {
                Name = "Фізика",
                Subject = Subjects.Фізика,
                MinScore = 60,
                IsCreativeContest = false
            };
            context.Exams.Add(exam3);
            context.SaveChanges();

            var exam4 = new Exam
            {
                Name = "Хімія",
                Subject = Subjects.Хімія,
                MinScore = 55,
                IsCreativeContest = false
            };
            context.Exams.Add(exam4);
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

            var requiredExam3 = new RequiredExam
            {
                SpecialityId = speciality2.Id,
                ExamId = exam1.Id
            };
            context.RequiredExams.Add(requiredExam3);
            context.SaveChanges();


            var requiredExam5 = new RequiredExam
            {
                SpecialityId = speciality2.Id,
                ExamId = exam3.Id
            };
            context.RequiredExams.Add(requiredExam5);
            context.SaveChanges();

            var requiredExam6 = new RequiredExam
            {
                SpecialityId = speciality1.Id,
                ExamId = exam4.Id
            };
            context.RequiredExams.Add(requiredExam6);
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

            var applicant3 = new Applicant
            {
                FullName = "Петренко Олександр Олександрович",
                DateOfBirth = new DateTime(2003, 5, 14).AddHours(12),
                PhoneNumber = "+380501234567",
                Address = "Чернівці",
                IsBeneficiary = false,
                SpecialityId = speciality2.Id
            };
            context.Applicants.Add(applicant3);
            context.SaveChanges();

            // Add teachers
            var teacher1 = new Teacher
            {
                FullName = "Комісарчук Володимир Васильович",
                DateOfBirth = new DateTime(1982, 11, 12).AddHours(12),
                PhoneNumber = "+380501234567",
                Subject = Subjects.Математика
            };
            context.Teachers.Add(teacher1);
            context.SaveChanges();

            var teacher2 = new Teacher
            {
                FullName = "Валь Олександр Олександрович",
                DateOfBirth = new DateTime(1989, 5, 23).AddHours(12),
                PhoneNumber = "+380501234567",
                Subject = Subjects.Фізика
            };
            context.Teachers.Add(teacher2);
            context.SaveChanges();

            // Add group
            var group1 = new Group
            {
                Name = "Група №1",
                ExamDate = new DateTime(2024, 11, 21).AddHours(12)
            };
            context.Groups.Add(group1);
            context.SaveChanges();

            var group2 = new Group
            {
                Name = "Група №2",
                ExamDate = new DateTime(2024, 11, 23).AddHours(12)
            };
            context.Groups.Add(group2);
            context.SaveChanges();

            var groupApplicant1 = new GroupApplicant
            {
                GroupId = group1.Id,
                ApplicantId = applicant1.Id
            };
            context.GroupApplicants.Add(groupApplicant1);
            context.SaveChanges();

            var groupApplicant2 = new GroupApplicant
            {
                GroupId = group2.Id,
                ApplicantId = applicant2.Id
            };
            context.GroupApplicants.Add(groupApplicant2);
            context.SaveChanges();

            var groupApplicant3 = new GroupApplicant
            {
                GroupId = group1.Id,
                ApplicantId = applicant3.Id
            };
            context.GroupApplicants.Add(groupApplicant3);
            context.SaveChanges();

            var groupApplicant4 = new GroupApplicant
            {
                GroupId = group2.Id,
                ApplicantId = applicant3.Id
            };
            context.GroupApplicants.Add(groupApplicant4);
            context.SaveChanges();

            var groupTeacher1 = new GroupTeacher
            {
                GroupId = group1.Id,
                TeacherId = teacher1.Id
            };
            context.GroupTeachers.Add(groupTeacher1);
            context.SaveChanges();

            var groupTeacher2 = new GroupTeacher
            {
                GroupId = group2.Id,
                TeacherId = teacher2.Id
            };
            context.GroupTeachers.Add(groupTeacher2);
            context.SaveChanges();

            var teacherExam1 = new TeacherExam
            {
                TeacherId = teacher1.Id,
                ExamId = exam1.Id
            };
            context.TeacherExams.Add(teacherExam1);
            context.SaveChanges();

            var groupExam1 = new GroupExam
            {
                GroupId = group1.Id,
                ExamId = exam1.Id
            };
            context.GroupExams.Add(groupExam1);
            context.SaveChanges();

            var groupExam2 = new GroupExam
            {
                GroupId = group1.Id,
                ExamId = exam2.Id
            };
            context.GroupExams.Add(groupExam2);
            context.SaveChanges();

            var groupExam3 = new GroupExam
            {
                GroupId = group1.Id,
                ExamId = exam3.Id
            };
            context.GroupExams.Add(groupExam3);
            context.SaveChanges();

            var groupExam4 = new GroupExam
            {
                GroupId = group2.Id,
                ExamId = exam1.Id
            };
            context.GroupExams.Add(groupExam4);
            context.SaveChanges();

            var groupExam5 = new GroupExam
            {
                GroupId = group2.Id,
                ExamId = exam2.Id
            };
            context.GroupExams.Add(groupExam5);
            context.SaveChanges();

            var groupExam6 = new GroupExam
            {
                GroupId = group2.Id,
                ExamId = exam3.Id
            };
            context.GroupExams.Add(groupExam6);
            context.SaveChanges();

            var groupExam7 = new GroupExam
            {
                GroupId = group2.Id,
                ExamId = exam4.Id
            };
            context.GroupExams.Add(groupExam7);
            context.SaveChanges();
        }
    }
}