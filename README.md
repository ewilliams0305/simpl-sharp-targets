﻿<img src="./target.png" alt="drawing" width="100"/>

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
Execute the clz command to create a new simpl sharp library archive.  Target a project that includes a reference to the `Crestron.SimplSharp.SDK.Library` nuget package.  The CLZ command will fail to create the project configuration file if the SIMPL Sharp SDK is not referenced.
The destination switch is optional while the path is required.  The path should target a csproj.  The destination is optional and can create the new archive a specific location.

```
PS C:\dev> dotnet simplsharp clz --help
Usage: dotnet clz [--path <path>] [--destination <destination>] [--help]

Creates a CLZ from the target assembly DLL referencing the SimplSharp SDK

Options:
  -p, --path <path>                  File path for the csproj file that will generate a SimplSharpPro CLZ (Required)
  -t, --target <target>              Target framework to compile (Default: net472)
  -a, --assembly <assembly>          Output assembly full path, if nothing is provided this argument will default to the bin/
  -c, --profile <profile>            The target projects build profile (Default: Release)
  -d, --destination <destination>    Newly created CLZ destination
  -h, --help                         Show help message
```

### CPZ Command
Execute the cpz command to create a new simpl sharp program archive.  Target an assembly that includes a reference to the `Crestron.SimplSharp.SDK.Program` nuget package.  The CPZ command will fail to create the project configuration file if the SIMPL Sharp SDK is not referenced.
The destination switch is optional while the path is required.  The path should target a csproj.  The destination is optional and can create the new archive a specific location.

```
PS C:\dev> dotnet simplsharp clz --help
Usage: dotnet cpz [--path <path>] [--destination <destination>] [--help]

Creates a CPZ from the target assembly DLL referencing the SimplSharpPro SDK

Options:
  -p, --path <path>                  File path for the csproj file that will generate a SimplSharpPro CPZ (Required)
  -t, --target <target>              Target framework to compile (Required)
  -a, --assembly <assembly>          Output assembly full path, if nothing is provided this argument will default to the bin/
  -c, --profile <profile>            The target projects build profile (Default: Release)
  -d, --destination <destination>    Newly created CPZ destination
  -h, --help                         Show help message
```

# Target Package
![Static Badge](https://img.shields.io/badge/TARGETS-blue)

Located in the `source/SimplSharp.Targets` directory you will find a project containing nothing more than build files.  This project is packaged as nuget to nuget.org.  
When included as a project reference the SimplSharp.Targets build pipeline will attempt to install the dotnet tool, and execute the proper command line actions to generate the Simpl Sharp archives.

# Usage
![Static Badge](https://img.shields.io/badge/USAGE-yellow)

### Library Project Setup

Include a reference to the required `SimplSharp.Library.Targets` or `SimplSharp.Program.Targets` depending on your use case. 
*Note Do not include a reference to both packages, only one is required*

A pre-build target has will be added to your project.  This target will install the required version of the `dotnet tool` and attempt to generate a Directory.Build.targets file.
This file will only be created if an existing file doesn't exist.  This target file will override the default SimplSharp targets.  If you have an exsting Directory.Build.targets
the tool will update it with the Simplsharp overrides.

### Compile Project
Once configured a pre-build step will install the `SimplSharp.Tool`.  Once installed the projects build output will be directed through the tools command line and a CLZ will be generated using the tool.
*Note the version of the tool used will always match the version of the SimplSharp.Library.Targets nuget package used*

# Examples
![Static Badge](https://img.shields.io/badge/EXAMPLES-red)

### Create a CLZ

The example below converts the `SimplSharp.Library.csproj` to an archived `SimplSharp.Library.clz` library for the SIMPL+ compiler.  By default the new CLZ will be created in the `..\SimplSharp.Library\bin\Debug\net472\` directory.
```
dotnet simplsharp clz --path ..\SimplSharp.Library\SimplSharp.Library.csproj
```

Optionally provide a target destination and the CLZ will be archived to the provided location.
```
dotnet simplsharp clz --path ..\SimplSharp.Library\SimplSharp.Library.csproj -d "C:/Temp/Libraries"
```

### Create a CPZ

The example below converts the `SimplSharp.Program.csproj` to an archived `SimplSharp.Program.clz` program.  
By default the new CLZ will be created in the `..\SimplSharp.Program\bin\Debug\net472\` directory.
*note be sure to provide the target framework and configuration*
```
dotnet simplsharp cpz --path ..\SimplSharp.Program\SimplSharp.Program.csproj -t net6 -c Debug
```

Optionally provide a target destination and the CLZ will be archived to the provided location.
```
dotnet simplsharp cpz --path ..\SimplSharp.Program\SimplSharp.Program.csproj -d "C:/Temp/Libraries" -t net6 -c Debug
```

*Note neither command actually builds the project so be sure to dotnet build first, if the nuget packages are included in your project the targets will execute these commands for you*