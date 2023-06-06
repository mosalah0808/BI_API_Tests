using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using WebApi.Integration.Services;
using Xunit;

namespace WebApi.Integration
{
    public class TestFixture : IAsyncLifetime
    {
        public IConfigurationRoot Configuration { get; set; }

        public string Token { get; set; }
        public string AuthCookie { get; set; }
        
        /// <summary>
        /// Выполняется перед запуском тестов
        /// </summary>
        public TestFixture()
        {
            Configuration = new ConfigurationBuilder()
                .AddJsonFile($"appsettings.json").Build();
        }
        
        public async Task InitializeAsync()
        {
            Token = await new TokenService().GetAdminTokenAsync();
            AuthCookie = await new CookieService().GetAdminCookieAsync();
        }

        public Task DisposeAsync()
        {
            return Task.CompletedTask;
        }
    }
}
