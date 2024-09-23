using DataTransferObjects.Dto.Request;
using DataTransferObjets.Dto.Request;
using System.Diagnostics.CodeAnalysis;

namespace DataTransferObjects.Dto.Response
{
    [ExcludeFromCodeCoverage]
    public class DownloadImageDto
    {
        public AddImageDto AddImageDto { get; set; }
        public PropertyImageDto PropertyImageDto { get; set; }
    }
}
