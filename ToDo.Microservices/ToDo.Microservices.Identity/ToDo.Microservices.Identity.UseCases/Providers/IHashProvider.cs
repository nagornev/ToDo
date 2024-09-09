namespace ToDo.Microservices.Identity.UseCases.Providers
{
    public interface IHashProvider
    {
        string Hash(string value);

        bool Verify(string value, string hash);
    }
}
