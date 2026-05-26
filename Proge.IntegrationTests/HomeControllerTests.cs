using System.Threading.Tasks;
using Proge.IntegrationTests.Helpers;
using Xunit;

namespace Proge.IntegrationTests
{
    [Collection("Sequential")]
    public class HomeControllerTests : TestBase
    {
        [Theory]
        [InlineData("/")]
        [InlineData("/Home/Privacy")]
        public async Task Get_endpoints_return_success(string url)
        {
            var client = Factory.CreateClient();

            var response = await client.GetAsync(url);

            response.EnsureSuccessStatusCode();
            Assert.Equal("text/html; charset=utf-8", response.Content.Headers.ContentType.ToString());
        }
    }
}