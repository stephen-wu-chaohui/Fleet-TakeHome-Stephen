using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;

namespace Fleet.Api.Tests.Fakes;

public class FakeHostEnvironment(string rootPath) : IWebHostEnvironment {
    public string ApplicationName { get; set; } = "Fleet.Api";
    public string ContentRootPath { get; set; } = rootPath;
    public string EnvironmentName { get; set; } = "Development";
    public string WebRootPath { get; set; } = "";
    public IFileProvider WebRootFileProvider { get; set; } = null!;
    public IFileProvider ContentRootFileProvider { get; set; } = null!;
}
