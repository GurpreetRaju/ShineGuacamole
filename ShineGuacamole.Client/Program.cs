using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using Serilog;
using ShineGuacamole.Client;
using ShineGuacamole.Shared;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddHttpClient("ShineGuacamole.ServerAPI", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));

Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration).CreateLogger();
builder.Services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(Log.Logger, dispose: true));

// Supply HttpClient instances that include access tokens when making requests to the server project
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("ShineGuacamole.ServerAPI"));
builder.Services.AddMudServices();
builder.Services.AddScoped<IAppBarContentProvider, AppBarContentProvider>();
builder.Services.AddAuthorizationCore(); 
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<AuthenticationStateProvider, PersistentAuthenticationStateProvider>();

await builder.Build().RunAsync();
