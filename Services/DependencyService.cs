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

            services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();
        }
    }
}