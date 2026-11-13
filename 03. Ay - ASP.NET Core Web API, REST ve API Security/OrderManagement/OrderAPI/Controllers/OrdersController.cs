using System.Security.Claims;
using Asp.Versioning;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using OrderAPI.Contracts;
using OrderAPI.Services;

namespace OrderAPI.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Authorize] // Bu controller'daki TÜM endpoint'ler için geçerli bir JWT token zorunlu
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;
    private readonly IValidator<CreateOrderRequest> _createOrderValidator;
    private readonly IValidator<OrderQueryParameters> _queryValidator;
    private readonly IAuthorizationService _authorizationService;

    public OrdersController(
        IOrderService orderService,
        IValidator<CreateOrderRequest> createOrderValidator,
        IValidator<OrderQueryParameters> queryValidator,
        IAuthorizationService authorizationService)
    {
        _orderService = orderService;
        _createOrderValidator = createOrderValidator;
        _queryValidator = queryValidator;
        _authorizationService = authorizationService;
    }

    // Token'daki "sub" claim'i = giriş yapan kullanıcının ID'si
    private Guid CurrentUserId => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpPost]
    [EnableRateLimiting("PerUserOrderPolicy")] // kullanıcı başına dakikada 5 sipariş
    [ProducesResponseType(typeof(OrderResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
    public async Task<ActionResult<OrderResponse>> CreateOrder([FromBody] CreateOrderRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await _createOrderValidator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            return ToValidationProblem(validationResult); // 400
        }

        // CustomerId artık istemciden değil, TOKEN'dan geliyor
        var response = await _orderService.CreateAsync(CurrentUserId, request, cancellationToken);

        return CreatedAtAction(nameof(GetOrderById), new { id = response.Id }, response);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(OrderResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<OrderResponse>> GetOrderById(Guid id, CancellationToken cancellationToken)
    {
        var order = await _orderService.GetByIdAsync(id, cancellationToken); // yoksa 404

        // Resource-Based Authorization: "Bu sipariş bu kullanıcının mı (veya Admin mi)?"
        var authResult = await _authorizationService.AuthorizeAsync(User, order, "OrderOwnerOrAdmin");
        if (!authResult.Succeeded)
        {
            return Forbid(); // 403
        }

        return Ok(order);
    }

    [HttpGet]
    [ProducesResponseType(typeof(PagedResponse<OrderResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PagedResponse<OrderResponse>>> GetOrders([FromQuery] OrderQueryParameters parameters, CancellationToken cancellationToken)
    {
        var validationResult = await _queryValidator.ValidateAsync(parameters, cancellationToken);
        if (!validationResult.IsValid)
        {
            return ToValidationProblem(validationResult); // 400 (ör: sortBy=hackedField)
        }

        // Admin herkesi görür (null = filtre yok), müşteri sadece kendi siparişlerini görür
        Guid? customerId = User.IsInRole("Admin") ? null : CurrentUserId;

        var response = await _orderService.GetPagedAsync(parameters, customerId, cancellationToken);
        return Ok(response);
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CancelOrder(Guid id, CancellationToken cancellationToken)
    {
        // Başkasının siparişini iptal edememeli -> önce sahiplik kontrolü
        var order = await _orderService.GetByIdAsync(id, cancellationToken);

        var authResult = await _authorizationService.AuthorizeAsync(User, order, "OrderOwnerOrAdmin");
        if (!authResult.Succeeded)
        {
            return Forbid();
        }

        await _orderService.CancelAsync(id, cancellationToken);
        return NoContent();
    }

    [HttpPatch("{id:guid}/status")]
    [Authorize(Policy = "RequireAdminRole")] // sadece Admin
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] UpdateOrderStatusRequest request, CancellationToken cancellationToken)
    {
        await _orderService.UpdateStatusAsync(id, request.Status, cancellationToken);
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