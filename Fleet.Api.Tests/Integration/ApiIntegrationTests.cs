using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Fleet.Api.Tests.Integration
{
    public class ApiIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public ApiIntegrationTests(WebApplicationFactory<Program> factory)
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
            var make = "Honda";
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
