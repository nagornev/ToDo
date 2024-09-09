using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDo.Microservices.Identity.Database.Entities
{
    public class RoleEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public IEnumerable<UserEntity> Users { get; set; }

        public IEnumerable<PermissionEntity> Permissions { get; set; }
    }
}
