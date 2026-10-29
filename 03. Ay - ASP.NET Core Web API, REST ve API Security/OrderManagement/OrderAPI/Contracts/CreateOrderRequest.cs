namespace OrderAPI.Contracts;

public record CreateOrderRequest(
    Guid CustomerId,
    List<OrderItemDto> Items);