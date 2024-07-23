using AutoMapper;
using ERPServer.Domain.Repositories;
using GenericRepository;
using MediatR;
using TS.Result;

namespace ERPServer.Application.Features.RecipeDetails.UpdateRecipeDetail
{
    public sealed record UpdateRecipeDetailCommand(
        Guid Id,
        Guid ProductId,
        Guid RecipeId,
        decimal Quantity) : IRequest<Result<string>>;

    internal sealed class UpdateRecipeDetailCommandHandler(
        IRecipeDetailRepository recipeDetailRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper) : IRequestHandler<UpdateRecipeDetailCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(UpdateRecipeDetailCommand request, CancellationToken cancellationToken)
        {
            var recipeDetail = await recipeDetailRepository.GetByExpressionWithTrackingAsync(p => p.Id == request.Id, cancellationToken);
            if (recipeDetail is null)
            {
                Result<string>.Failure("Reçetede bu ürün bulunamadı!");
            }

            var updatedRecipeDetail = await recipeDetailRepository.GetByExpressionWithTrackingAsync(
                p => p.Id != request.Id &&
                p.ProductId == request.ProductId &&
                p.RecipeId == request.RecipeId, cancellationToken);

            if(updatedRecipeDetail is not null)
            {
                updatedRecipeDetail.Quantity += request.Quantity;
                recipeDetailRepository.Delete(recipeDetail!);
            }
            else
            {
                mapper.Map(request, recipeDetail);
            }

            
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return "Reçetedeki ürün başarıyla güncellendi.";
        }
    }
}
