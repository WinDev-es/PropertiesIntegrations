using System.Diagnostics.CodeAnalysis;

namespace DataTransferObjets.Dto.Request
{
    [ExcludeFromCodeCoverage]
    public class PropertyFilterDto
    {
        public string? City { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
    }
}
