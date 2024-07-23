using MediatR;
using TS.Result;

namespace ERPServer.Application.Features.Depots.CreateDepot
{
    public sealed record CreateDepotCommand(
        string name,
        string city,
        string town,
        string fullAddress): IRequest<Result<string>>;
}
