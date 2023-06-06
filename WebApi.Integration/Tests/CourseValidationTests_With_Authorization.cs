using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using WebApi.Integration.Services;
using WebApi.Models;
using Xunit;

namespace WebApi.Integration.Tests
{
    public class CourseValidationTests_With_Authorization: IClassFixture<TestFixture>
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUri;
        private readonly string _cookie;

        public CourseValidationTests_With_Authorization(TestFixture testFixture)
        {
            _httpClient = new HttpClient();
            var configuration = testFixture.Configuration;
            _baseUri = configuration["BaseUri"];
            _cookie = testFixture.AuthCookie;
        }
        
        [Fact]
        public async Task IfPriceIsZero_PostCourseShouldReturnError()
        {
            //Arrange 
            var courseModel = new CourseModel
            {
                Name = Guid.NewGuid().ToString(),
                Price = 0
            };

            //Act
            _httpClient.DefaultRequestHeaders.Add("cookie", _cookie);
            var response = await _httpClient.PostAsJsonAsync($"{_baseUri}/course", courseModel);

            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            var responseMessage = await response.Content.ReadAsStringAsync();
            Assert.Equal(Errors.Поле_Price_должно_быть_больше_нуля, responseMessage);
        }
    }
}