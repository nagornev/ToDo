using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Text;
using ToDo.Microservices.Identity.UseCases.Providers;

namespace ToDo.Microservices.Identity.Infrastructure.Providers
{
    public class PasswordHashProvider : IHashProvider
    {
        private PasswordHashProviderOptions _options;

        public PasswordHashProvider(IOptions<PasswordHashProviderOptions> options)
        {
            _options = options.Value;
        }

        public string Hash(string value)
        {
            string hash = string.Empty;

            using (HMACSHA512 hasher = new HMACSHA512(Encoding.UTF8.GetBytes(_options.Key)))
            {
                byte[] buffer = hasher.ComputeHash(Encoding.UTF8.GetBytes(value));

                hash = Convert.ToBase64String(buffer);
            }

            return hash;
        }

        public bool Verify(string value, string hash)
        {
            string hashed = Hash(value);

            return hashed == hash;
        }
    }
}
