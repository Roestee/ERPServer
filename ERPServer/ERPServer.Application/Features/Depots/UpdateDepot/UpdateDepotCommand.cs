using MediatR;
using TS.Result;

namespace ERPServer.Application.Features.Depots.UpdateDepot
{
    public sealed record UpdateDepotCommand (
        Guid id,
        string name,
        string city,
        string town,
        string fullAddress) : IRequest<Result<string>>;
}
