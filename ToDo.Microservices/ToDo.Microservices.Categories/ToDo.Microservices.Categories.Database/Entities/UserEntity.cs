using System;
using System.Collections.Generic;
using System.Formats.Tar;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDo.Microservices.Categories.Database.Entities
{
    public class UserEntity
    {
        public Guid Id { get; set; }

        public IEnumerable<CategoryEntity> Categories { get; set; }
    }
}
