using System.Net.Http;
using System.Threading.Tasks;
using Demo.Authentication.Dto;
using Newtonsoft.Json;

namespace WebApi.Integration.Services;

public class TokenService
{
    private TokenApiClient _tokenApiClient;

    public TokenService()
    {
        _tokenApiClient = new TokenApiClient();
    }
    
    public async Task<string> GetAdminTokenAsync()
    {
        return await GetTokenAsync("admin", "admin");
    }
    
    public async Task<string> GetTokenAsync(string name, string password)
    {
        var response = await GetTokenInternalAsync(name, password);
        var responseMessage = await response.Content.ReadAsStringAsync();
        var tokenDto = JsonConvert.DeserializeObject<TokenResultDto>(responseMessage);
        return tokenDto.IdToken;
    }
    
    public async Task<HttpResponseMessage> GetTokenInternalAsync(string name, string password)
    {
        return await _tokenApiClient.GetTokenInternalAsync(name, password);
    }
}