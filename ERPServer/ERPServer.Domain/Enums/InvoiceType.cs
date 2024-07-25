using Ardalis.SmartEnum;

namespace ERPServer.Domain.Enums
{
    public sealed class InvoiceType : SmartEnum<InvoiceType>
    {
        public static readonly InvoiceType Purchase = new("Alış Faturası", 1);
        public static readonly InvoiceType Sales = new("Satış Faturası", 2);
        public InvoiceType(string name, int value) : base(name, value)
        {
        }
    }
}
