using ERPServer.Domain.Entities;
using ERPServer.Domain.Enums;
using ERPServer.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace ERPServer.Application.Features.Invoices.GetAllInvoice
{
    internal sealed class GetAllInvoiceQueryHandler (
        IInvoiceRepository invoiceRepository): IRequestHandler<GetAllInvoiceQuery, Result<List<Invoice>>>
    {
        public async Task<Result<List<Invoice>>> Handle(GetAllInvoiceQuery request, CancellationToken cancellationToken)
        {
            var invoices = 
                await invoiceRepository
                .Where(x=>x.InvoiceType == InvoiceType.FromValue(request.InvoiceType))
                .Include(x=>x.Customer)
                .Include(x=>x.Details!)
                .ThenInclude(x=>x.Product)
                .Include(x=>x.Details!)
                .ThenInclude(x=>x.Depot)
                .OrderBy(x=>x.Date)
                .ToListAsync(cancellationToken);

            return invoices;    
        }
    }
}
