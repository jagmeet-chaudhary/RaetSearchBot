using System.Globalization;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System.Diagnostics.CodeAnalysis;
using System.Configuration;


namespace SearchBot.Common
{
    public class TokenProvider : ITokenProvider
    {
        public string GetToken(string audience, string clientId, string clientSecret, string tenant, string aaDInstance)
        {
            var authority = string.Format(CultureInfo.InvariantCulture, aaDInstance, tenant);
            var authContext = new AuthenticationContext(authority);
            var clientCredential = new ClientCredential(clientId, clientSecret);
            var authenticationResult = authContext.AcquireTokenAsync(audience, clientCredential).Result;
            return authenticationResult.AccessToken;
        }

        public string GetToken(string audience, string clientId, string clientSecret, string tenant)
        {
            var aaDInstance = ConfigurationManager.AppSettings.Get("AadInstance");
            return GetToken(audience, clientId, clientSecret, tenant, aaDInstance);
        }

        public string GetToken(string audience, string clientId, string clientSecret)
        {
            var masterTenant = ConfigurationManager.AppSettings.Get("MasterTenant");
            return GetToken(audience, clientId, clientSecret, masterTenant);
        }

        public string GetToken(string resourceId, string tenant)
        {
            var clientId = ConfigurationManager.AppSettings.Get("ClientId");
            var clientSecret = ConfigurationManager.AppSettings.Get("ClientSecret");
            return GetToken(resourceId, clientId, clientSecret, tenant);
        }

        public string GetToken()
        {
            var clientId = ConfigurationManager.AppSettings.Get("ClientId");
            var clientSecret = ConfigurationManager.AppSettings.Get("ClientSecret");
            var idaAudience = ConfigurationManager.AppSettings.Get("idaAudience");
            return GetToken(idaAudience, clientId, clientSecret);
        }

        [ExcludeFromCodeCoverage]
        public async Task<string> GetTokenAsync(string audience, string clientId, string clientSecret)
        {
            var masterTenant = ConfigurationManager.AppSettings.Get("MasterTenant");
            return await GetTokenAsync(audience, clientId, clientSecret, masterTenant);
        }

        [ExcludeFromCodeCoverage]
        public async Task<string> GetTokenAsync(string audience, string clientId, string clientSecret, string tenant, string aaDInstance)
        {
            var authority = string.Format(CultureInfo.InvariantCulture, aaDInstance, tenant);
            var authContext = new AuthenticationContext(authority);
            var clientCredential = new ClientCredential(clientId, clientSecret);
            var authenticationResult = await authContext.AcquireTokenAsync(audience, clientCredential);
            return authenticationResult.AccessToken;
        }

        [ExcludeFromCodeCoverage]
        public async Task<string> GetTokenAsync(string audience, string clientId, string clientSecret, string tenant)
        {
            var aaDInstance = ConfigurationManager.AppSettings.Get("AadInstance");
            return await GetTokenAsync(audience, clientId, clientSecret, tenant, aaDInstance);
        }

        [ExcludeFromCodeCoverage]
        public async Task<string> GetTokenAsync(string resourceId, string tenant)
        {
            var clientId = ConfigurationManager.AppSettings.Get("ClientId");
            var clientSecret = ConfigurationManager.AppSettings.Get("ClientSecret");
            return await GetTokenAsync(resourceId, clientId, clientSecret, tenant);
        }

        [ExcludeFromCodeCoverage]
        public async Task<string> GetTokenAsync()
        {
            var clientId = ConfigurationManager.AppSettings.Get("ClientId");
            var clientSecret = ConfigurationManager.AppSettings.Get("ClientSecret");
            var idaAudience = ConfigurationManager.AppSettings.Get("idaAudience");
            return await GetTokenAsync(idaAudience, clientId, clientSecret);
        }
    }
}
