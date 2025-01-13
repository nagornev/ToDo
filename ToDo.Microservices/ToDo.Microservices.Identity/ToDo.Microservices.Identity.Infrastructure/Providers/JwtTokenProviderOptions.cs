namespace ToDo.Microservices.Identity.Infrastructure.Providers
{
    public class JwtTokenProviderOptions
    {
        public string Key { get; set; } = string.Empty;

        /// <summary>
        /// Lifetime in milliseconds.
        /// </summary>
        public int Lifetime { get; set; } = 43200000;
    }
}