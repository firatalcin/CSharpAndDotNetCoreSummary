using OrderAPI.Contracts;
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

    public Task<(List<Order> Items, int TotalCount)> GetPagedAsync(OrderQueryParameters parameters, Guid? customerId, CancellationToken cancellationToken)
    {
        IEnumerable<Order> query = _orders;

        // 0) SAHİPLİK FİLTRESİ: customerId null ise (Admin) hepsini gör
        if (customerId is not null)
        {
            query = query.Where(o => o.CustomerId == customerId);
        }
        
        // 1) FİLTRELEME (null ise hiç uygulanmaz)
        if (parameters.Status is not null)
        {
            query = query.Where(o => o.Status == parameters.Status);
        }
        
        if (parameters.MinTotalAmount is not null)
        {
            query = query.Where(o => o.TotalAmount >= parameters.MinTotalAmount);
        }
        
        // 2) SIRALAMA
        bool descending = parameters.SortDirection.Equals("desc", StringComparison.OrdinalIgnoreCase);
        
        if (parameters.SortBy.Equals("totalAmount", StringComparison.OrdinalIgnoreCase))
        { query = descending ? query.OrderByDescending(o => o.TotalAmount) : query.OrderBy(o => o.TotalAmount);
        }
        else // createdAt
        {
            query = descending ? query.OrderByDescending(o => o.CreatedAt) : query.OrderBy(o => o.CreatedAt);
        }
        
        // 3) TOPLAM SAYI (sayfalamadan ÖNCE!)
        int totalCount = query.Count();

        // 4) SAYFALAMA
        var items = query
            .Skip((parameters.PageNumber - 1) * parameters.PageSize)
            .Take(parameters.PageSize)
            .ToList();

        return Task.FromResult((items, totalCount));
    }
}