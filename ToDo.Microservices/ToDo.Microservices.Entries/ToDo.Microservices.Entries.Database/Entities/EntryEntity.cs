using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDo.Microservices.Entries.Database.Entities
{
    public class EntryEntity
    {
        public Guid Id { get; set; }

        public Guid CategoryId { get; set; }

        public string Text { get; set; }

        public DateTime? Deadline { get; set; }

        public bool Completed { get; set; }


        public Guid UserId { get; set; }

        public UserEntity User { get; set; }
    }
}
