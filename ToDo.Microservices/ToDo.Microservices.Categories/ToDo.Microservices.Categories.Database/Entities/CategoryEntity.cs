using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDo.Microservices.Categories.Database.Entities
{
    public class CategoryEntity
    {
        public Guid Id { get; set; }
        
        public string Name { get; set; }

        
        public Guid UserId { get; set; }

        public UserEntity User { get; set; }
    }
}
