using System;
using System.Security.Principal;

namespace SiteMonitoring.Authentication
{
    /// <summary>
    /// Идентити пользовательский
    /// </summary>
    public class UserIdentity : IIdentity
    {
        /// <inheritdoc />
        public string Name => "Form";

        /// <inheritdoc />
        public string AuthenticationType => "Form";

        /// <inheritdoc />
        public bool IsAuthenticated => UserId!= Guid.Empty;

        /// <summary>
        /// Идентификатор пользователя.
        /// </summary>
        public Guid UserId { get; set; }
    }
}
