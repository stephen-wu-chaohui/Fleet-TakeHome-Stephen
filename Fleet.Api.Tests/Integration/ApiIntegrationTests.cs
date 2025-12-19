using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;

namespace Fleet.Api.Tests.Integration
{
    public class ApiIntegrationTests : IClassFixture<TestWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public ApiIntegrationTests(TestWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetCars_ReturnsNonEmptyList()
        {
            var cars = await _client.GetFromJsonAsync<List<object>>("/api/cars");
            Assert.NotNull(cars);
            Assert.NotEmpty(cars);
        }

        [Fact]
        public async Task GetCars_withMake_ReturnsNonEmptyList() {
            await Task.Delay(TimeSpan.FromSeconds(3), CancellationToken.None);
            var make = "Toyota";
            var cars = await _client.GetFromJsonAsync<List<object>>($"/api/cars?make={make}");
            Assert.NotNull(cars);
            Assert.NotEmpty(cars);
        }

        [Fact] 
        public async Task GetCars_withUnknownMake_ReturnsNonEmptyList() {
            await Task.Delay(TimeSpan.FromSeconds(3), CancellationToken.None);
            var make = "[]";
            var cars = await _client.GetFromJsonAsync<List<object>>($"/api/cars?make={make}");
            Assert.NotNull(cars);
            Assert.Empty(cars);
        }

        [Fact]
        public async Task GetRegistration_Endpoint_ReturnsSuccess()
        {
            var res = await _client.GetAsync("/api/registration");
            Assert.True(res.IsSuccessStatusCode);
        }
    }
}
