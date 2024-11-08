using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using UniversityAdmission.Models.Entities;

namespace UniversityAdmission.Models.DTO
{
    public class PermissionRequirement : IAuthorizationRequirement
    {
        public Permission[] Permissions { get; set; }
        public PermissionRequirement(params Permission[] permissions)
        {
            Permissions = permissions;
        }
    }
}