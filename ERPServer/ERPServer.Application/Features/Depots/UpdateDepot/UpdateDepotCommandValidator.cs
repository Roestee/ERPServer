using FluentValidation;

namespace ERPServer.Application.Features.Depots.UpdateDepot
{
    public sealed class UpdateDepotCommandValidator: AbstractValidator<UpdateDepotCommand>
    {
        public UpdateDepotCommandValidator()
        {
            RuleFor(p => p.name).MinimumLength(3);
        }
    }
}
