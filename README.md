<img src="./crestron-logo.png" alt="drawing" width="100"/>

# SIMPL Sharp Targets
![Static Badge](https://img.shields.io/badge/CRESTRON-green)
![Static Badge](https://img.shields.io/badge/SIMPL__SHARP-blue)
![Static Badge](https://img.shields.io/badge/MSBUILD-red)
[![DOTNET TEST](https://github.com/ewilliams0305/simpl-sharp-targets/actions/workflows/dotnet-test.yml/badge.svg)](https://github.com/ewilliams0305/simpl-sharp-targets/actions/workflows/dotnet-test.yml)

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

# Target Package
![Static Badge](https://img.shields.io/badge/TARGETS-blue)

Located in the `source/SimplSharp.Targets` directory you will find a project containing nothing more than build files.  This project is packaged as nuget to nuget.org.  
When included as a project reference the SimplSharp.Targets build pipeline will attempt to install the dotnet tool, and execute the proper command line actions to generate the Simpl Sharp archives.

# Usage
![Static Badge](https://img.shields.io/badge/USAGE-yellow)

*comming soon*

# Examples
![Static Badge](https://img.shields.io/badge/EXAMPLES-red)

*comming soon*