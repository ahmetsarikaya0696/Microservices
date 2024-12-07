
using IdentityModel.Client;

namespace FreeCourse.Gateway.DelegateHandlers
{
    public class TokenExchangeDelegateHandler : DelegatingHandler
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private string _accessToken;

        public TokenExchangeDelegateHandler(IConfiguration configuration, HttpClient httpClient)
        {
            _configuration = configuration;
            _httpClient = httpClient;
        }

        private async Task<string> GetTokenAsync(string requestToken)
        {
            if (!string.IsNullOrEmpty(_accessToken))
            {
                return _accessToken;
            }

            var disco = await _httpClient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
            {
                Address = _configuration["IdentityServerUrl"],
                Policy = new DiscoveryPolicy { RequireHttps = false }
            });

            if (disco.IsError)
            {
                throw disco.Exception;
            }

            TokenExchangeTokenRequest tokenExchangeTokenRequest = new TokenExchangeTokenRequest()
            {
                Address = disco.TokenEndpoint,
                ClientId = _configuration["ClientId"],
                ClientSecret = _configuration["ClientSecret"],
                GrantType = _configuration["TokenGrantType"],
                SubjectToken = requestToken,
                SubjectTokenType = "urn:ietf:params:oauth:token-type:access-token",
                Scope = "openid discount_full_permission payment_full_permission",
            };

            var tokenResponse = await _httpClient.RequestTokenExchangeTokenAsync(tokenExchangeTokenRequest);
            if (tokenResponse.IsError)
            {
                throw tokenResponse.Exception;
            }

            _accessToken = tokenResponse.AccessToken;
            return _accessToken;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var requestToken = request.Headers.Authorization.Parameter;
            var newToken = await GetTokenAsync(requestToken);
            request.SetBearerToken(newToken);
            return await base.SendAsync(request, cancellationToken);
        }
    }
}
