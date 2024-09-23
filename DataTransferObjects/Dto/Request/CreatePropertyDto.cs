using System.Diagnostics.CodeAnalysis;

namespace DataTransferObjets.Dto.Request
{
    [ExcludeFromCodeCoverage]
    public class CreatePropertyDto
    {
        public required string Name { get; set; }
        public required string Address { get; set; }
        public required string City { get; set; }
        public required string State { get; set; }
        public required string ZipCode { get; set; }
        public required string Description { get; set; }
        public required decimal Price { get; set; }
    }
}
