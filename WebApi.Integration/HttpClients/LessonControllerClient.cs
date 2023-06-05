using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using WebApi.Models;

namespace WebApi.Integration.Services;

public class LessonControllerClient
{
    private HttpClient _httpClient;
    private readonly string _baseUri;

    public LessonControllerClient()
    {
        _httpClient = new HttpClient();
        var configuration = new ConfigurationBuilder()
            .AddJsonFile($"appsettings.json").Build();
        _baseUri = configuration["BaseUri"];
    }
    
    public async Task<LessonModel> GetLessonAsync(int id)
    {
        var response = await _httpClient.GetAsync($"{_baseUri}/lesson/{id}");
        return JsonConvert.DeserializeObject<LessonModel>(await response.Content.ReadAsStringAsync());
    }
}