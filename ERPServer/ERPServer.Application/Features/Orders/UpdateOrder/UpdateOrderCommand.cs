using System;
using AutoMapper;
using ERPServer.Domain.Dtos;
using ERPServer.Domain.Repositories;
using GenericRepository;
using MediatR;
using TS.Result;

namespace ERPServer.Application.Features.Orders.UpdateOrder
{
	public sealed record UpdateOrderCommand(
		Guid Id,
        Guid CustomerId,
        DateTime Date,
        DateTime DeliveryDate,
        List<OrderDetailDto> Details) : IRequest<Result<string>>;

    internal sealed class UpdateRecordCommand : IRequestHandler<UpdateOrderCommand, Result<string>>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateRecordCommand(IOrderRepository orderRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public Task<Result<string>> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}

