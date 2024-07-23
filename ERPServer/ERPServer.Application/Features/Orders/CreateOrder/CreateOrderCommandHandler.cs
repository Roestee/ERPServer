using AutoMapper;
using ERPServer.Domain.Entities;
using ERPServer.Domain.Repositories;
using GenericRepository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace ERPServer.Application.Features.Orders.CreateOrder
{
    internal sealed class CreateOrderCommandHandler: IRequestHandler<CreateOrderCommand, Result<string>>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateOrderCommandHandler(IUnitOfWork unitOfWork, IOrderRepository orderRepository, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        public async Task<Result<string>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var lastOrder = await _orderRepository
                .Where(p => p.OrderNumberYear == request.Date.Year)
                .OrderByDescending(p => p.OrderNumber)
                .FirstOrDefaultAsync(cancellationToken);

            var lastOrderNumber = 0;

            if(lastOrder is not null)
                lastOrderNumber = lastOrder.OrderNumber;

            var details = request.Details.Select(x => new OrderDetail
            {
                UnitPrice = x.UnitPrice,
                Quantity = x.Quantity,
                ProductId = x.ProductId
            }).ToList();

            var order = _mapper.Map<Order>(request);
            order.OrderNumber = lastOrderNumber++;
            order.OrderNumberYear = request.Date.Year;

            await _orderRepository.AddAsync(order, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return "Sipariş başarıyla eklendi.";
        }
    }
}

