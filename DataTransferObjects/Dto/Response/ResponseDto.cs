using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace DataTransferObjets.Dto.Response
{
    public class ResponseDto<T>
    {
        public HttpStatusCode StatusCode { get; set; }
        public T Data { get; set; }
        public string Message { get; set; }

        public IActionResult ToActionResult()
        {
            return StatusCode switch
            {
                HttpStatusCode.OK => new OkObjectResult(this),
                HttpStatusCode.Created => new CreatedResult(string.Empty, this),
                HttpStatusCode.NoContent => new NoContentResult(),
                HttpStatusCode.BadRequest => new BadRequestObjectResult(this),
                HttpStatusCode.Unauthorized => new UnauthorizedObjectResult(this),
                HttpStatusCode.Forbidden => new ObjectResult(this) { StatusCode = 403 },
                HttpStatusCode.Conflict => new ConflictObjectResult(this),
                HttpStatusCode.NotFound => new NotFoundObjectResult(this),
                _ => new ObjectResult(this) { StatusCode = (int)StatusCode }
            };
        }
    }

    //public class ResponseDto<T>
    //{
    //    public List<string> Errors { get; set; }
    //    public string Message { get; set; }
    //    public HttpStatusCode StatusCode { get; set; }
    //    public T Data { get; set; }
    //}

}
