using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using Shouldly;
using WebApi.Models;
using Xunit;

namespace WebApi.Integration.Tests
{
    public class LessonValidationTests_Assert: IClassFixture<TestFixture>
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUri;
        
        public LessonValidationTests_Assert(TestFixture testFixture)
        {
            _httpClient = new HttpClient();
            var configuration = testFixture.Configuration;
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
            
            //Assert using Assert
            Assert.False(response.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            var responseMessage = await response.Content.ReadAsStringAsync();
            Assert.Equal(Errors.CourseId_должен_быть_больше_нуля, responseMessage);

            //Assert using Shoudly
            /*
            response.IsSuccessStatusCode.ShouldBeFalse();
            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
            var responseMessage = await response.Content.ReadAsStringAsync();
            responseMessage.ShouldBe(Errors.CourseId_должен_быть_больше_нуля);
            */
            
            //Assert using FluentAssertions
            /*
            response.IsSuccessStatusCode.Should().BeFalse();
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var responseMessage = await response.Content.ReadAsStringAsync();
            responseMessage.Should().Be(Errors.CourseId_должен_быть_больше_нуля);
            */
        }
    }
}