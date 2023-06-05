using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using WebApi.Models;
using Xunit;

namespace WebApi.Integration.Tests
{
    public class LessonValidationTests_With_Authorization: IClassFixture<TestFixture>
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUri;
        private readonly string _token;

        public LessonValidationTests_With_Authorization(TestFixture testFixture)
        {
            _httpClient = new HttpClient();
            var configuration = testFixture.Configuration;
            _baseUri = configuration["BaseUri"];
            _token = testFixture.Token;
        }
        
        [Fact]
        public async Task IfCourseIdIsZero_PostLessonShouldReturnError()
        {
            //Arrange 
            var lessonModel = new LessonModel
            {
                CourseId = 0,
                Subject = Guid.NewGuid().ToString()
            };

            //Act
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_token}");
            var response = await _httpClient.PostAsJsonAsync($"{_baseUri}/lesson", lessonModel);
            
            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            var responseMessage = await response.Content.ReadAsStringAsync();
            Assert.Equal(Errors.CourseId_должен_быть_больше_нуля, responseMessage);
        }
    }
}