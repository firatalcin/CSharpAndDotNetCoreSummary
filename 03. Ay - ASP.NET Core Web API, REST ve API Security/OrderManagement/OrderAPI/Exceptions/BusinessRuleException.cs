namespace OrderAPI.Exceptions;

public class BusinessRuleException : Exception
{
    public int StatusCode { get; }

    public BusinessRuleException(string message, int statusCode = StatusCodes.Status422UnprocessableEntity) :
        base(message)
    {
        StatusCode = statusCode;
    }
}