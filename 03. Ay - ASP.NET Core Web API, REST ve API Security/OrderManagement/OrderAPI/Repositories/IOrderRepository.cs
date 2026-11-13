using OrderAPI.Contracts;
using OrderAPI.Models;

namespace OrderAPI.Repositories;

public interface IOrderRepository
{
    Task AddAsync(Order order, CancellationToken cancellationToken);
    Task<Order?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<(List<Order> Items, int TotalCount)> GetPagedAsync(OrderQueryParameters parameters, Guid? customerId, CancellationToken cancellationToken);
}