using AutoMapper;
using ERPServer.Domain.Entities;
using ERPServer.Domain.Repositories;
using GenericRepository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace ERPServer.Application.Features.Invoices.UpdateInvoice
{
    internal sealed class UpdateInvoiceCommandHandler(
        IInvoiceRepository invoiceRepository,
        IInvoiceDetailRepository invoiceDetailRepository,
        IStockMovementRepository stockMovementRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper) : IRequestHandler<UpdateInvoiceCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(UpdateInvoiceCommand request, CancellationToken cancellationToken)
        {
            var invoice = await invoiceRepository
                .WhereWithTracking(x => x.Id == request.Id)
                .Include(x=>x.Details)
                .FirstOrDefaultAsync(cancellationToken);

            if (invoice is null)
                return Result<string>.Failure("Fatura bulunamadı!");

            var movements = await stockMovementRepository
                .Where(x => x.InvoiceId == invoice.Id)
                .ToListAsync(cancellationToken);

            stockMovementRepository.DeleteRange(movements);
            invoiceDetailRepository.DeleteRange(invoice.Details);

            invoice.Details = request.Details.Select(x => new InvoiceDetail
            {
                InvoiceId = invoice.Id,
                DepotId = x.DepotId,
                ProductId = x.ProductId,
                Price = x.Price,
                Quantity = x.Quantity
            }).ToList();

            await invoiceDetailRepository.AddRangeAsync(invoice.Details, cancellationToken);

            mapper.Map(request, invoice);
            var newMovements = new List<StockMovement>();
            foreach (var item in request.Details)
            {
                var movement = new StockMovement
                {
                    InvoiceId = invoice.Id,
                    NumberOfEntries = invoice.InvoiceType.Value == 1 ? item.Quantity : 0,
                    NumberOfOutputs = invoice.InvoiceType.Value == 2 ? item.Quantity : 0,
                    DepotId = item.DepotId,
                    Price = item.Price,
                    ProductId = item.ProductId,
                };

                newMovements.Add(movement);
            }

            await stockMovementRepository.AddRangeAsync(newMovements, cancellationToken);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return "Fatura başarıyla güncellendi.";
        }
    }
}
