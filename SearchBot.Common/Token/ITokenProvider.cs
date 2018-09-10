using System.Threading.Tasks;

namespace SearchBot.Common
{
    public interface ITokenProvider
    {
        string GetToken();
        string GetToken(string resourceId, string tenant);
        string GetToken(string audience, string clientId, string clientSecret);
        string GetToken(string audience, string clientId, string clientSecret, string tenant);
        string GetToken(string audience, string clientId, string clientSecret, string tenant, string aaDInstance);
        Task<string> GetTokenAsync();
        Task<string> GetTokenAsync(string resourceId, string tenant);
        Task<string> GetTokenAsync(string audience, string clientId, string clientSecret);
        Task<string> GetTokenAsync(string audience, string clientId, string clientSecret, string tenant);
        Task<string> GetTokenAsync(string audience, string clientId, string clientSecret, string tenant, string aaDInstance);
    }
}