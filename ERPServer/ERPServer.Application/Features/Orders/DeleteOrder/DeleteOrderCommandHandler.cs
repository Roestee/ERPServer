using ERPServer.Domain.Repositories;
using GenericRepository;
using MediatR;
using TS.Result;

namespace ERPServer.Application.Features.Orders.DeleteOrder
{
    internal sealed class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand, Result<string>>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteOrderCommandHandler(IUnitOfWork unitOfWork, IOrderRepository orderRepository)
        {
            _unitOfWork = unitOfWork;
            _orderRepository = orderRepository;
        }

        public async Task<Result<string>> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetByExpressionAsync(p => p.Id == request.Id, cancellationToken);

            if (order is null)
                return Result<string>.Failure("Sipariş bulunamadı!");

            _orderRepository.Delete(order);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return "Sipariş başarıyla silindi.";
        }
    }
}

