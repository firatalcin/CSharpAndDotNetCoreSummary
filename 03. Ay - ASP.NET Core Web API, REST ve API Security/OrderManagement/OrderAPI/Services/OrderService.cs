using Microsoft.Extensions.Options;
using OrderAPI.Contracts;
using OrderAPI.Exceptions;
using OrderAPI.Models;
using OrderAPI.Repositories;
using OrderAPI.Settings;

namespace OrderAPI.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _repository;
    private readonly OrderSettings _settings;
    private readonly ILogger<OrderService> _logger;

    public OrderService(IOrderRepository repository, IOptions<OrderSettings> options, ILogger<OrderService> logger)
    {
        _repository = repository;
        _settings = options.Value;
        _logger = logger;
    }
    
    public async Task<OrderResponse> CreateAsync(CreateOrderRequest request, CancellationToken cancellationToken)
    {
        if (request.Items.Count > _settings.MaxItemsPerOrder)
        {
            _logger.LogWarning(
                "İş kuralı ihlali: Kalem sayısı limiti aşıldı. CustomerId: {CustomerId}, ItemCount: {ItemCount}, MaxItems: {MaxItems}",
                request.CustomerId, request.Items.Count, _settings.MaxItemsPerOrder);

            throw new BusinessRuleException($"Bir sipariş en fazla {_settings.MaxItemsPerOrder} kalem olabilir.");
        }
        
        var orderItems = request.Items.Select(item => new OrderItem()
        {
            ProductId = item.ProductId,
            ProductName = item.ProductName,
            Quantity = item.Quantity,
            UnitPrice = item.UnitPrice
        }).ToList();
        
        var totalAmount = orderItems.Sum(x => x.Quantity * x.UnitPrice);
        
        if (totalAmount < _settings.MinOrderAmount)
        {
            _logger.LogWarning(
                "İş kuralı ihlali: Minimum tutar altında sipariş. CustomerId: {CustomerId}, TotalAmount: {TotalAmount}, MinOrderAmount: {MinOrderAmount}",
                request.CustomerId, totalAmount, _settings.MinOrderAmount);

            throw new BusinessRuleException(
                $"Sipariş tutarı en az {_settings.MinOrderAmount} TL olmalıdır. Mevcut tutar: {totalAmount} TL.");
        }
        
        var newOrder = new Order
        {
            Id = Guid.NewGuid(),
            CustomerId = request.CustomerId,
            OrderNumber = $"ORD-{DateTime.UtcNow:yyyyMMdd}-{Random.Shared.Next(1000, 9999)}",
            Status = OrderStatus.Pending,
            TotalAmount = totalAmount,
            CreatedAt = DateTime.UtcNow,
            Items = orderItems
        };

        await _repository.AddAsync(newOrder, cancellationToken);
        
        _logger.LogInformation(
            "Sipariş oluşturuldu. OrderId: {OrderId}, OrderNumber: {OrderNumber}, CustomerId: {CustomerId}, TotalAmount: {TotalAmount}",
            newOrder.Id, newOrder.OrderNumber, newOrder.CustomerId, newOrder.TotalAmount);

        return MapToResponse(newOrder);
    }

    public async Task<OrderResponse> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var order = await _repository.GetByIdAsync(id, cancellationToken);

        if (order is null)
        {
            throw new OrderNotFoundException(id);
        }

        return MapToResponse(order);
    }

    public async Task<PagedResponse<OrderResponse>> GetPagedAsync(OrderQueryParameters parameters, CancellationToken cancellationToken)
    {
        // Tuple'ı iki ayrı değişkene açıyoruz (deconstruction)
        var (orders, totalCount) = await _repository.GetPagedAsync(parameters, cancellationToken);

        var items = orders.Select(MapToResponse).ToList();
        
        return new PagedResponse<OrderResponse>(items, parameters.PageNumber, parameters.PageSize, totalCount);
    }

    public async Task CancelAsync(Guid id, CancellationToken cancellationToken)
    {
        var order = await _repository.GetByIdAsync(id, cancellationToken);

        if (order is null)
        {
            throw new OrderNotFoundException(id);
        }

        if (order.Status == OrderStatus.Shipped)
        {
            _logger.LogWarning(
                "İş kuralı ihlali: Kargodaki sipariş iptal edilmek istendi. OrderId: {OrderId}, Status: {Status}",
                order.Id, order.Status);
            
            throw new BusinessRuleException(
                "Kargoya verilmiş bir sipariş iptal edilemez.",
                StatusCodes.Status409Conflict);
        }
        
        order.Status = OrderStatus.Cancelled;
        
        _logger.LogInformation(
            "Sipariş iptal edildi. OrderId: {OrderId}, CustomerId: {CustomerId}",
            order.Id, order.CustomerId);
    }
    
    private static OrderResponse MapToResponse(Order order)
    {
        var itemDtos = order.Items.Select(i => new OrderItemDto(
            i.ProductId,
            i.ProductName,
            i.Quantity,
            i.UnitPrice
        )).ToList();

        return new OrderResponse(
            order.Id,
            order.CustomerId,
            order.OrderNumber,
            order.Status,
            order.TotalAmount,
            order.CreatedAt,
            itemDtos
        );
    }
}