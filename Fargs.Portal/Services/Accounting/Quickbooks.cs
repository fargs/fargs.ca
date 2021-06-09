using Intuit.Ipp.OAuth2PlatformClient;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fargs.Portal.Services.Accounting
{
    public class Quickbooks
    {
        public const string _csrfToken = "DCD3A02B-CF59-42D7-B7E6-03C8A3D029A6";
        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly string _redirectUri;
        private readonly string _environment;
        private readonly List<OidcScopes> _scopes;

        public Quickbooks(IOptions<QuickbooksOptions> options) : this(options.Value) { }
        public Quickbooks(QuickbooksOptions options)
        {
            _clientId = options.ClientId;
            _clientSecret = options.ClientSecret;
            _redirectUri = options.RedirectUri;
            _environment = options.Environment;
            _scopes = new List<OidcScopes> { OidcScopes.Accounting, OidcScopes.Payment };
            
            var oauth2Client = new OAuth2Client(_clientId, _clientSecret, _redirectUri, _environment);

            AuthorizationUrl = oauth2Client.GetAuthorizationURL(_scopes, _csrfToken);
        }

        public string AuthorizationUrl { get; }
    }
}
