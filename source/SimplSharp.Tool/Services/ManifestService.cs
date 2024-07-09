#pragma warning disable IDE0290
#pragma warning disable CA1822

using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Xml;

namespace SimplSharp.Tool;

/// <summary>
/// Collection of methods used to generate the XML manifest files.
/// </summary>
internal sealed class ManifestService
{
    private readonly ILogger<ManifestService> _logger;

    public ManifestService(ILogger<ManifestService> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Creates a CLZ manifest
    /// </summary>
    /// <param name="assembly">The SimplSharp assembly targeted by the build pipeline</param>
    /// <param name="targetPath">Ask Crestron...</param>
    /// <param name="targetName"></param>
    /// <param name="manifest">The generated manifest XML document.</param>
    /// <returns>True when the manifest is created, and false when the manifest is skipped.</returns>
    public bool CreateClzManifestXml(Assembly assembly, string targetName, string targetPath, out XmlDocument? manifest)
    {
        var references = assembly.GetReferencedAssemblies().ToList();

        try
        {
            if (references.Exists(a => a.FullName == "SimplSharpPro.exe") || references.Exists(a => a.FullName == "SimplSharpPro.dll"))
            {
                manifest = null;
                return false;
            }

            var name = targetName.Replace(Global.LibraryExtension, "");

            var directory = new FileInfo(targetPath).Directory!;
            var sdkVersion = FileVersionInfo.GetVersionInfo(Path.Combine(directory.FullName, "SimplSharpProgrammingInterfaces.dll")).ProductVersion;
      
            var version = assembly.GetName().Version!.ToString();

            var xmlDoc = new XmlDocument();
            var root = xmlDoc.CreateElement("ProgramInfo");
            xmlDoc.AppendChild(root);

            var required = xmlDoc.CreateElement("RequiredInfo");
            root.AppendChild(required);

            var node = xmlDoc.CreateElement("FriendlyName");
            node.AppendChild(xmlDoc.CreateTextNode(name.Length > 20 ? name[..20] : name));
            required.AppendChild(node);

            node = xmlDoc.CreateElement("SystemName");
            node.AppendChild(xmlDoc.CreateTextNode(name));
            required.AppendChild(node);

            node = xmlDoc.CreateElement("EntryPoint");
            node.AppendChild(xmlDoc.CreateTextNode(name));
            required.AppendChild(node);

            node = xmlDoc.CreateElement("DesignToolId");
            node.AppendChild(xmlDoc.CreateTextNode("6"));
            required.AppendChild(node);

            node = xmlDoc.CreateElement("ProgramToolId");
            node.AppendChild(xmlDoc.CreateTextNode("6"));
            required.AppendChild(node);

            var fileInfo = new FileInfo(targetPath);

            var optional = xmlDoc.CreateElement("OptionalInfo");
            root.AppendChild(optional);

            node = xmlDoc.CreateElement("CompiledOn");
            node.AppendChild(xmlDoc.CreateTextNode(fileInfo.LastWriteTime.ToString("yyyy-MM-dd'T'HH:mm:ss.fffzzz", DateTimeFormatInfo.InvariantInfo)));
            optional.AppendChild(node);

            node = xmlDoc.CreateElement("CompilerRev");
            node.AppendChild(xmlDoc.CreateTextNode(version));
            optional.AppendChild(node);

            var plugin = xmlDoc.CreateElement("Plugin");
            root.AppendChild(plugin);

            node = xmlDoc.CreateElement("Include4.dat");
            node.AppendChild(xmlDoc.CreateTextNode(sdkVersion));
            plugin.AppendChild(node);

            manifest = xmlDoc;
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to generate the manifest file from the provided assembly {assembly}", assembly);
            manifest = null;
            return false;
        }
    }

    /// <summary>
    /// Creates a file path for the program configuration file added to the archive
    /// </summary>
    /// <param name="targetPath"></param>
    /// <returns></returns>
    /// <exception cref="DirectoryNotFoundException"></exception>
    public string CreateManifestFileName(string targetPath)
    {
        var fileInfo = new FileInfo(targetPath);
        var directory = fileInfo.DirectoryName;

        if (!Directory.Exists(directory))
        {
            throw new DirectoryNotFoundException(directory);
        }

        return Path.Combine(directory, "ProgramInfo.config");
    }
}
