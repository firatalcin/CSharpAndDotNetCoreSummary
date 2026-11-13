namespace OrderAPI.Contracts;

public record CreateOrderRequest(
    List<OrderItemDto> Items);