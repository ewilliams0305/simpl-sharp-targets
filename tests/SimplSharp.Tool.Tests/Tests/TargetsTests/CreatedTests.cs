using FluentAssertions;
using SimplSharp.Tool.Tests.Setup;

namespace SimplSharp.Tool.Tests.TargetsTests;


[Collection(nameof(BuildTargetsServiceCollection))]
public class CreatedTests
{
    private readonly BuildTargetsFixture _fixture;

    public CreatedTests(BuildTargetsFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public void CreateTarget_CreatesValidXmlDoc()
    {
        var validTarget = @"<Project xmlns=""http://schemas.microsoft.com/developer/msbuild/2003"" ToolsVersion=""4.0"" DefaultTargets=""build""><Target Name=""SimplSharpClean"" /><Target Name=""SimplSharpPostProcess"" /><Target Name=""SimplSharpPostProcess47"" /></Project>";

        var xml = _fixture.CreateBuildTarget();

        var stringValue = xml.InnerXml;

        xml.InnerXml.Should().BeEquivalentTo(validTarget, because: "The default XML should only contain SimplSharp overrides");
    }
}