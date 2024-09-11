using ToDo.Microservices.Identity.Domain.Models;

namespace ToDo.Microservices.Identity.API.Contracts.Sign
{
    public class IdentityContractValidate
    {
        public IdentityContractValidate(string token,
                                  IEnumerable<Permission> permissions)
        {
            Token = token;
            Permissions = permissions;
        }

        public string? Token { get; private set; }

        public IEnumerable<Permission>? Permissions { get; private set; }
    }
}
