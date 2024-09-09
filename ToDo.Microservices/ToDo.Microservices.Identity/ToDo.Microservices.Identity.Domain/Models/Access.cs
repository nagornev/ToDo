using System;
using System.Collections.Generic;
using System.Linq;
using ToDo.Domain.Results;

namespace ToDo.Microservices.Identity.Domain.Models
{
    public class Access
    {
        private static IReadOnlyDictionary<Role, IEnumerable<Permission>> _accesses = new Dictionary<Role, IEnumerable<Permission>>
        {
            { Role.Super, new[] { Permission.Super, Permission.User } },
            { Role.User, new[] { Permission.User } }
        };

        private Access(Role role)
        {
            Role = role;
            Permissions = _accesses[role];
        }

        public Role Role { get; private set; }

        public IEnumerable<Permission> Permissions { get; private set; }

        public bool IsContained(params Permission[] permissions)
        {
            return IsContained(permissions);
        }

        public bool IsContained(IEnumerable<Permission> permissions)
        {
            foreach (Permission required in permissions)
            {
                if (!Permissions.Contains(required))
                    return false;
            }

            return true;
        }

        public static Access Constructor(Role role)
        {
            return new Access(role);
        }

        public static Result<Access> Create(Role role)
        {
            if (!Enum.IsDefined(typeof(Role), role))
                return Result<Access>.Failure(Errors.IsInvalidArgument($"The '{role}' role is not defined."));

            if (!_accesses.ContainsKey(role))
                return Result<Access>.Failure(Errors.IsInvalidArgument($"The access by '{role}' role is not contains parameters."));

            return Result<Access>.Successful(Constructor(role));
        }
    }
}
