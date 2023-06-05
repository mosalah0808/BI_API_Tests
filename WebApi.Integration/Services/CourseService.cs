using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using WebApi.Models;

namespace WebApi.Integration.Services;

public class CourseService
{
    private CourseControllerClient _applicationHttpClient;
    public CourseService()
    {
        _applicationHttpClient = new CourseControllerClient();
    }
    
   
    public async Task<int> CreateRandomCourseAsync()
    {
        var autoFixture = new Fixture();

        #region FSetup

        autoFixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => autoFixture.Behaviors.Remove(b));
        autoFixture.Behaviors.Add(new OmitOnRecursionBehavior());

        #endregion

        var courseModel = autoFixture.Create<CourseModel>();
        return await _applicationHttpClient.CreateCourseAsync(courseModel);
    }
}