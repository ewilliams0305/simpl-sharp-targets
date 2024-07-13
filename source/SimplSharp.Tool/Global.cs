namespace SimplSharp.Tool;

/// <summary>
/// Constant variables used.
/// </summary>
internal sealed class Global
{
    /// <summary>
    /// The extension used for a Simpl Sharp Library
    /// </summary>
    public const string LibraryExtension = ".clz";

    /// <summary>
    /// The extension used for a Simpl Sharp Program
    /// </summary>
    public const string ProgramExtension = ".cpz";

    /// <summary>
    /// The extension used for a Simpl Sharp Program Library
    /// </summary>
    public const string ProgramLibraryExtension = ".cplz";

    /// <summary>
    /// The directory build targets file name
    /// </summary>
    public const string DirectoryTargetsFile = "Directory.Build.targets";

    /// <summary>
    /// The MSBuild XML Target
    /// </summary>
    public const string XmlTarget = "Target";
    
    /// <summary>
    /// The Targets Name
    /// </summary>
    public const string XmlName = "Name";

    /// <summary>
    /// The target name to override
    /// </summary>
    public const string SimplSharpCleanTarget = "SimplSharpClean";

    /// <summary>
    /// The target name to override
    /// </summary>
    public const string SimplSharpProcessTarget = "SimplSharpPostProcess";

    /// <summary>
    /// The target name to override
    /// </summary>
    public const string SimplSharpProcess47Target = "SimplSharpPostProcess47";


    public const string SimplSharpCleanQuery = "//msb:Target[@Name='SimplSharpClean']";

    public const string SimplSharpProcessQuery = "//msb:Target[@Name='SimplSharpPostProcess']";

    public const string SimplSharpProcess47Query = "//msb:Target[@Name='SimplSharpPostProcess47']";
}
