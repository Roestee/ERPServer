using AutoMapper;
using ERPServer.Domain.Entities;
using ERPServer.Domain.Repositories;
using GenericRepository;
using MediatR;
using TS.Result;

namespace ERPServer.Application.Features.Invoices.CreateInvoice
{
    internal sealed class CreateInvoiceCommandHandler (
        IInvoiceRepository invoiceRepository,
        IStockMovementRepository stockMovementRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper): IRequestHandler<CreateInvoiceCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(CreateInvoiceCommand request, CancellationToken cancellationToken)
        {   
            var invoice = mapper.Map<Invoice>(request);
            if (invoice.Details is not null)
            {
                var movements = new List<StockMovement>();
                foreach (var item in invoice.Details)
                {
                    var movement = new StockMovement
                    {
                        InvoiceId = invoice.Id,
                        NumberOfEntries = request.InvoiceTypeValue == 1 ? item.Quantity : 0,
                        NumberOfOutputs = request.InvoiceTypeValue == 2 ? item.Quantity : 0,
                        DepotId = item.DepotId,
                        Price = item.Price,
                        ProductId = item.ProductId,
                    };

                    movements.Add(movement);
                }

                await stockMovementRepository.AddRangeAsync(movements, cancellationToken);
            }

            await invoiceRepository.AddAsync(invoice, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return "Fatura başarıyla eklendi.";
        }
    }
}
