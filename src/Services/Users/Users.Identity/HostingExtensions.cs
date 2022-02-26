using Consul;
using Duende.IdentityServer;
using Duende.IdentityServer.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Text;
using System.Text.Json;
using Users.Identity.Data;
using Users.Identity.Models;

namespace Users.Identity
{
    internal static class HostingExtensions
    {
        public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
        {
            var settings = GetServiceSettings(builder.Configuration);

            if (settings.IdentityUrl == null || settings.IdentitySettings.PsqlConnection == null)
                throw new InvalidOperationException("IdentitySettings is null");

            builder.Services.AddRazorPages();
            builder.Services
                .AddCustomDbContext(settings)
                .AddCustomIdentity(settings)
                .AddCustomCors();

            builder.Services.AddTransient<IProfileService, ProfileService>();
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("role");

            return builder.Build();
        }

        public static WebApplication ConfigurePipeline(this WebApplication app)
        {
            app.UseSerilogRequestLogging();

            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseCors("CorsPolicy");
            app.UseRouting();
            app.UseIdentityServer();
            app.UseAuthorization();

            app.MapRazorPages()
                .RequireAuthorization();

            return app;
        }


        private static ServiceSettings GetServiceSettings(IConfiguration configuration)
        {
            // check consul
            var consulCfg = configuration.GetValue<bool>("ConsulCfg");
            ServiceSettings settings = new();
            if (consulCfg == true)
            {
                var consulAddress = configuration["ServiceSettings:Consul"];
                var client = new ConsulClient(cfg => cfg.Address = new Uri(consulAddress));
                var res = client.KV.Get("SampleEshop").GetAwaiter().GetResult();

                if (res.Response == null || res.StatusCode == HttpStatusCode.NotFound)
                    throw new InvalidOperationException("Consul config is null");

                var strResponse = Encoding.UTF8.GetString(res.Response.Value);
                Console.WriteLine(strResponse);

                settings = JsonSerializer.Deserialize<ServiceSettings>(strResponse);
            }
            else
            {
                settings = configuration.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>();
            }

            return settings;
        }

        private static IServiceCollection AddCustomCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder
                    .SetIsOriginAllowed((host) => true)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });
            return services;
        }

        private static IServiceCollection AddCustomDbContext(this IServiceCollection services, ServiceSettings settings)
        {
            services.AddDbContext<ApplicationDbContext>(opt =>
            {
                opt.UseNpgsql(settings.IdentitySettings.PsqlConnection, dbOpts => dbOpts.MigrationsAssembly(typeof(Program).Assembly.FullName));
            });

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            return services;
        }
        private static IServiceCollection AddCustomIdentity(this IServiceCollection services, ServiceSettings settings)
        {


            services
                .AddIdentityServer(options =>
                {
                    options.IssuerUri = settings.IdentityUrl;

                    options.Events.RaiseErrorEvents = true;
                    options.Events.RaiseInformationEvents = true;
                    options.Events.RaiseFailureEvents = true;
                    options.Events.RaiseSuccessEvents = true;
                    options.EmitStaticAudienceClaim = true;
                })
                //.AddInMemoryIdentityResources(Config.IdentityResources)
                //.AddInMemoryApiScopes(Config.ApiScopes)
                //.AddInMemoryClients(Config.Clients)
                .AddAspNetIdentity<ApplicationUser>()
                .AddConfigurationStore(options =>
                {
                    options.DefaultSchema = "IdentityConfiguration";
                    options.ConfigureDbContext = b =>
                        b.UseNpgsql(settings.IdentitySettings.PsqlConnection, dbOpts => dbOpts.MigrationsAssembly(typeof(Program).Assembly.FullName));
                })
                .AddOperationalStore(options =>
                {
                    options.DefaultSchema = "IdentityOperational";
                    options.ConfigureDbContext = b =>
                        b.UseNpgsql(settings.IdentitySettings.PsqlConnection, dbOpts => dbOpts.MigrationsAssembly(typeof(Program).Assembly.FullName));

                    options.EnableTokenCleanup = true;
                    options.RemoveConsumedTokens = true;
                });

            services.AddAuthentication()
                .AddGoogle(options =>
                {
                    options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;

                    // register your IdentityServer with Google at https://console.developers.google.com
                    // enable the Google+ API
                    // set the redirect URI to https://localhost:5001/signin-google
                    options.ClientId = "copy client ID from Google here";
                    options.ClientSecret = "copy client secret from Google here";
                });

            return services;
        }

    }
}