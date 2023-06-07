using Castle.Core.Internal;
using Demo.Authentication.Dto;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using WebApi.Integration.Services;
using WebApi.Models;
using Xunit;

namespace WebApi.Integration.Tests
{
    public class CookiesAuthTests
    {
        private readonly CookieService _cookieService;


        public CookiesAuthTests()
        {
            _cookieService = new CookieService();
            
        }
        
        [Fact]
        public async Task IfLoginAndPasswordIsCorrectCookieShouldBeRecieved()
        {
            //Arrange
            string name = "admin";
            string password = "admin";

            //Act
            var httpResponseMessage = await _cookieService.GetCookieInternalAsync(name, password);
            
            //Assert
            Assert.Equal(HttpStatusCode.OK, httpResponseMessage.StatusCode);
           
            var cookieDto = httpResponseMessage.Headers.FirstOrDefault(h => h.Key == "Set-Cookie").Value.ToList().First();
            Assert.NotNull(cookieDto);
            
        }

        [Fact]
        public async Task IfPasswordIsInCorrectCookieShouldNotBeRecieved()
        {
            //Arrange
            string name = "admin";
            string password = Guid.NewGuid().ToString();    

            //Act
            var httpResponseMessage = await _cookieService.GetCookieInternalAsync(name, password);

            //Assert
            Assert.Equal(HttpStatusCode.OK, httpResponseMessage.StatusCode);
            Assert.False(httpResponseMessage.Headers.Contains("Set-Cookie"));

        }
    }
}