using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using DataAccess.Entities;
using FluentAssertions;
using Newtonsoft.Json;
using Shouldly;
using WebApi.Integration.Services;
using WebApi.Models;
using Xunit;

namespace WebApi.Integration.Tests
{
    public class LessonValidationTests_Scenario: IClassFixture<TestFixture>
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUri;
        
        public LessonValidationTests_Scenario(TestFixture testFixture)
        {
            _httpClient = new HttpClient();
            var configuration = testFixture.Configuration;
            _baseUri = configuration["BaseUri"];
        }
        
        [Fact]
        public async Task IfInitialParametersAreSetCorrectly_PostLessonShouldCreateLessonSuccessfully_1()
        {
            //Arrange
            var lessonModel = new LessonModel 
            {
                CourseId = 1,
                Subject = Guid.NewGuid().ToString()
            };

            //Act
            var response = await _httpClient.PostAsJsonAsync($"{_baseUri}/lesson", lessonModel);
            
            //Assert using Assert
            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
        
        [Fact]
        public async Task IfInitialParametersAreSetCorrectly_PostLessonShouldCreateLessonSuccessfully_2()
        {
            //Arrange
            var initialCourseModel = new CourseModel
            {
                Name = "course_name",
                Price = (new Random()).Next(int.MaxValue)
            };
            var addCourseResponse = await _httpClient.PostAsJsonAsync($"{_baseUri}/course", initialCourseModel);
            var courseId = JsonConvert.DeserializeObject<int>(await addCourseResponse.Content.ReadAsStringAsync());
            var lessonModel = new LessonModel 
            {
                CourseId = courseId,
                Subject = Guid.NewGuid().ToString()
            };

            //Act
            var response = await _httpClient.PostAsJsonAsync($"{_baseUri}/lesson", lessonModel);
            
            //Assert using Assert
            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
        
        [Fact]
        public async Task IfInitialParametersAreSetCorrectly_PostLessonShouldCreateLessonSuccessfully_3()
        {
            //Arrange
            var initialCourseModel = new CourseModel
            {
                Name = "course_name",
                Price = (new Random()).Next(int.MaxValue)
            };
            var addCourseResponse = await _httpClient.PostAsJsonAsync($"{_baseUri}/course", initialCourseModel);
            var courseId = JsonConvert.DeserializeObject<int>(await addCourseResponse.Content.ReadAsStringAsync());
            var lessonModel = new LessonModel 
            {
                CourseId = courseId,
                Subject = Guid.NewGuid().ToString()
            };

            //Act
            var addLessonResponse = await _httpClient.PostAsJsonAsync($"{_baseUri}/lesson", lessonModel);
            
            //Assert using Assert
            Assert.Equal(HttpStatusCode.OK, addLessonResponse.StatusCode);
            var responseMessage = int.Parse(await addLessonResponse.Content.ReadAsStringAsync());
            var response = await _httpClient.GetAsync($"{_baseUri}/lesson/{responseMessage}");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var lesson = JsonConvert.DeserializeObject<Lesson>(await response.Content.ReadAsStringAsync());
            Assert.Equal(lessonModel.CourseId, lesson.CourseId);
            Assert.Equal(lessonModel.Subject, lesson.Subject);
        }
        
        [Fact]
        public async Task IfInitialParametersAreSetCorrectly_PostLessonShouldCreateLessonSuccessfully_4()
        {
            //Arrange
            var courseId = await new CourseService().CreateRandomCourseAsync();
            var lessonModel = new LessonModel
            {
                CourseId = courseId,
                Subject = Guid.NewGuid().ToString()
            };

            //Act
            var addLessonResponse = await _httpClient.PostAsJsonAsync($"{_baseUri}/lesson", lessonModel);
            
            //Assert using Assert
            Assert.Equal(HttpStatusCode.OK, addLessonResponse.StatusCode);
            var lessonId = int.Parse(await addLessonResponse.Content.ReadAsStringAsync());
            var lesson = await new LessonService().GetLessonAsync(lessonId);
            Assert.Equal(lessonModel.CourseId, lesson.CourseId);
            Assert.Equal(lessonModel.Subject, lesson.Subject);
        }

        [Fact]
        public async Task IfInitialParametersAreSetCorrectly_PostLessonShouldCreateLessonSuccessfully_5()
        {
            //TODO: вынести сервисы в IOC контейнер
        }
    }
}