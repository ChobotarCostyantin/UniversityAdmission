using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using UniversityAdmission.Data.Repos;

namespace UniversityAdmission.Services
{
    public class DependencyService
    {
        public static void InjectServices(IServiceCollection services)
        { 
            services.AddScoped<UserRepository>();
            services.AddScoped<UserService>();
            services.AddScoped<FacultyRepository>();
            services.AddScoped<DepartmentRepository>();
            services.AddScoped<SpecialityRepository>();
            services.AddScoped<RequiredExamRepository>();
            services.AddScoped<ExamRepository>();
            services.AddScoped<TeacherRepository>();
            services.AddScoped<ApplicantRepository>();
            services.AddScoped<TeacherExamRepository>();
            services.AddScoped<GroupRepository>();
            services.AddScoped<GroupExamRepository>();
            services.AddScoped<GroupTeacherRepository>();
            services.AddScoped<GroupApplicantRepository>();
            services.AddScoped<ExamResultRepository>();

            services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();
        }
    }
}