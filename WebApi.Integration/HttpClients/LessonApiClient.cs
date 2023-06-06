using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using WebApi.Models;

namespace WebApi.Integration.Services;

public class LessonApiClient
{
    private HttpClient _httpClient;
    private readonly string _baseUri;

    public LessonApiClient()
    {
        _httpClient = new HttpClient();
        var configuration = new ConfigurationBuilder()
            .AddJsonFile($"appsettings.json").Build();
        _baseUri = configuration["BaseUri"];
    }
    
    public async Task<HttpResponseMessage> GetLessonAsync(int id, string token = null)
    {
        if (token != null)
        {
            AddAuthCookie(token);
        }
        return await _httpClient.GetAsync($"{_baseUri}/lesson/{id}");
    }
    
    public async Task<HttpResponseMessage> AddLessonAsync(LessonModel lessonModel, string token = null)
    {
        if (token != null)
        {
            AddAuthCookie(token);
        }
        return await _httpClient.PostAsJsonAsync($"{_baseUri}/lesson", lessonModel);
    }
    
    private void AddAuthCookie(string token)
    {
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");    
    }
}