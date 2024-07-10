<img src="./target.png" alt="drawing" width="100"/>

![Static Badge](https://img.shields.io/badge/SIMPL-green)
![Static Badge](https://img.shields.io/badge/SHARP-blue)
![Static Badge](https://img.shields.io/badge/TARGETS-red)

# SIMPL Sharp Targets

[![DOTNET TEST](https://github.com/ewilliams0305/simpl-sharp-targets/actions/workflows/dotnet-test.yml/badge.svg)](https://github.com/ewilliams0305/simpl-sharp-targets/actions/workflows/dotnet-test.yml)
[![PUBLISH RELEASE](https://github.com/ewilliams0305/simpl-sharp-targets/actions/workflows/dotnet-release.yml/badge.svg)](https://github.com/ewilliams0305/simpl-sharp-targets/actions/workflows/dotnet-release.yml)

The SimplSharp Targets repository contains build tools used to generate CPZs CLZs and CPLZs.  Packaged as a source controlled dotnet tool, the simpl sharp targets tool offers an alternative to Crestron's MSBUILD targets.

Build targets to override the default SIMPL Sharp SDK Targets and offer support for cross platform development.

## Table of Contents
1. [Why](#Why)
2. [Dotnet Tool](#Dotnet-Tool)
3. [Target Package](#Target-Package)
4. [Usage](#Usage)
5. [Examples](#Examples)

# Why 
![Static Badge](https://img.shields.io/badge/WHY-green)

So why would we need this?  For automated CICD pipelines and new project templates.  The default crestron msbuild targets use aniquated libraries and won't execute properly on github runners and other dotnet core build tools.

*Plus its actually kindof nice to have a command line tool that creates CLZs*

# Dotnet Tool
![Static Badge](https://img.shields.io/badge/TOOL-green)

Located in the `source/SimplSharp.Tool` directory you will find a dotnet 8 console application that provides required commands used to generate `CLZ`, `CPZ`, and `CPLZ` *As of the first release only CLZs are supported* 
This application is packaged as a dotnet tool and published to `nuget.org`

### Install
Install the tool with the dotnet CLI
```
PS C:\dev> dotnet tool install --local SimplSharp.Tool --version <VERSIONNUMBER>
```

### Execute
Run the tool with the dotnet simplsharp command
```
PS C:\dev> dotnet simplsharp --help
Usage: dotnet [command]

SimplSharp.Tool

Commands:
  clz    Creates a CLZ from the target assembly DLL referencing the SimplSharp SDK

Options:
  -h, --help    Show help message
  --version     Show version
```

### CLZ Command
Execute the clz command to create a new simpl sharp library archive.  Target an assembly that includes a reference to the `Crestron.SimplSharp.SDK.Library` nuget package.  The CLZ command will fail to create the project configuration file if the SIMPL Sharp SDK is not referenced.

The destination switch is optional while the path is required.  The path should target a compiled DLL library.  The destination is optional and can create the new archive a specific location.

```
PS C:\dev> dotnet simplsharp clz --help
Usage: dotnet clz [--path <path>] [--destination <destination>] [--help]

Creates a CLZ from the target assembly DLL referencing the SimplSharp SDK

Options:
  -p, --path <path>                  File path for the compiled assembly that will be converted to a SimplSharp CLZ (Required)
  -d, --destination <destination>    Newly created CPZ destination
  -h, --help                         Show help message
```


# Target Package
![Static Badge](https://img.shields.io/badge/TARGETS-blue)

Located in the `source/SimplSharp.Targets` directory you will find a project containing nothing more than build files.  This project is packaged as nuget to nuget.org.  
When included as a project reference the SimplSharp.Targets build pipeline will attempt to install the dotnet tool, and execute the proper command line actions to generate the Simpl Sharp archives.

# Usage
![Static Badge](https://img.shields.io/badge/USAGE-yellow)

*comming soon*

# Examples
![Static Badge](https://img.shields.io/badge/EXAMPLES-red)

### Create a CLZ

The example below converts the compiled assembly `SimplSharp.Library.dll` to an archived `SimplSharp.Library.clz` library for the SIMPL+ compiler.  By default the new CLZ will be created in the `..\SimplSharp.Library\bin\Debug\net472\` directory.
```
dotnet simplsharp clz --path ..\SimplSharp.Library\bin\Debug\net472\SimplSharp.Library.dll
```

Optionally provide a target destination and the CLZ will be archived to the provided location.
```
dotnet simplsharp clz --path ..\SimplSharp.Library\bin\Debug\net472\SimplSharp.Library.dll -d "C:/Temp/Libraries"
```