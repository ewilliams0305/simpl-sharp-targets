using System.Xml;

namespace SimplSharp.Tool;

public sealed class ProjectService
{

    /// <summary>
    /// Queries a csproj file for the Crestron Package references
    /// </summary>
    /// <param name="csprojFilePath">Full path for the CSProj</param>
    /// <returns>The version of the SDK otherwise missing null</returns>
    public Version? QueryProjectForSimplSharpLibraryVersion(string csprojFilePath)
    {
        if (!File.Exists(csprojFilePath))
        {
            return null;
        }

        var doc = new XmlDocument();
        doc.Load(csprojFilePath);
        var root = doc.DocumentElement;

        if (root is null)
        {
            return null;
        }

        var itemGroups = root.GetElementsByTagName("ItemGroup");

        foreach (XmlElement itemGroup in itemGroups)
        {
            var packageReferences = itemGroup.GetElementsByTagName("PackageReference");

            foreach (XmlElement packageReference in packageReferences)
            {
                if (packageReference.GetAttribute("Include") != "Crestron.SimplSharp.SDK.Library")
                {
                    continue;
                }

                var version = packageReference.GetAttribute("Version");
                return Version.Parse(version);
            }
        }

        return null;
    }

    /// <summary>
    /// Queries a csproj file for the Crestron Package references
    /// </summary>
    /// <param name="csprojFilePath">Full path for the CSProj</param>
    /// <returns>The version of the SDK otherwise missing null</returns>
    public Version? QueryProjectForSimplSharpProgramVersion(string csprojFilePath)
    {
        if (!File.Exists(csprojFilePath))
        {
            return null;
        }

        var doc = new XmlDocument();
        doc.Load(csprojFilePath);
        var root = doc.DocumentElement;

        if (root is null)
        {
            return null;
        }

        var itemGroups = root.GetElementsByTagName("ItemGroup");

        foreach (XmlElement itemGroup in itemGroups)
        {
            var packageReferences = itemGroup.GetElementsByTagName("PackageReference");

            foreach (XmlElement packageReference in packageReferences)
            {
                if (packageReference.GetAttribute("Include") != "Crestron.SimplSharp.SDK.Program")
                {
                    continue;
                }

                var version = packageReference.GetAttribute("Version");
                return Version.Parse(version);
            }
        }

        return null;
    }
}