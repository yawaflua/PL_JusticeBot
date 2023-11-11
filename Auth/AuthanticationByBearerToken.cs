using DiscordApp.Database;
using DiscordApp.Database.Tables;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Collections.Specialized;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace DiscordApp.Auth
{
    public class AuthanticationByBearerToken : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private AppDbContext dbContext;

        public AuthanticationByBearerToken(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            AppDbContext dbContext) : base(options, logger, encoder, clock)
        {
            this.dbContext = dbContext;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.TryGetValue("Authorization", out var apiKeyHeaderValues))
            {
                return AuthenticateResult.Fail("API Key was not provided.");
            }

#pragma warning disable CS8602 // –азыменование веро€тной пустой ссылки.
            string providedApiKey = apiKeyHeaderValues
                    .FirstOrDefault().Replace("'", "");
            if (CheckForInvalidCharacters(apiKeyHeaderValues)) return AuthenticateResult.Fail("Don`t use SQL injections, dog`s son");
#pragma warning restore CS8602 // –азыменование веро€тной пустой ссылки.

            if (IsValidApiKey(providedApiKey))
            {
                var claims = new[] { new Claim("Bearer", providedApiKey) };
                var identity = new ClaimsIdentity(claims, Scheme.Name);
                var principal = new ClaimsPrincipal(identity);
                var ticket = new AuthenticationTicket(principal, Scheme.Name);

                return AuthenticateResult.Success(ticket);
            }

            return AuthenticateResult.Fail("Invalid API Key provided.");
        }

        private bool IsValidApiKey(string apiKey)
        {
            return false;
        }
        private bool CheckForInvalidCharacters(string value)
        {
            return value.IndexOfAny(";".ToCharArray()) != -1;
        }
    }
}