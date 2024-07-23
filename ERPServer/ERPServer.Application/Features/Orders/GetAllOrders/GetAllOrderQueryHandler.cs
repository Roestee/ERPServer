using ERPServer.Domain.Entities;
using ERPServer.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace ERPServer.Application.Features.Orders.GetAllOrders
{
    internal sealed class GetAllOrderQueryHandler : IRequestHandler<GetAllOrderQuery, Result<List<Order>>>
    {
        private readonly IOrderRepository _orderRepository;

        public GetAllOrderQueryHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<Result<List<Order>>> Handle(GetAllOrderQuery request, CancellationToken cancellationToken)
        {
            var orders = await _orderRepository.GetAll()
                .Include(p => p.Customer)
                .Include(p => p.Details!)
                .ThenInclude(p => p.Product)
                .OrderByDescending(p=>p.Date)
                .ToListAsync(cancellationToken);

            return orders;
        }
    }
}

