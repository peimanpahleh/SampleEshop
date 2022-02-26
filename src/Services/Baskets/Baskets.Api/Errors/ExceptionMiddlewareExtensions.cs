
namespace Baskets.Api.Errors
{
    public static class ExceptionMiddlewareExtensions
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();

                    // Handle for jwt
                    // Microsoft.IdentityModel.Protocols.ConfigurationManager
                    // Microsoft.IdentityModel.Protocols

                    if (contextFeature.Error.Source == "Microsoft.IdentityModel.Protocols")
                    {
                        Log.Error("could not connect to identity server");
                        await context.Response.WriteAsync(new ErrorDetails()
                        {
                            StatusCode = context.Response.StatusCode,
                            Message = "could not connect to identity server"
                        }.ToString());

                        return;
                    }

                    if (contextFeature != null)
                    {
                        await context.Response.WriteAsync(new ErrorDetails()
                        {
                            StatusCode = context.Response.StatusCode,
                            Message = "Internal Server Error."
                        }.ToString());
                    }
                });
            });
        }
    }
}
