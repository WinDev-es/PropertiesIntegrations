using DataTransferObjects.Dto.Request;
using DataTransferObjects.Dto.Response;
using DataTransferObjets.Dto.Response;

namespace BusinessLogic.Contracts
{
    public interface IPropertyImageService
    {
        #region Services
        Task<ResponseDto<bool>> UploadImageAsync(LoadImageDto propertyImage, CancellationToken cancellationToken);
        Task<ResponseDto<IEnumerable<DownloadImageDto>>> GetImagesByPropertyIdAsync(Guid propertyId, CancellationToken cancellationToken);
        #endregion
    }
}
