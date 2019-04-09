using System;
using System.Security.Authentication;
using System.Security.Principal;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SiteMonitoring.Services;

namespace SiteMonitoring.Authentication
{
    /// <summary>
    /// Аутентификационный хандлер
    /// </summary>
    public class AuthenticationHandler : AuthenticationHandler<CustomAuthenticationOptions>
    {
        public AuthenticationHandler(
            IOptionsMonitor<CustomAuthenticationOptions> authOptions,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock)
            : base(authOptions, logger, encoder, clock)
        {
        }

#pragma warning disable 1998
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
#pragma warning restore 1998
        {
            try
            {
                var authorizationString = this.Request.Headers["Authorization"].ToString();
                Guid userId = Guid.Empty;

                 //Сделать хранение тикета в куках и его восстановление. Пока проверяется при каждом запросе и только при наличии хедера авторизации
                if (!string.IsNullOrEmpty(authorizationString))
                {
                    var split = authorizationString.Split(" ");
                    var token = split.Length > 1 ? split[1] : split[0];
                    var credentials = JWTService.DecodeUserToken(token);
                    userId = Guid.Parse(credentials.Userid);
                }

                var identity = new UserIdentity
                {
                    UserId = userId
                };

                var principal = new GenericPrincipal(identity, null);
                var ticket = new AuthenticationTicket(principal, this.Scheme.Name);

                return AuthenticateResult.Success(ticket);
            }
            catch (Exception exception)
            {
                throw new AuthenticationException($"Аутентификация прошла с ошибкой: {exception}");
            }
        }
    }

    /// <summary>
    /// Кастомный класс схемы аутентификации
    /// </summary>
    public class CustomAuthenticationOptions : AuthenticationSchemeOptions
    {
        public UserIdentity Identity { get; set; }
    }

    public static class CustomAuthenticationExtensions
    {
        /// <summary>
        /// Добавить немного аутентификации в проект
        /// </summary>
        public static AuthenticationBuilder AddCustomAuthentication(this AuthenticationBuilder builder, string authenticationScheme, string displayName, Action<CustomAuthenticationOptions> configureOptions)
        {
            return builder.AddScheme<CustomAuthenticationOptions, AuthenticationHandler>(authenticationScheme, displayName, configureOptions);
        }
    }
}