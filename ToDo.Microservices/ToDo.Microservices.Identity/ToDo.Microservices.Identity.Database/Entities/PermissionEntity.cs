using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDo.Microservices.Identity.Database.Entities
{
    public class PermissionEntity
    {
        public int Id { get; set; }
        
        public string Name { get; set; }

        public IEnumerable<RoleEntity> Roles { get; set; }
    }
}
