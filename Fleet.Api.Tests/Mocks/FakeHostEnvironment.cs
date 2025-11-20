using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;

namespace Fleet.Api.Tests.Mocks
{
    public class FakeHostEnvironment : IWebHostEnvironment
    {
        public string ApplicationName { get; set; } = "Fleet.Api.Tests";
        public string EnvironmentName { get; set; } = "Development";
        public string WebRootPath { get; set; } = Directory.GetCurrentDirectory();
        public string ContentRootPath { get; set; } = Directory.GetCurrentDirectory();
        public IFileProvider WebRootFileProvider { get; set; } = null!;
        public IFileProvider ContentRootFileProvider { get; set; } = null!;
    }
}
