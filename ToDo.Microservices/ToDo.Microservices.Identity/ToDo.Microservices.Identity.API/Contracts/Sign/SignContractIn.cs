namespace ToDo.Microservices.Identity.API.Contracts.Sign
{
    public class SignContractIn
    {
        public SignContractIn(string email,
                              string password)
        {
            Email = email;
            Password = password;
        }

        public string? Email { get; private set; }

        public string? Password { get; private set; }
    }
}
