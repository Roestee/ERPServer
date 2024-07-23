using AutoMapper;
using ERPServer.Domain.Entities;
using ERPServer.Domain.Repositories;
using GenericRepository;
using MediatR;
using TS.Result;

namespace ERPServer.Application.Features.Products.CreateProduct
{
    internal sealed class CreateProductCommandHandler(
        IProductRepository productRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper) : IRequestHandler<CreateProductCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var isNameExists = await productRepository.AnyAsync(p=>p.Name == request.Name, cancellationToken);
            if (isNameExists) {
                return Result<string>.Failure("Ürün adı daha önce kullanılmış!");
            }

            var product = mapper.Map<Product>(request);
            await productRepository.AddAsync(product);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return "Ürün başarıyla eklendi.";
        }
    }
}
