using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Demo.Authentication.Dto;
using Microsoft.Extensions.Configuration;

namespace WebApi.Integration.Services;

public class CookieAuthApiClient
{
    private HttpClient _httpClient;
    private readonly string _baseUri;

    public CookieAuthApiClient()
    {
        _httpClient = new HttpClient();
        var configuration = new ConfigurationBuilder()
            .AddJsonFile($"appsettings.json").Build();
        _baseUri = configuration["BaseUri"];
    }
    
    public async Task<HttpResponseMessage> GetAuthCookieAsync(string login, string password)
    {
        return await _httpClient.PostAsJsonAsync($"{_baseUri}/Auth/Login", new AuthDto { Login = login, Password = password });
    }
}