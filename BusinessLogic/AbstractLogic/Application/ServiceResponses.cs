using DataTransferObjets.Dto.Response;
using System.Net;

public static class ServiceResponses
{
    public static ResponseDto<T> SuccessfulResponse200<T>(T responseData)
    {
        return new ResponseDto<T>() { StatusCode = HttpStatusCode.OK, Data = responseData };
    }
    public static ResponseDto<T> CreateResponse201<T>(T responseData)
    {
        return new ResponseDto<T>() { StatusCode = HttpStatusCode.Created, Data = responseData };
    }
    public static ResponseDto<T> NoContent204<T>()
    {
        return new ResponseDto<T>() { StatusCode = HttpStatusCode.NoContent };
    }
    public static ResponseDto<T> BadRequestResponse400<T>(string errorMessage)
    {
        return new ResponseDto<T>() { StatusCode = HttpStatusCode.BadRequest, Message = errorMessage };
    }
    public static ResponseDto<T> Unauthorized401<T>(string errorMessage)
    {
        return new ResponseDto<T>() { StatusCode = HttpStatusCode.Unauthorized, Message = errorMessage };
    }
    public static ResponseDto<T> Forbidden403<T>(string errorMessage)
    {
        return new ResponseDto<T>() { StatusCode = HttpStatusCode.Forbidden, Message = errorMessage };
    }
    public static ResponseDto<T> Conflict409<T>(string errorMessage)
    {
        return new ResponseDto<T>() { StatusCode = HttpStatusCode.Conflict, Message = errorMessage };
    }
    public static ResponseDto<T> NotFound404<T>(string errorMessage)
    {
        return new ResponseDto<T>() { StatusCode = HttpStatusCode.NotFound, Message = errorMessage };
    }
    public static ResponseDto<T> MultiStatus207<T>(string errorMessage)
    {
        return new ResponseDto<T>() { StatusCode = HttpStatusCode.MultiStatus, Message = errorMessage };
    }
}