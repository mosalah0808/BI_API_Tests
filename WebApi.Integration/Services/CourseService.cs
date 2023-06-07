using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AutoFixture;
using Newtonsoft.Json;
using WebApi.Models;

namespace WebApi.Integration.Services;

public class CourseService
{
    private CourseApiClient _applicationHttpClient;
    public CourseService()
    {
        _applicationHttpClient = new CourseApiClient();
    }
    
   
    public async Task<int> CreateRandomCourseAsync(string cookie = null) 
    {
        var autoFixture = new Fixture();

        #region FSetup

        autoFixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => autoFixture.Behaviors.Remove(b));
        autoFixture.Behaviors.Add(new OmitOnRecursionBehavior());

        #endregion

        var courseModel = autoFixture.Create<AddCourseModel>();
        return await AddCourseAsync(courseModel, cookie);
    }
    
    public async Task<int> AddCourseAsync(AddCourseModel courseModel, string cookie = null)
    {
        var addCourseResponse = await AddCourseInternalAsync(courseModel, cookie);
        return JsonConvert.DeserializeObject<int>(await addCourseResponse.Content.ReadAsStringAsync());
    }
    
    public async Task<HttpResponseMessage> AddCourseInternalAsync(AddCourseModel courseModel, string cookie = null)
    {
        return await _applicationHttpClient.CreateCourseAsync(courseModel, cookie);
    }

    public async Task<HttpResponseMessage> GetCourseInternalAsync(int id, string cookie = null)
    {
        return await _applicationHttpClient.GetCourseAsync(id, cookie);
    }

    public async Task<AddCourseModel> GetCourseAsync(int id, string cookie = null)
    {
        var response = await GetCourseInternalAsync(id, cookie);
        return JsonConvert.DeserializeObject<AddCourseModel>(await response.Content.ReadAsStringAsync());
    }

    public async Task<DelCourseModel> GetCourseAsyncDel(int id, string cookie = null)
    {
        var response = await GetCourseInternalAsync(id, cookie);
        return JsonConvert.DeserializeObject<DelCourseModel>(await response.Content.ReadAsStringAsync());
    }

    public async Task<HttpResponseMessage> EditCourseInternalAsync(int id, AddCourseModel course, string cookie = null)
    {
        return await _applicationHttpClient.EditCourseAsync(id, course, cookie);
    }

    public async Task<HttpResponseMessage> DeleteCourseInternalAsync(int id, string cookie = null)
    {
        return await _applicationHttpClient.DeleteCourseAsync(id, cookie);
    }
}