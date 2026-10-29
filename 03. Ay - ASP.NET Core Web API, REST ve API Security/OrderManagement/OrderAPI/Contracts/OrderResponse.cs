using OrderAPI.Models;

namespace OrderAPI.Contracts;

public record OrderResponse(
    Guid Id,
    Guid CustomerId,
    string OrderNumber,
    OrderStatus Status,
    decimal TotalAmount,
    DateTime CreatedAt,
    List<OrderItemDto> Items);