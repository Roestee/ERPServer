using ERPServer.Domain.Dtos;
using ERPServer.Domain.Enums;
using ERPServer.Domain.Repositories;
using GenericRepository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace ERPServer.Application.Features.Orders.RequirementsPlanningByOrderId
{
    internal sealed class RequirementsPlanningByOrderIdCommandHandler(
        IOrderRepository orderRepository,
        IStockMovementRepository stockMovementRepository,
        IRecipeRepository recipeRepository,
        IUnitOfWork unitOfWork) : IRequestHandler<RequirementsPlanningByOrderIdCommand,
        Result<RequirementsPlanningByOrderIdCommandResponse>>
    {
        public async Task<Result<RequirementsPlanningByOrderIdCommandResponse>> Handle(RequirementsPlanningByOrderIdCommand request
            , CancellationToken cancellationToken)
        {
            var order = await orderRepository
                .Where(x=>x.Id == request.OrderId)
                .Include(x=>x.Details!)
                .ThenInclude(x=>x.Product)
                .FirstOrDefaultAsync(cancellationToken);

            if (order == null)
                return Result<RequirementsPlanningByOrderIdCommandResponse>.Failure("Sipariş bulunamadı!");

            var productsToBeProduced = new List<ProductDto>();
            var requirementsPlanningProducts = new List<ProductDto>();

            if (order.Details is not null)
            {
                foreach (var item in order.Details)
                {
                    var product = item.Product;
                    var movements =
                        await stockMovementRepository
                        .Where(p => p.ProductId == product!.Id)
                        .ToListAsync(cancellationToken);

                    var stock = movements.Sum(p => p.NumberOfEntries) - movements.Sum(p => p.NumberOfOutputs);
                    if(stock < item.Quantity)
                    {
                        var productToBeProduced = new ProductDto
                        {
                            Id = item.ProductId,
                            Name = product!.Name,
                            Quantity = item.Quantity - stock,
                        };

                        productsToBeProduced.Add(productToBeProduced);
                    }
                }
            }

            foreach (var item in productsToBeProduced)
            {
                var recipe = 
                    await recipeRepository
                    .Where(x => x.ProductId == item.Id)
                    .Include(x => x.Details!)
                    .ThenInclude(x => x.Product)
                    .FirstOrDefaultAsync(cancellationToken);

                if (recipe is not null && recipe.Details is not null)
                {
                    foreach (var productDetail in recipe.Details)
                    {
                        var stockMovementsForDetail = await stockMovementRepository
                            .Where(x => x.ProductId == productDetail!.ProductId)
                            .ToListAsync(cancellationToken);

                        var stock = stockMovementsForDetail.Sum(x => x.NumberOfEntries) - stockMovementsForDetail.Sum(x => x.NumberOfOutputs);
                        if (stock < productDetail.Quantity)
                        {
                            var requirementsProduct = new ProductDto
                            {
                                Id = productDetail.ProductId,
                                Name = productDetail.Product!.Name,
                                Quantity = productDetail.Quantity - stock,
                            };

                            requirementsPlanningProducts.Add(requirementsProduct);
                        }
                    }
                }
            }

            requirementsPlanningProducts = requirementsPlanningProducts.GroupBy(x=>x.Id)
                .Select(x => new ProductDto
                {
                    Id = x.Key,
                    Name = x.First().Name,
                    Quantity = x.Sum(i => i.Quantity),
                }).ToList();

            order.Status = OrderStatus.RequirementsPlanWorked;
            orderRepository.Update(order);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            
            return new RequirementsPlanningByOrderIdCommandResponse(
                DateOnly.FromDateTime(DateTime.Now)
                ,order.Number + " NO'lu Siparişin İhtiyaç Planlaması" 
                , requirementsPlanningProducts);
        }
    }
}
