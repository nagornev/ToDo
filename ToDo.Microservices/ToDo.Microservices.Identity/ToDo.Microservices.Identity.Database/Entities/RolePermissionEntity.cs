using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDo.Microservices.Identity.Database.Entities
{
    internal class RolePermissionEntity
    {
        public int RoleId { get; set; }

        public int PermissionId { get; set; }
    }
}
