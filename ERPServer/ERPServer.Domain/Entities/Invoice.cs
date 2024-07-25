using ERPServer.Domain.Abstractions;
using ERPServer.Domain.Enums;

namespace ERPServer.Domain.Entities
{
    public sealed class Invoice : Entity
    {
        public Guid CustomerId { get; set; }
        public Customer? Customer { get; set; }
        public string InvoiceNumber { get; set; } = string.Empty;
        public DateOnly Date { get; set; }
        public InvoiceType InvoiceType{ get; set; } = InvoiceType.Purchase;
        public List<InvoiceDetail>? Details { get; set; }
    }
}
