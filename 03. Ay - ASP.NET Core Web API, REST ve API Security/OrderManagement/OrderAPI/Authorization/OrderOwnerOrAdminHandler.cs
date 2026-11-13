using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using OrderAPI.Contracts;

namespace OrderAPI.Authorization;

public class OrderOwnerOrAdminRequirement : IAuthorizationRequirement
{
    
}

public class OrderOwnerOrAdminHandler : AuthorizationHandler<OrderOwnerOrAdminRequirement, OrderResponse>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OrderOwnerOrAdminRequirement requirement,
        OrderResponse order)
    {
        var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier); // token'daki "sub"

        if (context.User.IsInRole("Admin") || userId == order.CustomerId.ToString())
        {
            context.Succeed(requirement); // izin ver
        }

        // Succeed çağırmazsak -> yetki yok -> controller Forbid() ile 403 döner
        return Task.CompletedTask;
    }
}