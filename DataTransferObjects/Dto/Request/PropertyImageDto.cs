using System.Diagnostics.CodeAnalysis;

namespace DataTransferObjects.Dto.Request
{
    [ExcludeFromCodeCoverage]
    public class PropertyImageDto
    {
        public required string File { get; set; }
        public required bool Enabled { get; set; }
        public required Guid IdProperty { get; set; }
    }
}
