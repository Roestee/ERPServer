using AutoMapper;
using ERPServer.Domain.Entities;
using ERPServer.Domain.Repositories;
using GenericRepository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace ERPServer.Application.Features.Orders.UpdateOrder
{
    internal sealed class UpdateOrderCommandHandler(
        IOrderRepository orderRepository,
        IOrderDetailRepository orderDetailRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper) : IRequestHandler<UpdateOrderCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await orderRepository
                .Where(x => x.Id == request.Id)
                .Include(x=>x.Details)
                .FirstOrDefaultAsync(cancellationToken);
            if (order is null)
                return Result<string>.Failure("Sipariş bulunamadı!");

            orderDetailRepository.DeleteRange(order.Details);
            var newDetails = request.Details.Select(x=> new OrderDetail
            {
                OrderId = order.Id,
                UnitPrice = x.UnitPrice,
                Quantity = x.Quantity,  
                ProductId = x.ProductId,
            }).ToList();

            await orderDetailRepository.AddRangeAsync(newDetails, cancellationToken);

            mapper.Map(request, order);
            orderRepository.Update(order);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return "Sipariş başarıyla güncellendi.";
        }
    }
}

