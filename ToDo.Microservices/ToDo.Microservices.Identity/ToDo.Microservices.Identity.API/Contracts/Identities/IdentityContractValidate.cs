using ToDo.Microservices.Identity.Domain.Models;

namespace ToDo.Microservices.Identity.API.Contracts.Sign
{
    public class IdentityContractValidate
    {
        public IdentityContractValidate(IEnumerable<Permission> permissions)
        {
            Permissions = permissions;
        }
        
        public IEnumerable<Permission>? Permissions { get; private set; }
    }
}
