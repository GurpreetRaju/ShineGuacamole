using ShineGuacamole.Services;
using ShineGuacamole.Options;
using Serilog;
using ShineGuacamole.Services.Interfaces;

var builder = WebApplication.CreateBuilder();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(10);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Host.UseSerilog();

builder.Services.Configure<GuacamoleSharpOptions>(builder.Configuration.GetSection(GuacamoleSharpOptions.Name));
builder.Services.Configure<ClientOptions>(builder.Configuration.GetSection(ClientOptions.Name));
builder.Services.Configure<GuacdOptions>(builder.Configuration.GetSection(GuacdOptions.Name));

builder.Services.AddSingleton<RemoteConnectionService>();
builder.Services.AddScoped<IConnectionManagerService, ConnectionManagerService>();

builder.Services.AddHostedService<RemoteConnectionService>(provider => provider.GetService<RemoteConnectionService>());


var app = builder.Build();

app.UseWebSockets();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseSession();

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
