using ToDo.Microservices.Identity.Database.Entities;
using ToDo.Microservices.Identity.Domain.Models;

namespace ToDo.Microservices.Identity.Database.Extensions
{
    public static class UserExtensions
    {
        public static UserEntity GetEntity(this User user)
        {
            return new UserEntity()
            {
                Id = user.Id,
                Email = user.Email,
                Password = user.Password,

                RoleId = (int)user.Access.Role
            };
        }
    }
}
