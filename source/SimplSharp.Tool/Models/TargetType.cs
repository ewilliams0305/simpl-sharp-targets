namespace SimplSharp.Tool;

/// <summary>
/// The type of simpl sharp library archive being targeted
/// </summary>
internal enum TargetType
{
    /// <summary>
    /// SimplSharp Library
    /// </summary>
    Library,
    /// <summary>
    /// SimplSharp Program
    /// </summary>
    Program,
    /// <summary>
    /// SimplSharp Program Library (I don't see why we would EVER need this....)
    /// </summary>
    ProgramLibrary
}