using FluentValidation;
using OrderApi.Models.Order;

namespace OrderApi.Validators.Order;

public class OrderAddModelValidator : AbstractValidator<OrderAddModel>
{
	public OrderAddModelValidator()
	{
		RuleFor(r => r).NotEmpty().WithMessage("Model must be not null");
		RuleFor(r => r.Amount)
			.NotEmpty().WithMessage("Amount must be not null")
			.LessThanOrEqualTo(20000).WithMessage("Amount must be no more than 20.000 TL")
			.GreaterThanOrEqualTo(100).WithMessage("Amount must be at least 100 TL");
		RuleFor(r => r.OrderDate)
			.NotEmpty().WithMessage("OrderDate must be not null")
			.Must(e => e.Day < 28).WithMessage("OrderDate must be between 1-28 day of month");
	}
}
