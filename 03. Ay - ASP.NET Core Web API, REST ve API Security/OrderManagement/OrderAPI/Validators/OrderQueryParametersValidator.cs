using FluentValidation;
using OrderAPI.Contracts;

namespace OrderAPI.Validators;

public class OrderQueryParametersValidator : AbstractValidator<OrderQueryParameters>
{
    // Whitelist: sadece bu değerlere izin var
    private static readonly string[] AllowedSortFields = ["createdAt", "totalAmount"];
    private static readonly string[] AllowedSortDirections = ["asc", "desc"];

    public OrderQueryParametersValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1).WithMessage("pageNumber en az 1 olmalıdır.");

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1).WithMessage("pageSize en az 1 olmalıdır.");

        RuleFor(x => x.SortBy)
            .Must(value => AllowedSortFields.Contains(value, StringComparer.OrdinalIgnoreCase))
            .WithMessage("sortBy sadece 'createdAt' veya 'totalAmount' olabilir.");

        RuleFor(x => x.SortDirection)
            .Must(value => AllowedSortDirections.Contains(value, StringComparer.OrdinalIgnoreCase))
            .WithMessage("sortDirection sadece 'asc' veya 'desc' olabilir.");

        RuleFor(x => x.MinTotalAmount)
            .GreaterThanOrEqualTo(0).When(x => x.MinTotalAmount is not null)
            .WithMessage("minTotalAmount negatif olamaz.");
    }
}