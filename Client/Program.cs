using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using Serilog.Core;
using Serilog;
using ShineGuacamole.Client;
using ShineGuacamole.Client.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddHttpClient("ShineGuacamole.ServerAPI", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));

Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration).CreateLogger();
builder.Services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(Log.Logger, dispose: true));

// Supply HttpClient instances that include access tokens when making requests to the server project
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("ShineGuacamole.ServerAPI"));
builder.Services.AddScoped<ConnectionService>();

builder.Services.AddMudServices();

await builder.Build().RunAsync();
