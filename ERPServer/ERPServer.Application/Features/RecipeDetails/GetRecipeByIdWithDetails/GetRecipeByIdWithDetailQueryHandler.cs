using ERPServer.Domain.Entities;
using ERPServer.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace ERPServer.Application.Features.RecipeDetails.GetRecipeByIdWithDetails
{
    internal sealed class GetRecipeByIdWithDetailQueryHandler(
        IRecipeRepository recipeRepository) : IRequestHandler<GetRecipeByIdWithDetailQuery, Result<Recipe>>
    {
        public async Task<Result<Recipe>> Handle(GetRecipeByIdWithDetailQuery request, CancellationToken cancellationToken)
        {
            var recipe = await recipeRepository
                .Where(p => p.Id == request.RecipeId)
                .Include(p => p.Product)
                .Include(p => p.Details!.OrderBy(p=>p.Product!.Name))
                .ThenInclude(p => p.Product)
                .FirstOrDefaultAsync(cancellationToken);

            if (recipe is null)
            {
                return Result<Recipe>.Failure("Ürüne ait reçete bulunamadı!");
            }

            return recipe;
        }
    }
}
