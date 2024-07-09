
using Cocona;
using Microsoft.Extensions.DependencyInjection;
using SimplSharp.Tool;

var builder = CoconaApp.CreateBuilder();

builder.Services
    .AddTransient<ManifestService>()
    .AddTransient<ArchiveService>();

var app = builder.Build();

app.AddCommands<ClzCommand>();

app.Run();