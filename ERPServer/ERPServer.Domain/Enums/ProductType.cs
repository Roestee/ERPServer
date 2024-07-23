using Ardalis.SmartEnum;

namespace ERPServer.Domain.Enums
{
    public sealed class ProductType : SmartEnum<ProductType>
    {
        public static readonly ProductType Product = new("Mamül", 1);
        public static readonly ProductType SemiProduct = new("Yarı Mamül", 2);

        public ProductType(string name, int value) : base(name, value)
        {
        }
    }
}
