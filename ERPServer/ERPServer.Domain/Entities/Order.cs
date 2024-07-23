using ERPServer.Domain.Abstractions;
using ERPServer.Domain.Enums;

namespace ERPServer.Domain.Entities
{
    public sealed class Order: Entity
    {
        public Guid CustomerId { get; set; }
        public Customer? Customer { get; set; }
        public int OrderNumberYear { get; set; }
        public int OrderNumber { get; set; }
        public DateOnly Date { get; set; }
        public DateOnly DeliveryDate { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public List<OrderDetail>? Details { get; set; }
        public string Number => SetNumber();
    
        public string SetNumber()
        {
            var prefix = "RS";
            var initialString = prefix + OrderNumberYear + OrderNumber;
            var targetLength = 16;
            var missingLength = targetLength - initialString.Length;
            var finalString = prefix + OrderNumberYear + new string('0', missingLength) + OrderNumber;

            return finalString;
        }
    }
}
