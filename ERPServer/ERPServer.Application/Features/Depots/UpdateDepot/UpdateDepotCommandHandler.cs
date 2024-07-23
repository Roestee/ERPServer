using AutoMapper;
using ERPServer.Domain.Repositories;
using GenericRepository;
using MediatR;
using TS.Result;

namespace ERPServer.Application.Features.Depots.UpdateDepot
{
    internal sealed class UpdateDepotCommandHandler(
        IDepotRepository depotRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper) : IRequestHandler<UpdateDepotCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(UpdateDepotCommand request, CancellationToken cancellationToken)
        {
            var depot = await depotRepository.GetByExpressionWithTrackingAsync(p=>p.Id == request.id, cancellationToken);
            if(depot is null)
            {
                return Result<string>.Failure($"{request.id}'li depo bulunamadı!");
            }

            mapper.Map(request, depot);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return "Depo bilgileri başarıyla güncellendi.";
        }
    }
}
