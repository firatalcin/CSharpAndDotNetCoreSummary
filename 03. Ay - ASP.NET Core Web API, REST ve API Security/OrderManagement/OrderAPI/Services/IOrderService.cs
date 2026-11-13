using OrderAPI.Contracts;
using OrderAPI.Models;

namespace OrderAPI.Services;

public interface IOrderService
{
    Task<OrderResponse> CreateAsync(Guid customerId, CreateOrderRequest request, CancellationToken cancellationToken);
    Task<OrderResponse> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<PagedResponse<OrderResponse>> GetPagedAsync(OrderQueryParameters parameters, Guid? customerId, CancellationToken cancellationToken);
    Task CancelAsync(Guid id, CancellationToken cancellationToken);
    Task UpdateStatusAsync(Guid id, OrderStatus newStatus, CancellationToken cancellationToken);
}