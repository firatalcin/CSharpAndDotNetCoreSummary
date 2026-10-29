using Microsoft.AspNetCore.Mvc;
using OrderAPI.Contracts;
using OrderAPI.Models;

namespace OrderAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private static readonly List<Order> _orders = new();

    [HttpPost]
    [ProducesResponseType(typeof(OrderResponse), StatusCodes.Status201Created)]
    public ActionResult<OrderResponse> CreateOrder([FromBody] CreateOrderRequest request)
    {
        var orderItems = request.Items.Select(item => new OrderItem()
        {
            ProductId = item.ProductId,
            ProductName = item.ProductName,
            Quantity = item.Quantity,
            UnitPrice = item.UnitPrice
        }).ToList();

        var totalAmount = orderItems.Sum(x => x.Quantity * x.UnitPrice);
        
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

        _orders.Add(newOrder);
        
        var response = MapToResponse(newOrder);

        return CreatedAtAction(
            actionName: nameof(GetOrderById),
            routeValues: new { id = newOrder.Id },
            value: response);

    }
    
    // GET /api/orders/{id:guid}
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(OrderResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<OrderResponse> GetOrderById(Guid id)
    {
        var order = _orders.FirstOrDefault(o => o.Id == id);

        if (order is null)
        {
            return NotFound(); // 404 Not Found
        }

        return Ok(MapToResponse(order)); // 200 OK
    }
    
    // GET /api/orders
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<OrderResponse>), StatusCodes.Status200OK)]
    public ActionResult<IEnumerable<OrderResponse>> GetOrders()
    {
        var responseList = _orders.Select(MapToResponse).ToList();
        return Ok(responseList); // 200 OK
    }
    
    
    // Yardımcı (Helper) Mapping Metodu
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