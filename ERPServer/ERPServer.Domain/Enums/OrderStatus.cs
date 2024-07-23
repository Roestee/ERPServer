using Ardalis.SmartEnum;

namespace ERPServer.Domain.Enums
{
    public sealed class OrderStatus : SmartEnum<OrderStatus>
    {
        public static readonly OrderStatus Pending = new("Bekliyor", 1);
        public static readonly OrderStatus RequirementsPlanWorked = new("İhtiyaç Planı Çalışıldı", 2);
        public static readonly OrderStatus Completed = new("Tamamlandı", 3);

        public OrderStatus(string name, int value) : base(name, value)
        {
        }
    }
}
