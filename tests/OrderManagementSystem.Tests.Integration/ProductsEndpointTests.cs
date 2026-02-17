using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace OrderManagementSystem.Tests.Integration
{
    public class ProductsEndpointTests : IClassFixture<ApiFactory>
    {
        private readonly HttpClient _client;

        public ProductsEndpointTests(ApiFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Get_Products_ShouldReturnList()
        {
            var resp = await _client.GetAsync("/api/products");

            Assert.Equal(HttpStatusCode.OK, resp.StatusCode);

            var data = await resp.Content.ReadFromJsonAsync<List<ProductDto>>();
            Assert.NotNull(data);
            Assert.True(data!.Count >= 2);
        }

        private class ProductDto
        {
            public int Id { get; set; }
            public string Name { get; set; } = null!;
            public decimal Price { get; set; }
            public int Stock { get; set; }
        }
    }
}
