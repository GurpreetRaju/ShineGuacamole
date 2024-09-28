using ShineGuacamole.Options;
using ShineGuacamole.Services.Interfaces;
using ShineGuacamole.Services;
using MudBlazor.Services;
using ShineGuacamole.DataAccess.SqlServer;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using Autofac.Extensions.DependencyInjection;
using Autofac;

namespace ShineGuacamole
{
    /// <summary>
    /// The startup configuration.
    /// </summary>
    public static class StartupConfiguration
    {
        /// <summary>
        /// Add required services.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/>.</param>
        /// <param name="configuration">The <see cref="ConfigurationManager"/>.</param>
        public static void AddServices(this IServiceCollection services, ConfigurationManager configuration)
        {
            services.Configure<ClientOptions>(configuration.GetSection(ClientOptions.Name));
            services.Configure<GuacdOptions>(configuration.GetSection(GuacdOptions.Name));

            services.AddMudServices();

            //configure SQL Server data access.
            services.AddSqlServerDataAccess(configuration.GetConnectionString("SqlServerConnection"));

            services.AddMicrosoftIdentityWebAppAuthentication(configuration, "AzureAd");

            services.AddControllersWithViews()
                            .AddMicrosoftIdentityUI();

            services.AddAuthorization(options =>
            {
                options.FallbackPolicy = options.DefaultPolicy;
            });

            services.AddCascadingAuthenticationState();
        }

        /// <summary>
        /// Add container services.
        /// </summary>
        /// <param name="host">The <see cref="ConfigureHostBuilder"/>.</param>
        /// <param name="configuration">The <see cref="ConfigurationManager"/>.</param>
        public static void AddContainerServices(this ConfigureHostBuilder host, ConfigurationManager configuration)
        {
            host.UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureContainer<ContainerBuilder>(b =>
                {
                    b.RegisterType<AppBarContentProvider>()
                        .As<IAppBarContentProvider>()
                        .InstancePerLifetimeScope();

                    b.RegisterType<RemoteConnectionService>()
                        .AsSelf()
                        .As<IHostedService>()
                        .SingleInstance();

                    b.RegisterType<ConnectionManagerService>()
                        .As<IConnectionManagerService>()
                        .InstancePerLifetimeScope();

                    b.RegisterType<UserPreferences>()
                        .AsSelf()
                        .InstancePerLifetimeScope();
                });
        }
    }
}
