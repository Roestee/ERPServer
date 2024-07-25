using ERPServer.Domain.Entities;
using ERPServer.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace ERPServer.Application.Features.Productions.GetAllProduction
{
    internal sealed class GetAllProductionQueryHandler(
        IProductionRepository productionRepository) : IRequestHandler<GetAllProductionQuery, Result<List<Production>>>
    {
        public async Task<Result<List<Production>>> Handle(GetAllProductionQuery request, CancellationToken cancellationToken)
        {
            var productions = await productionRepository
                .GetAll()
                .Include(x=>x.Product)
                .Include(x=>x.Depot)
                .OrderByDescending(x=>x.CreatedAt)
                .Include(x=>x.Product)
                .ToListAsync(cancellationToken);

            return productions;
        }
    }
}
