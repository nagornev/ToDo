using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ToDo.Microservices.MQ.Publishers
{
    public class DeleteCategoryPublish
    {
        public const string Exchange = "delete_category_exchange";

        public DeleteCategoryPublish(Guid userId, Guid categoryId)
        {
            UserId = userId;
            CategoryId = categoryId;
        }

        [JsonPropertyName("userId")]
        public Guid UserId { get; private set; }

        [JsonPropertyName("categoryId")]
        public Guid CategoryId { get; private set; }
    }
}
