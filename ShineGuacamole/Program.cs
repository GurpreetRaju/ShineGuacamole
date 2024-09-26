using Serilog;
using ShineGuacamole.Components;
using ShineGuacamole.Options;
using ShineGuacamole.Services.Interfaces;
using ShineGuacamole.Services;
using ShineGuacamole.DataAccess.SqlServer;
using MudBlazor.Services;
using ShineGuacamole.Shared;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(10);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Add serilog services to the container and read config from appsettings
builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

builder.Services.AddMudServices();

builder.Services.Configure<ClientOptions>(builder.Configuration.GetSection(ClientOptions.Name));
builder.Services.Configure<GuacdOptions>(builder.Configuration.GetSection(GuacdOptions.Name));

builder.Services.AddScoped<IAppBarContentProvider, AppBarContentProvider>();

builder.Services.AddSingleton<RemoteConnectionService>();
builder.Services.AddScoped<IConnectionManagerService, ConnectionManagerService>();

//configure SQL Server data access.
builder.Services.AddSqlServerDataAccess(builder.Configuration.GetConnectionString("SqlServerConnection"));

builder.Services.AddHostedService(provider => provider.GetService<RemoteConnectionService>());

builder.Services.AddMicrosoftIdentityWebAppAuthentication(builder.Configuration, "AzureAd");

builder.Services.AddControllersWithViews()
                .AddMicrosoftIdentityUI();

builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = options.DefaultPolicy;
});

builder.Services.AddCascadingAuthenticationState();

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor()
                .AddMicrosoftIdentityConsentHandler();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(ShineGuacamole.Client._Imports).Assembly);

app.Run();
