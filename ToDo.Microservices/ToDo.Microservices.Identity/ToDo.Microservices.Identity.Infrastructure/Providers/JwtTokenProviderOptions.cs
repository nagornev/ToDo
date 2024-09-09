namespace ToDo.Microservices.Identity.Infrastructure.Providers
{
    public class JwtTokenProviderOptions
    {
        public string Key { get; set; } = string.Empty;

        public int Lifetime { get; set; } = 12;

        public string Subject { get; set; } = string.Empty;
    }
}