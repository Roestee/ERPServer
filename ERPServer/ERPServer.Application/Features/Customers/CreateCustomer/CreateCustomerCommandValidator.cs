using FluentValidation;

namespace ERPServer.Application.Features.Customers.CreateCustomer
{
    public sealed class CreateCustomerCommandValidator : AbstractValidator<CreateCustomerCommand>
    {
        public CreateCustomerCommandValidator()
        {
            RuleFor(p=>p.taxNumber).MinimumLength(10).MaximumLength(11);
            RuleFor(p => p.name).MinimumLength(3);
        }
    }
}
