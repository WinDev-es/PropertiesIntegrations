using Microsoft.AspNetCore.Http;

namespace DataTransferObjets.Dto.Request
{
    public class AddImageDto
    {
        public string ImageUrl { get; set; }
        public List<byte[]>? Img { get; set; }
        public List<IFormFile> Files { get; set; } = new();
    }
}
