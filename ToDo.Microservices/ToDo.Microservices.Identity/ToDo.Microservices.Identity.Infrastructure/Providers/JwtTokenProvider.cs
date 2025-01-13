using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ToDo.Microservices.Identity.Domain.Models;
using ToDo.Microservices.Identity.UseCases.Providers;

namespace ToDo.Microservices.Identity.Infrastructure.Providers
{
    public class JwtTokenProvider : ITokenProvider
    {
        private JwtTokenProviderOptions _options;

        public JwtTokenProvider(IOptions<JwtTokenProviderOptions> options)
        {
            _options = options.Value;
        }

        public string Create(User user)
        {
            Claim[] claims =
            {
                new(JwtTokenProviderDefaults.Subject, $"{user.Id}"),
            };

            SigningCredentials credentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Key)),
                                                                    SecurityAlgorithms.HmacSha256);

            JwtSecurityToken token = new JwtSecurityToken(claims: claims,
                                                          signingCredentials: credentials,
                                                          expires: DateTime.UtcNow.AddMilliseconds(_options.Lifetime));

            string value = new JwtSecurityTokenHandler().WriteToken(token);

            return value;
        }

        public bool Validate(string token, out string subject)
        {
            bool response = Validate(token, out IEnumerable<Claim>? claims);

            subject = claims?.FirstOrDefault(x => x.Type == JwtTokenProviderDefaults.Subject)?
                             .Value ?? string.Empty;

            return response;
        }

        private bool Validate(string token, out IEnumerable<Claim>? claims)
        {
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();

            TokenValidationParameters parameters = new TokenValidationParameters()
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Key)),
            };

            ClaimsPrincipal? principal = default;

            try
            {
                principal = handler.ValidateToken(token, parameters, out SecurityToken security);
            }
            catch
            {
            }
            finally
            {
                claims = principal?.Claims ?? default;
            }

            return principal != null;
        }
    }
}
