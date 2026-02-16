using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace OrderManagementSystem.Web.Mvc.Auth
{
    public class JwtCookieAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly IConfiguration _config;

        public JwtCookieAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, IConfiguration config): base(options, logger, encoder)
        {
            _config = config;
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var token = Request.Cookies["access_token"];
            if (string.IsNullOrWhiteSpace(token))
                return Task.FromResult(AuthenticateResult.NoResult());

            var jwt = _config.GetSection("Jwt");
            var key = jwt["Key"]!;
            var issuer = jwt["Issuer"]!;
            var audience = jwt["Audience"]!;

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = issuer,
                ValidAudience = audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                ClockSkew = TimeSpan.FromSeconds(30)
            };

            try
            {
                var handler = new JwtSecurityTokenHandler();
                var principal = handler.ValidateToken(token, tokenValidationParameters, out var validatedToken);

                // Identity.IsAuthenticated olması için authType veriyoruz
                var identity = principal.Identity as System.Security.Claims.ClaimsIdentity;
                if(identity != null && string.IsNullOrEmpty(identity.AuthenticationType))
                {
                    identity = new System.Security.Claims.ClaimsIdentity(identity.Claims, Scheme.Name);
                    principal = new System.Security.Claims.ClaimsPrincipal(identity);
                }

                var ticket = new AuthenticationTicket(principal, Scheme.Name);
                return Task.FromResult(AuthenticateResult.Success(ticket));
            }
            catch 
            {
                // Token bozuk/expired ise cookie’yi temizlemek mantıklı
                Response.Cookies.Delete("access_token");
                return Task.FromResult(AuthenticateResult.Fail("Invalid token"));
            }
        }
    }
}
