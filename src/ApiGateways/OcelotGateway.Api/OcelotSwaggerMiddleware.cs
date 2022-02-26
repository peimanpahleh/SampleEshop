using Microsoft.Extensions.Options;
using OcelotGateway.Api.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Text.RegularExpressions;

namespace OcelotGateway.Api
{
    public class OcelotSwaggerMiddleware
    {
        private readonly OcelotSwaggerOptions _options;

        private readonly RequestDelegate _next;

        public OcelotSwaggerMiddleware(
            RequestDelegate next,
            IOptionsMonitor<OcelotSwaggerOptions> optionsAccessor)
        {
            _next = next;
            _options = optionsAccessor.CurrentValue;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            var path = httpContext.Request.Path.Value;

            if (path.IndexOf("/swagger") > -1)
            {
                var content = await ReadContentAsync(httpContext);

                foreach (var replace in _options.SwaggerReplaces)
                {
                    Regex reg = new Regex(replace.UpstreamPathRouteRegex);

                    Match match = reg.Match(path);

                    if (match.Success)
                    {
                        string value = match.Groups[1].Value;

                        content = Regex.Replace(content, replace.DownstreamPathRouteRegex, $"/{value}/");
                    }
                }

                await WriteContentAsync(httpContext, content);
            }
            else
            {
                await _next(httpContext);
            }
        }

        private async Task<string> ReadContentAsync([NotNull] HttpContext httpContext)
        {
            var existingBody = httpContext.Response.Body;
            using (var newBody = new MemoryStream())
            {
                // We set the response body to our stream so we can read after the chain of middlewares have been called.
                httpContext.Response.Body = newBody;

                await _next(httpContext);

                // Reset the body so nothing from the latter middlewares goes to the output.
                httpContext.Response.Body = existingBody;

                newBody.Seek(0, SeekOrigin.Begin);
                var newContent = await new StreamReader(newBody).ReadToEndAsync();

                return newContent;
            }
        }

        private async Task WriteContentAsync([NotNull] HttpContext httpContext, string content)
        {
            httpContext.Response.ContentLength = Encoding.UTF8.GetByteCount(content);
            await httpContext.Response.WriteAsync(content);
        }
    }
}
