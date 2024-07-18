
using Cocona;
using Microsoft.Extensions.DependencyInjection;
using SimplSharp.Tool;

var builder = CoconaApp.CreateBuilder();

builder.Services
    .AddTransient<BuildTargetsService>()
    .AddTransient<ProjectService>()
    .AddTransient<ManifestService>()
    .AddTransient<ArchiveService>();

var app = builder.Build();

app.AddCommands<ClzCommand>();
app.AddCommands<CpzCommand>();
app.AddCommands<ProjectQueryCommand>();
app.AddCommands<DirectoryBuildTargetsCommand>();

app.Run();