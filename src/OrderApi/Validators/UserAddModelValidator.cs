using System.Text.RegularExpressions;
using FluentValidation;
using Order.Models.User;

namespace Order.Validators;

public class UserAddModelValidator : AbstractValidator<UserAddModel>
{
	public UserAddModelValidator()
	{
		RuleFor(r => r).NotEmpty().WithMessage("Model must be not null");
		RuleFor(r => r.Email)
			.NotEmpty().WithMessage("Email must be not null")
			.EmailAddress().WithMessage("Email must be a valid")
			.Matches(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$", RegexOptions.IgnoreCase).WithMessage("Email must be a valid")
			.MaximumLength(100).WithMessage("Email must be less than 100 characters");
		RuleFor(r => r.PhoneNumber)
			.NotEmpty().WithMessage("PhoneNumber must be not null")
			.MaximumLength(50).WithMessage("PhoneNumber must be less than 100 characters");
	}
}
