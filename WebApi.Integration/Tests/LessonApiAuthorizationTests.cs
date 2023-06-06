using System;
using System.Net;
using System.Threading.Tasks;
using WebApi.Integration.Services;
using Xunit;

namespace WebApi.Integration.Tests
{
    public class LessonApiAuthorizationTests: IClassFixture<TestFixture>
    {
        private readonly TokenService _tokenService;

        public LessonApiAuthorizationTests(TestFixture testFixture)
        {
            _tokenService = new TokenService();
        }
        
        [Fact]
        public async Task IfInitialParametersAreSetCorrectly_PostLessonShouldCreateLessonSuccessfully()
        {
            //Arrange
            var name = Guid.NewGuid().ToString();
            var password = Guid.NewGuid().ToString();

            //Act
            var addLessonResponse = await _tokenService.GetTokenInternalAsync(name, password);
            
            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, addLessonResponse.StatusCode);
            string message = await addLessonResponse.Content.ReadAsStringAsync();
            Assert.Equal("Некорректные логин/пароль", message);
        }
    }
}