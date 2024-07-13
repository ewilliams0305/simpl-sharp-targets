using Microsoft.Extensions.Logging;
using System.Xml;

namespace SimplSharp.Tool;

public sealed class BuildTargetsService
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

    internal XmlDocument CreateBuildTargetsXmlDocument()
    {
        var xmlDocument = new XmlDocument();
        var root = xmlDocument.CreateElement("Project");
        root.SetAttribute("xmlns", "http://schemas.microsoft.com/developer/msbuild/2003");
        root.SetAttribute("ToolsVersion", "4.0");
        root.SetAttribute("DefaultTargets", "build");

        xmlDocument.AppendChild(root);

        var simplSharpCleanElement = xmlDocument.CreateElement(Global.XmlTarget);
        simplSharpCleanElement.SetAttribute(Global.XmlName, Global.SimplSharpCleanTarget);

        var simplSharpProcessFrameworkElement = xmlDocument.CreateElement(Global.XmlTarget);
        simplSharpProcessFrameworkElement.SetAttribute(Global.XmlName, Global.SimplSharpProcess47Target);

        var simplSharpProcessElement = xmlDocument.CreateElement(Global.XmlTarget);
        simplSharpProcessElement.SetAttribute(Global.XmlName, Global.SimplSharpProcessTarget);

        root.AppendChild(simplSharpCleanElement);
        root.AppendChild(simplSharpProcessElement);
        root.AppendChild(simplSharpProcessFrameworkElement);
        return xmlDocument;
    }

    internal (XmlDocument doc, bool modified) ModifyBuildTargetsXmlDocument(XmlDocument xmlDocument)
    {
        var root = xmlDocument.DocumentElement;

        if (root is null)
        {
            throw new NullReferenceException("The XML Document has no valid root element.");
        }

        var modified = new bool[]
        {
            AppendCleanElement(xmlDocument, root),
            AppendProcessElement(xmlDocument, root), 
            AppendProcess47Element(xmlDocument, root)
        };

        return (xmlDocument, modified.Any(m => m));
    }

    private static bool AppendCleanElement(XmlDocument xmlDocument, XmlElement root)
    {
        var targets = root.GetElementsByTagName(Global.XmlTarget, Global.XmlNameSpace);
        var targetExists = targets.Cast<XmlElement>().Any(target => target.GetAttribute(Global.XmlName) == Global.SimplSharpCleanTarget);

        if (targetExists)
        {
            return false;
        }
        var simplSharpCleanElement = xmlDocument.CreateElement(Global.XmlTarget, Global.XmlNameSpace);
        simplSharpCleanElement.SetAttribute(Global.XmlName, Global.SimplSharpCleanTarget);
        root.AppendChild(simplSharpCleanElement);
        return true;
    }

    private static bool AppendProcess47Element(XmlDocument xmlDocument, XmlElement root)
    {
        var targets = root.GetElementsByTagName(Global.XmlTarget, Global.XmlNameSpace);
        var targetExists = targets.Cast<XmlElement>().Any(target => target.GetAttribute(Global.XmlName) == Global.SimplSharpProcess47Target);

        if (targetExists)
        {
            return false;
        }
        var simplSharpProcessFrameworkElement = xmlDocument.CreateElement(Global.XmlTarget, Global.XmlNameSpace);
        simplSharpProcessFrameworkElement.SetAttribute(Global.XmlName, Global.SimplSharpProcess47Target);

        root.AppendChild(simplSharpProcessFrameworkElement);
        return true;
    }

    private static bool AppendProcessElement(XmlDocument xmlDocument, XmlElement root)
    {
        var targets = root.GetElementsByTagName(Global.XmlTarget, Global.XmlNameSpace);
        var targetExists = targets.Cast<XmlElement>().Any(target => target.GetAttribute(Global.XmlName) == Global.SimplSharpProcessTarget);

        if (targetExists)
        {
            return false;
        }
        var simplSharpProcessElement = xmlDocument.CreateElement(Global.XmlTarget, Global.XmlNameSpace);
        simplSharpProcessElement.SetAttribute(Global.XmlName, Global.SimplSharpProcessTarget);

        root.AppendChild(simplSharpProcessElement);
        return true;
    }



    /// <summary>
    /// The results of the directory build target functions.
    /// </summary>
    public enum BuildTargetsResult
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

