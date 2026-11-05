using System.Collections.Concurrent;
using OrderAPI.Models;

namespace OrderAPI.Repositories;

public class InMemoryOrderRepository : IOrderRepository
{
    private readonly List<Order> _orders = new();
    
    public Task AddAsync(Order order, CancellationToken cancellationToken)
    {
        _orders.Add(order);
        return Task.CompletedTask;
    }

    public Task<Order?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var order = _orders.FirstOrDefault(o => o.Id == id);
        return Task.FromResult(order);
    }

    public Task<List<Order>> GetAllAsync(CancellationToken cancellationToken)
    {
        return Task.FromResult(_orders.ToList());
    }
}