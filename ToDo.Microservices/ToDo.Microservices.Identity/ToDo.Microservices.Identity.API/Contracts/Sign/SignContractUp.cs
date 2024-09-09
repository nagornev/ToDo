namespace ToDo.Microservices.Identity.API.Contracts.Sign
{
    public class SignContractUp
    {
        public SignContractUp(string email,
                              string password)
        {
            Email = email;
            Password = password;
        }

        public string? Email { get; private set; }

        public string? Password { get; private set; }
    }
}
