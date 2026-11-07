namespace OrderAPI.Middlewares;

public class CorrelationIdMiddleware
{
    public const string HeaderName = "X-Correlation-ID";

    private readonly RequestDelegate _next;
    private readonly ILogger<CorrelationIdMiddleware> _logger;
    
    public CorrelationIdMiddleware(RequestDelegate next, ILogger<CorrelationIdMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // 1) İstemci header gönderdiyse onu kullan, göndermediyse yeni üret
        string correlationId = context.Request.Headers[HeaderName].FirstOrDefault()
                               ?? Guid.NewGuid().ToString();
        
        // 2) Cevap istemciye gönderilmeden hemen önce header'a ekle
        context.Response.OnStarting(() =>
        {
            context.Response.Headers[HeaderName] = correlationId;
            return Task.CompletedTask;
        });

        // 3) Bu isteğin içindeki TÜM loglara CorrelationId'yi ekle
        using (_logger.BeginScope("CorrelationId: {CorrelationId}", correlationId))
        {
            await _next(context); // pipeline'daki bir sonraki adıma geç
        }
    }
}