using AutoMapper;
using ERPServer.Domain.Entities;
using ERPServer.Domain.Repositories;
using GenericRepository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace ERPServer.Application.Features.Productions.CreateProduction
{
    internal sealed class CreateProductionCommandHandler(
        IProductionRepository productionRepository,
        IRecipeRepository recipeRepository,
        IStockMovementRepository stockMovementRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper) : IRequestHandler<CreateProductionCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(CreateProductionCommand request, CancellationToken cancellationToken)
        {
            var production = mapper.Map<Production>(request);
            var newMovements = new List<StockMovement>();
            var recipe = await recipeRepository
                .Where(p => p.ProductId == request.ProductId)
                .Include(x => x.Details!)
                .ThenInclude(x => x.Product)
                .FirstOrDefaultAsync(cancellationToken);

            if (recipe is not null && recipe.Details is not null)
            {
                var details = recipe.Details;
                foreach (var item in details)
                {
                    var movements = await stockMovementRepository
                        .Where(x => x.ProductId == item.ProductId)
                        .ToListAsync(cancellationToken);

                    var depotIds = movements
                        .GroupBy(p=>p.DepotId)
                        .Select(x => x.Key)
                        .ToList();

                    var stock = movements.Sum(x => x.NumberOfEntries) - movements.Sum(x => x.NumberOfOutputs);
                    if (item.Quantity > stock)
                        return Result<string>.Failure(item.Product!.Name + " ürününden üretim için yeterli miktarda yok. Eksik miktar: " + (item.Quantity - stock));

                    foreach (var depotId in depotIds)
                    {
                        if(item.Quantity <= 0)
                            break;

                        var quantity = movements
                            .Where(x=>x.DepotId == depotId)
                            .Sum(x => x.NumberOfEntries - x.NumberOfOutputs);

                        var totalAmount = movements
                            .Where(x => x.DepotId == depotId && x.NumberOfEntries > 0)
                            .Sum(x => x.Price * x.NumberOfEntries);

                        var totalEntriesQuantity = movements
                        .Where(x => x.DepotId == depotId && x.NumberOfEntries > 0)
                        .Sum(x => x.NumberOfEntries);

                        var price = totalAmount / totalEntriesQuantity;

                        var stockMovement = new StockMovement
                        {
                            ProductionId = production.Id,
                            ProductId = item.ProductId,
                            DepotId = depotId,
                            Price = price,
                        };

                        if (item.Quantity <= quantity)
                        {
                            stockMovement.NumberOfOutputs = item.Quantity;
                        }
                        else
                        {
                            stockMovement.NumberOfOutputs = quantity;                           
                        }

                        item.Quantity -= quantity;
                        newMovements.Add(stockMovement);
                    }
                }
            }

            await productionRepository.AddAsync(production, cancellationToken);
            await stockMovementRepository.AddRangeAsync(newMovements, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return "Ürün başarıyla üretildi.";
        }
    }
}
