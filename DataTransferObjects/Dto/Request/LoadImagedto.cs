using DataTransferObjets.Dto.Request;
using System.Diagnostics.CodeAnalysis;

namespace DataTransferObjects.Dto.Request
{
    [ExcludeFromCodeCoverage]
    public class LoadImageDto
    {
        public AddImageDto AddImageDto { get; set; }
        public PropertyImageDto PropertyImageDto { get; set; }
    }
}