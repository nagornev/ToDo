using Newtonsoft.Json;

namespace ToDo.Microservices.Middleware.Identities
{
    public class IdentityContent
    {
        public IdentityContent(IEnumerable<KeyValuePair<string, string>>  cookies, IEnumerable<int> permissions)
        {
            Cookies = cookies;
            Permissions = permissions;
        }

        [JsonProperty("cookies")]
        public IEnumerable<KeyValuePair<string, string>> Cookies { get; private set; }

        [JsonProperty("permissions")]
        public IEnumerable<int> Permissions { get; private set; }

        public string GetContent()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
