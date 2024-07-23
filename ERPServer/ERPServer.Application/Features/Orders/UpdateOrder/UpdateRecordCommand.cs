using AutoMapper;
using ERPServer.Domain.Repositories;
using GenericRepository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace ERPServer.Application.Features.Orders.UpdateOrder
{
    internal sealed class UpdateRecordCommand : IRequestHandler<UpdateOrderCommand, Result<string>>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderDetailRepository _orderDetailRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateRecordCommand(IOrderRepository orderRepository, IUnitOfWork unitOfWork, IMapper mapper, IOrderDetailRepository orderDetailRepository)
        {
            _orderRepository = orderRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _orderDetailRepository = orderDetailRepository;
        }

        public async Task<Result<string>> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await _orderRepository
                .Where(x => x.Id == request.Id)
                .Include(x=>x.Details)
                .FirstOrDefaultAsync(cancellationToken);
            if (order is null)
                return Result<string>.Failure("Sipariş bulunamadı!");

            _orderDetailRepository.DeleteRange(order.Details);

            _mapper.Map(request, order);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return "Sipariş başarıyla güncellendi.";
        }
    }
}

