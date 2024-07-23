using MediatR;
using TS.Result;

namespace ERPServer.Application.Features.Products.DeleteProductById
{
    public sealed record DeleteProductByIdCommand(Guid id) : IRequest<Result<string>>;
}
