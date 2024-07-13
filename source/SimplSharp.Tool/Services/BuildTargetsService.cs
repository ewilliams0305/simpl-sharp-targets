using Microsoft.Extensions.Logging;
using System.Xml;

namespace SimplSharp.Tool;

internal sealed class BuildTargetsService
{
    private readonly ILogger<BuildTargetsService> _logger;
    private readonly ProjectService _projectService;

    public BuildTargetsService(ILogger<BuildTargetsService> logger, ProjectService projectService)
    {
        _logger = logger;
        _projectService = projectService;
    }

    /// <summary>
    /// Creates or appends the existing Directory.Build.targets file
    /// </summary>
    /// <param name="directory"></param>
    /// <returns></returns>
    public BuildTargetsResult CreateOrModifyDirectoryTargets(DirectoryInfo directory)
    {
        try
        {
            if (!directory.Exists)
            {
                return BuildTargetsResult.DirectoryNotFound;
            }

            var filePath = Path.Combine(directory.FullName, Global.DirectoryTargetsFile);

            XmlDocument targetsXmlDocument;

            if (File.Exists(filePath))
            {
                targetsXmlDocument = new XmlDocument();
                targetsXmlDocument.Load(filePath);

                var (modifiedXml, wasModified) = ModifyBuildTargetsXmlDocument(targetsXmlDocument);
                modifiedXml.Save(filePath);

                _logger.LogInformation("Modified existing {targets} file in the {directory}", Global.DirectoryTargetsFile, directory);
                return wasModified ? BuildTargetsResult.Modified : BuildTargetsResult.Ignored;
            }

            targetsXmlDocument = CreateBuildTargetsXmlDocument();
            targetsXmlDocument.Save(filePath);

            _logger.LogInformation("Created new {targets} file in the {directory}", Global.DirectoryTargetsFile, directory);
            return BuildTargetsResult.Created;
        }
        catch (IOException e)
        {
            _logger.LogError(e, "Failed system IO option {directory}", directory);
            throw;
        }
        catch (XmlException e)
        {
            _logger.LogError(e, "Failed parsing or modifying the XML file {directory}", directory);
            throw;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "General exception processing {directory}", directory);
            throw;
        }
    }

    private XmlDocument CreateBuildTargetsXmlDocument()
    {
        var xmlDocument = new XmlDocument();
        var root = xmlDocument.CreateElement("Project", "http://schemas.microsoft.com/developer/msbuild/2003");
        root.SetAttribute("ToolsVersion", "4.0");
        root.SetAttribute("DefaultTargets", "build");

        xmlDocument.AppendChild(root);

        var simplSharpCleanElement = xmlDocument.CreateElement(Global.XmlTarget);
        simplSharpCleanElement.SetAttribute(Global.XmlName, Global.SimplSharpCleanTarget);

        var simplSharpProcessElement = xmlDocument.CreateElement(Global.XmlTarget);
        simplSharpProcessElement.SetAttribute(Global.XmlName, Global.SimplSharpCleanTarget);

        var simplSharpProcessFrameworkElement = xmlDocument.CreateElement(Global.XmlTarget);
        simplSharpProcessFrameworkElement.SetAttribute(Global.XmlName, Global.SimplSharpCleanTarget);

        root.AppendChild(simplSharpCleanElement);
        root.AppendChild(simplSharpProcessElement);
        root.AppendChild(simplSharpProcessFrameworkElement);
        return xmlDocument;
    }
    
    private (XmlDocument doc, bool modified) ModifyBuildTargetsXmlDocument(XmlDocument xmlDocument)
    {
        var root = xmlDocument.DocumentElement;

        if (root is null)
        {
            throw new NullReferenceException("The XML Document has no valid root element.");
        }

        var nsmgr = new XmlNamespaceManager(xmlDocument.NameTable);
        nsmgr.AddNamespace("msb", "http://schemas.microsoft.com/developer/msbuild/2003");

        var modified = (
            AppendCleanElement(xmlDocument, nsmgr, root) ||
            AppendProcessElement(xmlDocument, nsmgr, root) ||
            AppendProcess47Element(xmlDocument, nsmgr, root));

        return (xmlDocument, modified);
    }

    private static bool AppendProcess47Element(XmlDocument xmlDocument, XmlNamespaceManager nsmgr, XmlElement root)
    {
        var process47Node = xmlDocument.SelectSingleNode(Global.SimplSharpProcess47Query, nsmgr);

        if (process47Node is not null)
        {
            return false;
        }
        var simplSharpProcessFrameworkElement = xmlDocument.CreateElement(Global.XmlTarget);
        simplSharpProcessFrameworkElement.SetAttribute(Global.XmlName, Global.SimplSharpCleanTarget);

        root.AppendChild(simplSharpProcessFrameworkElement);
        return true;
    }

    private static bool AppendProcessElement(XmlDocument xmlDocument, XmlNamespaceManager nsmgr, XmlElement root)
    {
        var processNode = xmlDocument.SelectSingleNode(Global.SimplSharpProcessQuery, nsmgr);

        if (processNode is not null)
        {
            return false;
        }
        var simplSharpProcessElement = xmlDocument.CreateElement(Global.XmlTarget);
        simplSharpProcessElement.SetAttribute(Global.XmlName, Global.SimplSharpCleanTarget);

        root.AppendChild(simplSharpProcessElement);
        return true;
    }

    private static bool AppendCleanElement(XmlDocument xmlDocument, XmlNamespaceManager nsmgr, XmlElement root)
    {
        var cleanNode = xmlDocument.SelectSingleNode(Global.SimplSharpCleanQuery, nsmgr);

        if (cleanNode is not null)
        {
            return false;
        }
        var simplSharpCleanElement = xmlDocument.CreateElement(Global.XmlTarget);
        simplSharpCleanElement.SetAttribute(Global.XmlName, Global.SimplSharpCleanTarget);
        root.AppendChild(simplSharpCleanElement);
        return true;
    }

    /// <summary>
    /// The results of the directory build target functions.
    /// </summary>
    internal enum BuildTargetsResult
    {
        /// <summary>
        /// A new file was created
        /// </summary>
        Created,
        /// <summary>
        /// An existing file was modified.
        /// </summary>
        Modified,
        /// <summary>
        /// The file already contains the required targets
        /// </summary>
        Ignored,
        /// <summary>
        /// The path specified does not exist
        /// </summary>
        DirectoryNotFound,
    }
}

