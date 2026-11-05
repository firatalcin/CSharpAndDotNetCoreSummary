using FluentValidation;
using OrderAPI.Contracts;

namespace OrderAPI.Validators;

public class OrderItemDtoValidator : AbstractValidator<OrderItemDto>
{
    public OrderItemDtoValidator()
    {
        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage("Adet (Quantity) 0'dan büyük olmalıdır.");

        RuleFor(x => x.UnitPrice)
            .GreaterThan(0).WithMessage("Birim fiyat (UnitPrice) 0'dan büyük olmalıdır.");
    }
}