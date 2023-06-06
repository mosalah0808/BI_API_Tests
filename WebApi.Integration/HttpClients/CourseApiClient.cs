using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using WebApi.Models;

namespace WebApi.Integration.Services;

public class CourseApiClient
{
    private HttpClient _httpClient;
    private readonly string _baseUri;

    public CourseApiClient()
    {
        _httpClient = new HttpClient();
        var configuration = new ConfigurationBuilder()
            .AddJsonFile($"appsettings.json").Build();
        _baseUri = configuration["BaseUri"];
    }

    public async Task<HttpResponseMessage> CreateCourseAsync(CourseModel course, string cookie = null)
    {
        if (cookie != null)
        {
            AddAuthCookie(cookie);
        }
        return await _httpClient.PostAsJsonAsync($"{_baseUri}/course", course);
    }

    private void AddAuthCookie(string cookie)
    {
        _httpClient.DefaultRequestHeaders.Add("cookie", cookie);
    }
}