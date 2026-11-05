using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using OrderAPI.Contracts;
using OrderAPI.Models;
using OrderAPI.Services;

namespace OrderAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;
    private readonly IValidator<CreateOrderRequest> _createOrderValidator;

    public OrdersController(IOrderService orderService, IValidator<CreateOrderRequest> createOrderValidator)
    {
        _orderService = orderService;
        _createOrderValidator = createOrderValidator;
    }
    
    [HttpPost]
    [ProducesResponseType(typeof(OrderResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails),StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult<OrderResponse>> CreateOrder([FromBody] CreateOrderRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await _createOrderValidator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            foreach (var error in validationResult.Errors)
            {
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }

            return ValidationProblem(ModelState); // 400 Bad Request
        }
        
        var response = await _orderService.CreateAsync(request, cancellationToken);

        return CreatedAtAction(
            actionName: nameof(GetOrderById),
            routeValues: new { id = response.Id },
            value: response);
    }
    
    // GET /api/orders/{id:guid}
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(OrderResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<OrderResponse>> GetOrderById(Guid id, CancellationToken cancellationToken)
    {
        var response = await _orderService.GetByIdAsync(id, cancellationToken);
        return Ok(response);
    }
    
    // GET /api/orders
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<OrderResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<OrderResponse>>> GetOrders(CancellationToken  cancellationToken)
    {
        var response = await _orderService.GetAllAsync(cancellationToken);
        return Ok(response);
    }
    
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CancelOrder(Guid id, CancellationToken cancellationToken)
    {
        await _orderService.CancelAsync(id, cancellationToken);
        return NoContent();
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