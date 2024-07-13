using System.Xml;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace SimplSharp.Tool.Tests.Setup;

public class BuildTargetsFixture : IAsyncLifetime
{
    private IServiceProvider? _serviceProvider;
    private BuildTargetsService? _buildTargetsService;

    /// <inheritdoc />
    public Task InitializeAsync()
    {
        var services = new ServiceCollection()
            .AddTransient<BuildTargetsService>()
            .AddTransient<ProjectService>()
            .AddTransient<ManifestService>()
            .AddTransient<ArchiveService>()
            .AddLogging();

        _serviceProvider = services.BuildServiceProvider();
        _buildTargetsService = _serviceProvider.GetRequiredService<BuildTargetsService>();

        return Task.CompletedTask;
    }

    public BuildTargetsService.BuildTargetsResult CreateOrModifyBuildTarget(DirectoryInfo directory)
    {
        return _buildTargetsService!.CreateOrModifyDirectoryTargets(directory);
    }

    public XmlDocument CreateBuildTarget()
    {
        return _buildTargetsService!.CreateBuildTargetsXmlDocument();
    }

    public (XmlDocument, bool) ModifyBuildTarget(XmlDocument doc)
    {
        return _buildTargetsService!.ModifyBuildTargetsXmlDocument(doc);
    }

    /// <inheritdoc />
    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }
}