using FluentAssertions;
using SimplSharp.Tool.Tests.Setup;
using System.Xml.Linq;
using File = System.IO.File;

namespace SimplSharp.Tool.Tests;

[Collection(nameof(ClzTestCollection))]
public class ManifestTests
{
    private readonly ClzContainerFixture _fixture;

    public ManifestTests(ClzContainerFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public void ClzCommand_CreatesManifest_WhenTargetIsValid()
    {
        _fixture.DeleteExistingManifest();

        var (output, exitCode) = _fixture.CreateClz();

        var exists = File.Exists(ClzContainerFixture.ManifestPath);

        exists.Should().BeTrue(because: "The assembly path provided is valid");
    }

    [Fact]
    public void ClzCommand_CreateManifest_WithValidXmlRootElement()
    {
        _fixture.DeleteExistingManifest();

        var (output, exitCode) = _fixture.CreateClz();

        var xdoc = XDocument.Load(ClzContainerFixture.ManifestPath);
        var root = xdoc.Element("ProgramInfo");

        root.Should().NotBeNull(because: "All manifest files should contain a program info root element");
    }    
    
    [Fact]
    public void ClzCommand_Manifest_Contains_RequiredElement()
    {
        _fixture.DeleteExistingManifest();

        var (output, exitCode) = _fixture.CreateClz();

        var xdoc = XDocument.Load(ClzContainerFixture.ManifestPath);
        var root = xdoc.Element("ProgramInfo");
        var required = root!.Element("RequiredInfo");

        required.Should().NotBeNull();
    }
    

    [Fact]
    public void ClzCommand_Manifest_Contains_OptionalElement()
    {
        _fixture.DeleteExistingManifest();

        var (output, exitCode) = _fixture.CreateClz();

        var xdoc = XDocument.Load(ClzContainerFixture.ManifestPath);
        var root = xdoc.Element("ProgramInfo");
        var options = root!.Element("OptionalInfo");

        options.Should().NotBeNull();
    }

    [Fact]
    public void ClzCommand_Manifest_Contains_PluginElement()
    {
        _fixture.DeleteExistingManifest();

        var (output, exitCode) = _fixture.CreateClz();

        var xdoc = XDocument.Load(ClzContainerFixture.ManifestPath);
        var root = xdoc.Element("ProgramInfo");
        var plugin = root!.Element("Plugin");

        plugin.Should().NotBeNull();
    }

    [Fact]
    public void ClzCommand_Manifest_Contains_PluginVersion()
    {
        _fixture.DeleteExistingManifest();

        var (output, exitCode) = _fixture.CreateClz();

        var xdoc = XDocument.Load(ClzContainerFixture.ManifestPath);
        var root = xdoc.Element("ProgramInfo");
        var plugin = root!.Element("Plugin");
        var version = plugin!.Element("Include4.dat")!.Value;

        version.Should().Be(_fixture.ReferencedSdkVersion);
    }

}