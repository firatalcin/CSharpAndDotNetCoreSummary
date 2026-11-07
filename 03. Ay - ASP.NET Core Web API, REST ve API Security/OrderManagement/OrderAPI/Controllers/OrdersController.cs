using Asp.Versioning;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using OrderAPI.Contracts;
using OrderAPI.Models;
using OrderAPI.Services;

namespace OrderAPI.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;
    private readonly IValidator<CreateOrderRequest> _createOrderValidator;
    private readonly IValidator<OrderQueryParameters> _queryValidator;

    public OrdersController(IOrderService orderService, IValidator<CreateOrderRequest> createOrderValidator, IValidator<OrderQueryParameters> queryValidator)
    {
        _orderService = orderService;
        _createOrderValidator = createOrderValidator;
        _queryValidator = queryValidator;
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
            return ToValidationProblem(validationResult); // 400
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
    [ProducesResponseType(typeof(PagedResponse<OrderResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PagedResponse<OrderResponse>>> GetOrders([FromQuery] OrderQueryParameters parameters, CancellationToken  cancellationToken)
    {
        var validationResult = await _queryValidator.ValidateAsync(parameters, cancellationToken);
        if (!validationResult.IsValid)
        {
            return ToValidationProblem(validationResult); // 400 (ör: sortBy=hackedField)
        }
        
        var response = await _orderService.GetPagedAsync(parameters, cancellationToken);
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
    
    // FluentValidation hatalarını standart 400 ValidationProblemDetails'e çevirir
    private ActionResult ToValidationProblem(ValidationResult validationResult)
    {
        foreach (var error in validationResult.Errors)
        {
            ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
        }

        return ValidationProblem(ModelState);
    }
}