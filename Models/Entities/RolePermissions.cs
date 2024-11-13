using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace UniversityAdmission.Models.Entities
{
    public class RolePermissions
    {
        private static readonly Dictionary<Roles, List<Permission>> _permissions = new()
        {
            { Roles.Owner, new List<Permission> { Permission.OwnerOnly, Permission.AdministratorOnly, Permission.OperatorOnly, Permission.Default } },
            { Roles.Administrator, new List<Permission> { Permission.AdministratorOnly, Permission.OperatorOnly, Permission.Default } },
            { Roles.Operator, new List<Permission> { Permission.OperatorOnly, Permission.Default } },
            { Roles.Default, new List<Permission> { Permission.Default } }
        };

        public static readonly ReadOnlyDictionary<Roles, List<Permission>> Permissions = 
            new(_permissions);
    }
}