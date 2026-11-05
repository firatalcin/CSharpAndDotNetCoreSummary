using OrderAPI.Contracts;

namespace OrderAPI.Services;

public interface IOrderService
{
    Task<OrderResponse> CreateAsync(CreateOrderRequest request, CancellationToken cancellationToken);
    Task<OrderResponse> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<List<OrderResponse>> GetAllAsync(CancellationToken cancellationToken);
    Task CancelAsync(Guid id, CancellationToken cancellationToken);
}