using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Demo.Authentication.Dto;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace WebApi.Integration.Services;

public class TokenApiClient
{
    private HttpClient _httpClient;
    private readonly string _baseUri;

    public TokenApiClient()
    {
        _httpClient = new HttpClient();
        var configuration = new ConfigurationBuilder()
            .AddJsonFile($"appsettings.json").Build();
        _baseUri = configuration["BaseUri"];
    }
    
    public async Task<HttpResponseMessage> GetTokenInternalAsync(string name, string password)
    {
        return await _httpClient.PostAsJsonAsync($"{_baseUri}/token", new AuthDto { Login = name, Password = password });
    }
}