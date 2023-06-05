using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Demo.Authentication.Dto;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace WebApi.Integration.Services;

public class TokenControllerClient
{
    private HttpClient _httpClient;
    private readonly string _baseUri;

    public TokenControllerClient()
    {
        _httpClient = new HttpClient();
        var configuration = new ConfigurationBuilder()
            .AddJsonFile($"appsettings.json").Build();
        _baseUri = configuration["BaseUri"];
    }
    
    public async Task<string> GetAdminTokenAsync()
    {
        var response = await _httpClient.PostAsJsonAsync($"{_baseUri}/token", new AuthDto { Login = "admin", Password = "admin" });
        var responseMessage = await response.Content.ReadAsStringAsync();
        var tokenDto = JsonConvert.DeserializeObject<TokenResultDto>(responseMessage);
        return tokenDto.IdToken;
    }
}