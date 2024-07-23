using ERPServer.Domain.Entities;
using MediatR;
using TS.Result;

namespace ERPServer.Application.Features.RecipeDetails.GetRecipeByIdWithDetails
{
    public sealed record GetRecipeByIdWithDetailQuery(
        Guid RecipeId) : IRequest<Result<Recipe>>;
}
