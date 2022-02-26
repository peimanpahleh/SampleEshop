namespace OcelotGateway.Api.Configuration;

public class OcelotSwaggerOptions
{
    public List<SwaggerReplace> SwaggerReplaces { get; set; } = new List<SwaggerReplace>();
    public List<SwaggerEndPoint> SwaggerEndPoints { get; set; } = new List<SwaggerEndPoint>();
}
