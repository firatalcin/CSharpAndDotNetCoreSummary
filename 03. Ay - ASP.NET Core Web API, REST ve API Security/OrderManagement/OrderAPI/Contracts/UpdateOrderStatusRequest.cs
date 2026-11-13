using OrderAPI.Models;

namespace OrderAPI.Contracts;

public record UpdateOrderStatusRequest(OrderStatus Status);