using System.Diagnostics;

namespace Products.Application.Configuration.Behaviours;

public class PerformanceBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    private readonly Stopwatch _timer;
    private readonly ILogger<TRequest> _logger;

    public PerformanceBehaviour(ILogger<TRequest> logger)
    {
        _timer = new Stopwatch();
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        _timer.Start();

        var response = await next();

        _timer.Stop();

        var elapsedMilliseconds = _timer.ElapsedMilliseconds;

        // just for testing
        //bool logEverything = true;

        var requestName = typeof(TRequest).Name;

        _logger.LogInformation($"PerformanceBehaviour: {requestName}  Time(ms):{elapsedMilliseconds}");

        return response;
    }
}
