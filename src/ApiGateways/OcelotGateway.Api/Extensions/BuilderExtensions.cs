using Microsoft.Extensions.Options;
using OcelotGateway.Api;
using OcelotGateway.Api.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OcelotGateway.Api.Extensions
{
    public static class BuilderExtensions
    {
        public static IServiceCollection AddOcelotSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen();
            return services;
        }

        public static IApplicationBuilder UseOcelotSwagger(this IApplicationBuilder app)
        {
            var optionsAccessor = app.ApplicationServices.GetRequiredService<IOptionsMonitor<OcelotSwaggerOptions>>();

            app.UseSwagger();
            app.UseSwaggerUI(
                options =>
                {
                    optionsAccessor.CurrentValue.SwaggerEndPoints.ForEach(
                        i => options.SwaggerEndpoint(i.Url, i.Name));
                });

            app.UseMiddleware<OcelotSwaggerMiddleware>();
            return app;
        }
    }


}
