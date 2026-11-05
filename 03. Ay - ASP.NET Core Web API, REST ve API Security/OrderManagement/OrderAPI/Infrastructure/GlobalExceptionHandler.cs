using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using OrderAPI.Exceptions;

namespace OrderAPI.Infrastructure;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly IProblemDetailsService _problemDetailsService;
    private readonly ILogger<GlobalExceptionHandler> _logger;
    
    public GlobalExceptionHandler(IProblemDetailsService problemDetailsService, ILogger<GlobalExceptionHandler> logger)
    {
        _problemDetailsService = problemDetailsService;
        _logger = logger;
    }
    
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        int statusCode;
        string title;
        string? detail;

        if (exception is OrderNotFoundException)
        {
            statusCode = StatusCodes.Status404NotFound;
            title = "Sipariş bulunamadı";
            detail = exception.Message;
        }
        else if (exception is BusinessRuleException businessException)
        {
            statusCode = businessException.StatusCode; // 422 veya 409
            title = "İş kuralı ihlali";
            detail = exception.Message;
        }
        else
        {
            // Beklemediğimiz bir hata: logla ama iç detayı kullanıcıya gösterme
            _logger.LogError(exception, "Beklenmeyen bir hata oluştu");
            statusCode = StatusCodes.Status500InternalServerError;
            title = "Beklenmeyen bir hata oluştu";
            detail = null;
        }
        
        httpContext.Response.StatusCode = statusCode;

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = detail
        };

        // ProblemDetails'i standart JSON formatında cevaba yazar
        return await _problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            ProblemDetails = problemDetails
        });
    }
}