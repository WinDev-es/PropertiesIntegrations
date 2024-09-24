using DataTransferObjets.Dto.Request;
using DataTransferObjets.Dto.Response;

namespace BusinessLogic.Contracts
{
    public interface IPropertyService
    {
        #region Services
        public Task<ResponseDto<bool>> CreatePropertyAsync(CreatePropertyDto requestDto, CancellationToken cancellationToken);
        public Task<ResponseDto<bool>> ChangePriceAsync(Guid id, ChangePriceDto requestDto, CancellationToken cancellationToken);
        public Task<ResponseDto<bool>> UpdatePropertyAsync(Guid id, UpdatePropertyDto propertyDto, CancellationToken cancellationToken);
        public Task<ResponseDto<IEnumerable<PropertyDto>>> ListPropertiesAsync(PropertyFilterDto filterDto, CancellationToken cancellationToken);
        public Task<ResponseDto<PropertyDto>> GetPropertyByIdAsync(Guid id, CancellationToken cancellationToken);
        public Task<ResponseDto<bool>> DeleteAsync(Guid id, CancellationToken cancellationToken);
        #endregion
    }
}