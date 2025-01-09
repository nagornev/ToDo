using ToDo.Domain.Results;
using ToDo.Microservices.Identity.Database.Entities;
using ToDo.Microservices.Identity.Domain.Models;

namespace ToDo.Microservices.Identity.Database.Extensions
{
    public static class UserEntityExtensions
    {
        public static Result<User> GetDomain(this UserEntity userEntity)
        {
            return User.Constructor(userEntity.Id,
                                    userEntity.Email,
                                    userEntity.Password,
                                    Access.Constructor((Role)userEntity.RoleId));
        }
    }
}
