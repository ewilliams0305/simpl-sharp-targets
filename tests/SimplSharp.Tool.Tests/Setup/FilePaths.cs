using System.Runtime.InteropServices;

namespace SimplSharp.Tool.Tests.Setup;
internal class FilePaths
{

    static FilePaths()
    {
        Directory.SetCurrentDirectory("../../../../../");
        var newDir = Directory.GetCurrentDirectory();
        Console.WriteLine($"-------------> Changing working directory: {newDir}");

        if (!File.Exists(ExePath))
        {
            throw new FileNotFoundException(ExePath);
        }

        if (!File.Exists(TargetLibraryPath))
        {
            throw new FileNotFoundException(TargetLibraryPath);
        }

        if (!File.Exists(TargetLibraryProject))
        {
            throw new FileNotFoundException(TargetLibraryProject);
        }
        
        if (!File.Exists(UserPlusWrapperPath))
        {
            throw new FileNotFoundException(UserPlusWrapperPath);
        }
    }


#if DEBUG
    private static string Configuration => "Debug";
#else
    private static string Configuration => "Release";
#endif

    public static string ExePath => Path.Combine(Directory.GetCurrentDirectory(), "source", "SimplSharp.Tool", "bin", Configuration, "net8.0", RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ? "SimplSharp.Tool" : "SimplSharp.Tool.exe");
    
    public static string TargetLibraryPath => Path.Combine(Directory.GetCurrentDirectory(), "tests", "SimplSharp.Library", "bin", Configuration, "net472", "SimplSharp.Library.dll");
    public static string TargetLibraryProject => Path.Combine(Directory.GetCurrentDirectory(), "tests", "SimplSharp.Library", "SimplSharp.Library.csproj");    
    public static string TargetProgramPath => Path.Combine(Directory.GetCurrentDirectory(), "tests", "SimplSharp.Program", "bin", Configuration, "net472", "SimplSharp.Program.dll");
    public static string TargetProgramProject => Path.Combine(Directory.GetCurrentDirectory(), "tests", "SimplSharp.Program", "SimplSharp.Program.csproj");
    public static string TargetArchivePath => Path.Combine(Directory.GetCurrentDirectory(), "tests", "SimplSharp.Library", "bin", Configuration, "net472", "SimplSharp.Library.clz");
    public static string TargetProgramNet6ArchivePath => Path.Combine(Directory.GetCurrentDirectory(), "tests", "SimplSharp.Program", "bin", Configuration, "net6", "SimplSharp.Program.cpz");
    public static string TargetProgramNet472ArchivePath => Path.Combine(Directory.GetCurrentDirectory(), "tests", "SimplSharp.Program", "bin", Configuration, "net472", "SimplSharp.Program.cpz");
    
    public static string WrapperLibraryPath => Path.Combine(Directory.GetCurrentDirectory(), "example", "SimplSharp.Targets.Clz", "SimplSharp.Targets.Clz.csproj");
    public static string WrapperClzPath => Path.Combine(Directory.GetCurrentDirectory(), "example", "SimplSharp.Targets.Clz", "bin", Configuration, "net472", "SimplSharp.Targets.Clz.clz");
    
    public static string ManifestPath => Path.Combine(Directory.GetCurrentDirectory(), "tests", "SimplSharp.Library", "bin", Configuration, "net472", "ProgramInfo.config");
    
    public static string UserPlusWrapperPath => Path.Combine(Directory.GetCurrentDirectory(), "example", "SimplSharpWrapper.usp");
    public static string UserPlusCompiledPath => Path.Combine(Directory.GetCurrentDirectory(), "example", "SimplSharpWrapper.ush");
    
    public static string SmwProgramPath => Path.Combine(Directory.GetCurrentDirectory(), "example", "Wrapper.smw");
    public static string LpzProgramPath => Path.Combine(Directory.GetCurrentDirectory(), "example", "Wrapper.lpz");
    public static string SplsWorkPath => Path.Combine(Directory.GetCurrentDirectory(), "example", "SPlsWork");

    public static void DeleteExistingClz()
    {
        if (File.Exists(TargetArchivePath))
        {
            File.Delete(TargetArchivePath);
        }
    }

    public static void DeleteExistingNet6Cpz()
    {
        if (File.Exists(TargetProgramNet6ArchivePath))
        {
            File.Delete(TargetProgramNet6ArchivePath);
        }
    }

    public static void DeleteExistingNet472Cpz()
    {
        if (File.Exists(TargetProgramNet472ArchivePath))
        {
            File.Delete(TargetProgramNet472ArchivePath);
        }
    }

    public static void DeleteExistingManifest()
    {
        if (File.Exists(ManifestPath))
        {
            File.Delete(ManifestPath);
        }
    }
    public static void DeleteExistingLpz()
    {
        if (File.Exists(LpzProgramPath))
        {
            File.Delete(LpzProgramPath);
        }
    }
    public static void DeleteExistingUsh()
    {
        if (File.Exists(UserPlusCompiledPath))
        {
            File.Delete(UserPlusCompiledPath);
        }
    }
    public static void DeleteWrapperClz()
    {
        if (File.Exists(WrapperClzPath))
        {
            File.Delete(WrapperClzPath);
        }
    }

    public static void DeleteExistingSpls()
    {
        if (Directory.Exists(SplsWorkPath))
        {
            Directory.Delete(SplsWorkPath, true);
        }
    }

}
