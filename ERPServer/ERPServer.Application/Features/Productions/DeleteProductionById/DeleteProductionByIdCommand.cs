using ERPServer.Domain.Repositories;
using GenericRepository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace ERPServer.Application.Features.Productions.DeleteProductionById
{
    public sealed record DeleteProductionByIdCommand(
        Guid Id) : IRequest<Result<string>>;

    internal sealed class DeleteProductionByIdCommandHandler(
        IProductionRepository productionRepository,
        IStockMovementRepository stockMovementRepository,
        IUnitOfWork unitOfWork) : IRequestHandler<DeleteProductionByIdCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(DeleteProductionByIdCommand request, CancellationToken cancellationToken)
        {
            var production = await productionRepository.GetByExpressionAsync(x=>x.Id == request.Id, cancellationToken);
            if(production is null)
            {
                return Result<string>.Failure("Üretim bulunamadı!");
            }

            var movements = await stockMovementRepository.Where(x=>x.ProductId == production.Id).ToListAsync(cancellationToken);
            stockMovementRepository.DeleteRange(movements);
            productionRepository.Delete(production);

            await unitOfWork.SaveChangesAsync(cancellationToken);
            return "Üretim başarıyla silindi.";
        }
    }
}
