using System.Diagnostics.CodeAnalysis;

namespace DataTransferObjets.Dto.Response
{
    [ExcludeFromCodeCoverage]
    public class PropertyDto
    {
        public Guid IdProperty { get; set; }
        public required string Name { get; set; }
        public required string Address { get; set; }
        public required string City { get; set; }
        public required string State { get; set; }
        public required string ZipCode { get; set; }
        public required string Description { get; set; }
        public decimal Price { get; set; }
        public string ImageURL { get; set; }

        public required Guid OwnerId { get; set; }
        public required string OwnerName { get; set; }
    }
}
