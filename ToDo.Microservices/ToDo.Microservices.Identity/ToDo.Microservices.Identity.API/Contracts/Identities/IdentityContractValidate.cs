using ToDo.Microservices.Identity.Domain.Models;

namespace ToDo.Microservices.Identity.API.Contracts.Sign
{
    public class IdentityContractValidate
    {
        public IdentityContractValidate(IEnumerable<KeyValuePair<string, string>> cookies, 
                                        IEnumerable<Permission> permissions)
        {
            Cookies = cookies;
            Permissions = permissions;
        }
        
        public IEnumerable<KeyValuePair<string, string>>? Cookies { get; private set; }

        public IEnumerable<Permission>? Permissions { get; private set; }
    }
}
