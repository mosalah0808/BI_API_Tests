using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using WebApi.Models;
using Xunit;

namespace WebApi.Integration.Tests
{
    public class LessonControllerTests
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUri;
        
        public LessonControllerTests()
        {
            _httpClient = new HttpClient();
            var configuration = new ConfigurationBuilder()
                .AddJsonFile($"appsettings.json").Build();
            _baseUri = configuration["BaseUri"];
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
            var response = await _httpClient.PostAsJsonAsync($"{_baseUri}/lesson", lessonModel);
            
            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            var responseMessage = await response.Content.ReadAsStringAsync();
            Assert.Equal(Errors.CourseId_должен_быть_больше_нуля, responseMessage);
        }
        
        [Fact]
        public async Task IfSubjectIsNull_PostLessonShouldReturnError()
        {
            //Arrange 
            var lessonModel = new LessonModel
            {
                CourseId = 1,
                Subject = null
            };

            //Act
            var response = await _httpClient.PostAsJsonAsync($"{_baseUri}/lesson", lessonModel);
            
            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            var responseMessage = await response.Content.ReadAsStringAsync();
            Assert.Equal(Errors.Поле_Subject_не_должно_быть_пустым, responseMessage);
        }
    }
}