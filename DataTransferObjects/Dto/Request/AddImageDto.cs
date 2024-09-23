using Microsoft.AspNetCore.Http;
using System.Diagnostics.CodeAnalysis;

namespace DataTransferObjets.Dto.Request
{
    [ExcludeFromCodeCoverage]
    public class AddImageDto
    {
        public string ImageUrl { get; set; }
        public List<byte[]>? Img { get; set; }
        public List<IFormFile> Files { get; set; } = new();
    }
}
