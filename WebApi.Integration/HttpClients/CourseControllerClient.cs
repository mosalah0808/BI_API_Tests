using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using WebApi.Models;

namespace WebApi.Integration.Services;

public class CourseControllerClient
{
    private HttpClient _httpClient;
    private readonly string _baseUri;

    public CourseControllerClient()
    {
        _httpClient = new HttpClient();
        var configuration = new ConfigurationBuilder()
            .AddJsonFile($"appsettings.json").Build();
        _baseUri = configuration["BaseUri"];
    }
    
    public async Task<int> CreateCourseAsync(CourseModel course)
    {
        var addCourseResponse = await _httpClient.PostAsJsonAsync($"{_baseUri}/course", course);
        return JsonConvert.DeserializeObject<int>(await addCourseResponse.Content.ReadAsStringAsync());
    }
}