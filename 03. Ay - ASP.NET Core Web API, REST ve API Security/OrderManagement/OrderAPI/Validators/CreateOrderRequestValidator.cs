using FluentValidation;
using OrderAPI.Contracts;

namespace OrderAPI.Validators;

public class CreateOrderRequestValidator : AbstractValidator<CreateOrderRequest>
{
    public CreateOrderRequestValidator()
    {
        RuleFor(x => x.Items)
            .NotEmpty().WithMessage("Sipariş en az bir kalem içermelidir.");

        // Listedeki HER kalemi OrderItemDtoValidator ile kontrol et
        RuleForEach(x => x.Items)
            .SetValidator(new OrderItemDtoValidator());
    }
}