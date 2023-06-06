using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Demo.Authentication.Dto;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

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
    
    public async Task<string> GetAuthCookieAsync()
    {
        var response = await _httpClient.PostAsJsonAsync($"{_baseUri}/Auth/Login", new AuthDto { Login = "admin", Password = "admin" });
        return response.Headers.FirstOrDefault(h=> h.Key == "Set-Cookie").Value.ToList().First();
    }
}