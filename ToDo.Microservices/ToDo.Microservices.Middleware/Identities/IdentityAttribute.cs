namespace ToDo.Microservices.Middleware.Identities
{
    public class IdentityAttribute : Attribute
    {
        public IdentityAttribute(params int[] permissions)
        {
            Permissions = permissions;
        }

        public IEnumerable<int> Permissions { get; private set; }
    }
}
