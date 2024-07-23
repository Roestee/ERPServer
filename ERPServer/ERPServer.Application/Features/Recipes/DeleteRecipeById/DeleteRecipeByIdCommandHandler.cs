using ERPServer.Domain.Repositories;
using GenericRepository;
using MediatR;
using TS.Result;

namespace ERPServer.Application.Features.Recipes.DeleteRecipeById
{
    internal sealed class DeleteRecipeByIdCommandHandler(
        IRecipeRepository recipeRepository,
        IUnitOfWork unitOfWork) : IRequestHandler<DeleteRecipeByIdCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(DeleteRecipeByIdCommand request, CancellationToken cancellationToken)
        {
            var recipe = await recipeRepository.GetByExpressionAsync(x=>x.Id == request.Id, cancellationToken);
            if (recipe is null)
            {
                return Result<string>.Failure("Reçete bulunamadı!");
            }

            recipeRepository.Delete(recipe);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return "Reçete başarıyla silindi.";
        }
    }
}
