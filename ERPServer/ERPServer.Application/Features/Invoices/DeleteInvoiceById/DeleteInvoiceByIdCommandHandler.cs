using ERPServer.Domain.Repositories;
using GenericRepository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace ERPServer.Application.Features.Invoices.DeleteInvoiceById
{
    internal sealed class DeleteInvoiceByIdCommandHandler(
        IInvoiceRepository invoiceRepository,
        IStockMovementRepository stockMovementRepository,
        IUnitOfWork unitOfWork) : IRequestHandler<DeleteInvoiceByIdCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(DeleteInvoiceByIdCommand request, CancellationToken cancellationToken)
        {
            var invoice = await invoiceRepository.GetByExpressionAsync(x => x.Id == request.Id);
            if(invoice is null)
                return Result<string>.Failure("Fatura bulunamadı!");

            var movements = await stockMovementRepository
                .Where(x => x.InvoiceId == invoice.Id)
                .ToListAsync(cancellationToken);

            stockMovementRepository.DeleteRange(movements);

            invoiceRepository.Delete(invoice);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            
            return "Fatura başarıyla silindi.";
        }
    }
}
