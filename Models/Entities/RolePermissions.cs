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
            { Roles.Owner, new List<Permission> { Permission.Create, Permission.Read, Permission.Update, Permission.Delete } },
            { Roles.Administrator, new List<Permission> { Permission.Create, Permission.Read, Permission.Update, Permission.Delete } },
            { Roles.Operator, new List<Permission> { Permission.Create, Permission.Read, Permission.Update, Permission.Delete } },
            { Roles.Default, new List<Permission> { Permission.Read } }
        };

        public static readonly ReadOnlyDictionary<Roles, List<Permission>> Permissions = 
            new(_permissions);
    }
}