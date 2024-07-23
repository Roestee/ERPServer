using ERPServer.Domain.Abstractions;

namespace ERPServer.Domain.Entities
{
    public sealed class Depot: Entity
    {
        public required string Name { get; set; }
        public required string City { get; set; }
        public required string Town { get; set; }
        public required string FullAddress { get; set; }
    }
}
