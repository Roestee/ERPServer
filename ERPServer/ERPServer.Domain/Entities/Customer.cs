using ERPServer.Domain.Abstractions;

namespace ERPServer.Domain.Entities
{
    public sealed class Customer: Entity
    {
        public required string Name { get; set; }
        public required string TaxDepartmant { get; set; }
        public required string TaxNumber { get; set; }
        public string? City { get; set; }
        public string? Town { get; set; }
        public string? FullAddress { get; set; }
    }
}
