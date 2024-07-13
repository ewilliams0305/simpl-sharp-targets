using System.Xml;
using FluentAssertions;
using SimplSharp.Tool.Tests.Setup;

namespace SimplSharp.Tool.Tests.TargetsTests;

[Collection(nameof(BuildTargetsServiceCollection))]
public class ModifiedTests
{
    private readonly BuildTargetsFixture _fixture;

    public ModifiedTests(BuildTargetsFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public void ModifyTarget_AddsClearTarget_WhenClearTarget_DoesNotExist()
    {
        
        var xmlDocument = new XmlDocument();
        var root = xmlDocument.CreateElement("Project");
        root.SetAttribute("xmlns", "http://schemas.microsoft.com/developer/msbuild/2003");
        root.SetAttribute("ToolsVersion", "4.0");
        root.SetAttribute("DefaultTargets", "build");

        xmlDocument.AppendChild(root);

        var (xml, _) = _fixture.ModifyBuildTarget(xmlDocument);

        xml.InnerXml.Should().Contain(Global.SimplSharpCleanTarget, because: "The xml didn't contain the element and now it does");
    }
    
    [Fact]
    public void ModifyTarget_ModifiesTarget_WhenClearTarget_DoesNotExist()
    {
        
        var xmlDocument = new XmlDocument();
        var root = xmlDocument.CreateElement("Project");
        root.SetAttribute("xmlns", "http://schemas.microsoft.com/developer/msbuild/2003");
        root.SetAttribute("ToolsVersion", "4.0");
        root.SetAttribute("DefaultTargets", "build");

        xmlDocument.AppendChild(root);

        var (_, modified) = _fixture.ModifyBuildTarget(xmlDocument);

        modified.Should().BeTrue(because: "The xml didn't contain the element and now it does");
    }
    
    [Fact]
    public void ModifyTarget_AddsProcessTarget_WhenProcessTarget_DoesNotExist()
    {
        
        var xmlDocument = new XmlDocument();
        var root = xmlDocument.CreateElement("Project");
        root.SetAttribute("xmlns", "http://schemas.microsoft.com/developer/msbuild/2003");
        root.SetAttribute("ToolsVersion", "4.0");
        root.SetAttribute("DefaultTargets", "build");

        xmlDocument.AppendChild(root);

        var (xml, _) = _fixture.ModifyBuildTarget(xmlDocument);

        xml.InnerXml.Should().Contain(Global.SimplSharpProcessTarget, because: "The xml didn't contain the element and now it does");
    }
    
    [Fact]
    public void ModifyTarget_ModifiesTarget_WhenProcessTarget_DoesNotExist()
    {
        
        var xmlDocument = new XmlDocument();
        var root = xmlDocument.CreateElement("Project");
        root.SetAttribute("xmlns", "http://schemas.microsoft.com/developer/msbuild/2003");
        root.SetAttribute("ToolsVersion", "4.0");
        root.SetAttribute("DefaultTargets", "build");

        xmlDocument.AppendChild(root);

        var (_, modified) = _fixture.ModifyBuildTarget(xmlDocument);

        modified.Should().BeTrue(because: "The xml didn't contain the element and now it does");
    }
    
    
    [Fact]
    public void ModifyTarget_AddsProcess47Target_WhenProcess47Target_DoesNotExist()
    {
        
        var xmlDocument = new XmlDocument();
        var root = xmlDocument.CreateElement("Project");
        root.SetAttribute("xmlns", "http://schemas.microsoft.com/developer/msbuild/2003");
        root.SetAttribute("ToolsVersion", "4.0");
        root.SetAttribute("DefaultTargets", "build");

        xmlDocument.AppendChild(root);

        var (xml, _) = _fixture.ModifyBuildTarget(xmlDocument);

        xml.InnerXml.Should().Contain(Global.SimplSharpProcess47Target, because: "The xml didn't contain the element and now it does");
    }
    
    [Fact]
    public void ModifyTarget_ModifiesTarget_WhenProcess47Target_DoesNotExist()
    {
        
        var xmlDocument = new XmlDocument();
        var root = xmlDocument.CreateElement("Project");
        root.SetAttribute("xmlns", "http://schemas.microsoft.com/developer/msbuild/2003");
        root.SetAttribute("ToolsVersion", "4.0");
        root.SetAttribute("DefaultTargets", "build");

        xmlDocument.AppendChild(root);

        var (_, modified) = _fixture.ModifyBuildTarget(xmlDocument);

        modified.Should().BeTrue(because: "The xml didn't contain the element and now it does");
    }
    
    [Fact]
    public void ModifyTarget_IgnoreDocument_WhenTargetsExist()
    {
        var doc = _fixture.CreateBuildTarget();

        var (_, modified) = _fixture.ModifyBuildTarget(doc);

        modified.Should().BeFalse(because: "The xml didn't contain the element and now it does");
    }
    
}