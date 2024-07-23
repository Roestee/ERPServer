using MediatR;
using TS.Result;

namespace ERPServer.Application.Features.Customers.UpdateCustomer
{
    public sealed record UpdateCustomerCommand(
        Guid id,
        string name,
        string taxDepartmant,
        string taxNumber,
        string city,
        string town,
        string fullAddress) : IRequest<Result<string>>;
}
