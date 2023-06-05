using System.Threading.Tasks;
using WebApi.Models;

namespace WebApi.Integration.Services;

public class LessonService
{
    private LessonControllerClient _applicationHttpClient;
    public LessonService()
    {
        _applicationHttpClient = new LessonControllerClient();
    }

    public async Task<LessonModel> GetLessonAsync(int id)
    {
        return await _applicationHttpClient.GetLessonAsync(id);
    }
}