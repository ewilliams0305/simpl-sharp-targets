using System.Xml.Linq;
using FluentAssertions;
using SimplSharp.Tool.Tests.Setup;
using File = System.IO.File;

namespace SimplSharp.Tool.Tests.ClzTests;

[Collection(nameof(ClzTestCollection))]
public class ManifestTests(ExecutableTestFixture fixture)
{
    [Fact]
    public void ClzCommand_CreatesManifest_WhenTargetIsValid()
    {
        FilePaths.DeleteExistingManifest();

        fixture.CreateClz();

        var exists = File.Exists(FilePaths.ManifestPath);

        exists.Should().BeTrue(because: "The assembly path provided is valid");
    }

    [Fact]
    public void ClzCommand_CreateManifest_WithValidXmlRootElement()
    {
        FilePaths.DeleteExistingManifest();

        fixture.CreateClz();

        var xdoc = XDocument.Load(FilePaths.ManifestPath);
        var root = xdoc.Element("ProgramInfo");

        root.Should().NotBeNull(because: "All manifest files should contain a program info root element");
    }    
    
    [Fact]
    public void ClzCommand_Manifest_Contains_RequiredElement()
    {
        FilePaths.DeleteExistingManifest();

        fixture.CreateClz();

        var xdoc = XDocument.Load(FilePaths.ManifestPath);
        var root = xdoc.Element("ProgramInfo");
        var required = root!.Element("RequiredInfo");

        required.Should().NotBeNull();
    }
    

    [Fact]
    public void ClzCommand_Manifest_Contains_OptionalElement()
    {
        FilePaths.DeleteExistingManifest();

        fixture.CreateClz();

        var xdoc = XDocument.Load(FilePaths.ManifestPath);
        var root = xdoc.Element("ProgramInfo");
        var options = root!.Element("OptionalInfo");

        options.Should().NotBeNull();
    }

    [Fact]
    public void ClzCommand_Manifest_Contains_PluginElement()
    {
        FilePaths.DeleteExistingManifest();

        fixture.CreateClz();

        var xdoc = XDocument.Load(FilePaths.ManifestPath);
        var root = xdoc.Element("ProgramInfo");
        var plugin = root!.Element("Plugin");

        plugin.Should().NotBeNull();
    }

    [Fact]
    public void ClzCommand_Manifest_Contains_PluginVersion()
    {
        FilePaths.DeleteExistingManifest();

        fixture.CreateClz();

        var xdoc = XDocument.Load(FilePaths.ManifestPath);
        var root = xdoc.Element("ProgramInfo");
        var plugin = root!.Element("Plugin");
        var version = plugin!.Element("Include4.dat")!.Value;

        version.Should().Be(fixture.ReferencedSdkVersion);
    }

}