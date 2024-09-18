using FreeCourse.Shared.DTOs;
using FreeCourse.Web.Models;
using FreeCourse.Web.Services.Interfaces;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Globalization;
using System.Security.Claims;
using System.Text.Json;

namespace FreeCourse.Web.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ClientSettings _clientSettings;
        private readonly ServiceApiSettings _serviceApiSettings;

        public IdentityService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor, IOptions<ClientSettings> clientSettings, IOptions<ServiceApiSettings> serviceApiSettings)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
            _clientSettings = clientSettings.Value;
            _serviceApiSettings = serviceApiSettings.Value;
        }

        public async Task<TokenResponse> GetAccessTokenByRefreshToken()
        {
            var discovery = await _httpClient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest()
            {
                Address = _serviceApiSettings.IdentityBaseUri,
                Policy = new DiscoveryPolicy()
                {
                    RequireHttps = false
                },
            });

            if (discovery.IsError) throw discovery.Exception;

            var refreshToken = await _httpContextAccessor.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.RefreshToken);

            var refreshTokenRequest = new RefreshTokenRequest()
            {
                ClientId = _clientSettings.WebClientForUser.ClientId,
                ClientSecret = _clientSettings.WebClientForUser.ClientSecret,
                RefreshToken = refreshToken,
                Address = discovery.TokenEndpoint,
            };

            var tokenResponse = await _httpClient.RequestRefreshTokenAsync(refreshTokenRequest);

            if (tokenResponse.IsError) return null;

            var authenticationTokens = new List<AuthenticationToken>()
            {
                new AuthenticationToken() {Name = OpenIdConnectParameterNames.AccessToken, Value = tokenResponse.AccessToken},
                new AuthenticationToken() {Name = OpenIdConnectParameterNames.RefreshToken, Value = tokenResponse.RefreshToken},
                new AuthenticationToken() {Name = OpenIdConnectParameterNames.ExpiresIn, Value = DateTime.Now.AddSeconds(tokenResponse.ExpiresIn).ToString("o", CultureInfo.InvariantCulture)},
            };

            var authenticationResult = await _httpContextAccessor.HttpContext.AuthenticateAsync();
            var authenticationProperties = authenticationResult.Properties;
            authenticationProperties.StoreTokens(authenticationTokens);

            await _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, authenticationResult.Principal, authenticationProperties);

            return tokenResponse;
        }

        public async Task RevokeRefreshToken()
        {
            var discovery = await _httpClient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest()
            {
                Address = _serviceApiSettings.IdentityBaseUri,
                Policy = new DiscoveryPolicy()
                {
                    RequireHttps = false
                },
            });

            if (discovery.IsError) throw discovery.Exception;

            var refreshToken = await _httpContextAccessor.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.RefreshToken);

            TokenRevocationRequest tokenRevocationRequest = new TokenRevocationRequest()
            {
                ClientId = _clientSettings.WebClientForUser.ClientId,
                ClientSecret = _clientSettings.WebClientForUser.ClientSecret,
                Address = discovery.RevocationEndpoint,
                Token = refreshToken,
                TokenTypeHint = "refresh_token"
            };

            var tokenRevocationResponse = await _httpClient.RevokeTokenAsync(tokenRevocationRequest);

            if (tokenRevocationResponse.IsError) throw tokenRevocationResponse.Exception;
        }

        public async Task<ResponseDTO<bool>> SignIn(SignInInput signInInput)
        {
            var discovery = await _httpClient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest()
            {
                Address = _serviceApiSettings.IdentityBaseUri,
                Policy = new DiscoveryPolicy()
                {
                    RequireHttps = false
                },
            });

            if (discovery.IsError) throw discovery.Exception;

            var passwordTokenRequest = new PasswordTokenRequest()
            {
                ClientId = _clientSettings.WebClientForUser.ClientId,
                ClientSecret = _clientSettings.WebClientForUser.ClientSecret,
                UserName = signInInput.Email,
                Password = signInInput.Password,
                Address = discovery.TokenEndpoint
            };

            var tokenResponse = await _httpClient.RequestPasswordTokenAsync(passwordTokenRequest);

            if (tokenResponse.IsError)
            {
                var responseContent = await tokenResponse.HttpResponse.Content.ReadAsStringAsync();

                var errorDTO = JsonSerializer.Deserialize<ErrorDTO>(responseContent, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

                return ResponseDTO<bool>.Fail(errorDTO.Errors, 400);
            }

            var userInfoRequest = new UserInfoRequest()
            {
                Token = tokenResponse.AccessToken,
                Address = discovery.UserInfoEndpoint,
            };

            var userInfoResponse = await _httpClient.GetUserInfoAsync(userInfoRequest);

            if (userInfoResponse.IsError) throw userInfoResponse.Exception;

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(userInfoResponse.Claims, CookieAuthenticationDefaults.AuthenticationScheme, "name", "role");

            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            var authenticationProperties = new AuthenticationProperties();

            authenticationProperties.StoreTokens(new List<AuthenticationToken>()
            {
                new AuthenticationToken() {Name = OpenIdConnectParameterNames.AccessToken, Value = tokenResponse.AccessToken},
                new AuthenticationToken() {Name = OpenIdConnectParameterNames.RefreshToken, Value = tokenResponse.RefreshToken},
                new AuthenticationToken() {Name = OpenIdConnectParameterNames.ExpiresIn, Value = DateTime.Now.AddSeconds(tokenResponse.ExpiresIn).ToString("o", CultureInfo.InvariantCulture)},
            });

            authenticationProperties.IsPersistent = signInInput.IsRemember;

            await _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal, authenticationProperties);

            return ResponseDTO<bool>.Success(200);
        }
    }
}
