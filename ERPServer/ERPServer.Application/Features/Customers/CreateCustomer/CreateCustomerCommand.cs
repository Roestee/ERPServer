using MediatR;
using TS.Result;

namespace ERPServer.Application.Features.Customers.CreateCustomer
{
    public sealed record CreateCustomerCommand(
        string name,
        string taxDepartmant,
        string taxNumber,
        string city,
        string town,
        string fullAddress): IRequest<Result<string>>;
}
