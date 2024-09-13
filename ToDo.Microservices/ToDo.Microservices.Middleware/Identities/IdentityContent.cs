using Newtonsoft.Json;

namespace ToDo.Microservices.Middleware.Identities
{
    public class IdentityContent
    {
        public IdentityContent(IEnumerable<int> permissions)
        {
            Permissions = permissions;
        }

        [JsonProperty("permissions")]
        public IEnumerable<int> Permissions { get; private set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
