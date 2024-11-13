using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using MongoDB.Bson;
using UniversityAdmission.Data.Repos;
using UniversityAdmission.Models.DTO;

namespace UniversityAdmission.Services
{
    public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        public PermissionAuthorizationHandler(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            var userId = context.User.Claims.FirstOrDefault(x => x.Type == "Id")?.Value;

            if (userId == null || !ObjectId.TryParse(userId, out ObjectId id))
                return;

            using var scope = _serviceScopeFactory.CreateScope();

            var userRepository = scope.ServiceProvider.GetRequiredService<UserRepository>();

            if (!userRepository.IsUser(id))
                return;

            var userPermissions = await userRepository.GetUserPermissionsAsync(id);

            Console.WriteLine($"Required permissions: {string.Join(", ", requirement.Permissions)}");
            Console.WriteLine($"User permissions: {string.Join(", ", userPermissions)}");

            if (userPermissions.Intersect(requirement.Permissions).Any())
            {
                Console.WriteLine("Access granted");
                context.Succeed(requirement);
            }
            else
            {
                Console.WriteLine("Access denied");
            }
        }
    }
}