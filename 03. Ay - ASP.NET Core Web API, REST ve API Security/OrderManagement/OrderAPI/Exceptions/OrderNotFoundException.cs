namespace OrderAPI.Exceptions;

public class OrderNotFoundException : Exception
{
    public OrderNotFoundException(Guid orderId) : base($"'{orderId}' ID'li sipariş bulunamadı")
    {
    }
}