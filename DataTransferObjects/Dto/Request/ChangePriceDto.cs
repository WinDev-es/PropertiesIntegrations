using System.Diagnostics.CodeAnalysis;

namespace DataTransferObjets.Dto.Request
{
    [ExcludeFromCodeCoverage]
    public class ChangePriceDto
    {
        public decimal NewPrice { get; set; }
    }
}
