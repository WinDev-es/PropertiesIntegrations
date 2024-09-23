using AutoMapper;
using BusinessLogic.AbstractLogic.Domain;
using BusinessLogic.Contracts;
using DataTransferObjets.Configuration;
using DataTransferObjets.Dto.Request;
using DataTransferObjets.Dto.Response;
using DataTransferObjets.Entities;

//using DataTransferObjets.Entities;
using Repository.GenericRepository.Interfaces;

namespace BusinessLogic.ServicesLogic
{
    public class PropertyService : IPropertyService
    {
        private readonly IMapper mapper;
        private readonly IUnitOfWork unitOfWork;

        public PropertyService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
        }
        public async Task<ResponseDto<bool>> CreatePropertyAsync(CreatePropertyDto requestDto, CancellationToken cancellationToken)
        {
            Property entityPropertyNew = mapper.Map<Property>(requestDto);
            IEnumerable<Property?> data = await unitOfWork.PropertyRepository.ReadAll(cancellationToken, includeProperties: StaticDefination.PropertyRelations);
            IEnumerable<PropertyDto> responseDb = mapper.Map<IEnumerable<PropertyDto>>(data);

            if (GenericValidation.IsGreaterThanZero<bool>(await DomainRulesServices.ValidatePropertyNameIdIntoSystem(
                                                            responseDb, entityPropertyNew.IdProperty, entityPropertyNew.Name)))
                return ServiceResponses.Conflict409<bool>(string.Concat(StaticDefination.ConflictMessage, StaticDefination.Separator, StaticDefination.DuplicateRegistration));

            await unitOfWork.PropertyRepository.Create(entityPropertyNew, cancellationToken);
            return GenericValidation.IsGreaterThanZero<bool>(await unitOfWork.SaveChangesAsync(cancellationToken) > 0) ?
                        ServiceResponses.CreateResponse201<bool>(true) :
                        ServiceResponses.BadRequestResponse400<bool>(StaticDefination.NotImplemented);
        }

        public async Task<ResponseDto<PropertyDto>> GetPropertyByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            Property? entity = await unitOfWork.PropertyRepository.ReadById(cancellationToken, x => x.IdProperty.Equals(id), includeProperties: StaticDefination.PropertyRelations);
            PropertyDto responseDto = mapper.Map<PropertyDto>(entity);

            return GenericValidation.IsNotNull(responseDto) ?
                ServiceResponses.SuccessfulResponse200(responseDto) :
                ServiceResponses.NotFound404<PropertyDto>(StaticDefination.DataNotFound);
        }

        public async Task<ResponseDto<bool>> UpdatePropertyAsync(Guid id, UpdatePropertyDto propertyDto, CancellationToken cancellationToken)
        {
            Property entityPropertyUpd = mapper.Map<Property>(propertyDto);
            entityPropertyUpd.IdProperty = id;
            IEnumerable<Property?> data = await unitOfWork.PropertyRepository.ReadAll(cancellationToken, includeProperties: StaticDefination.PropertyRelations);
            IEnumerable<PropertyDto> responseDb = mapper.Map<IEnumerable<PropertyDto>>(data);

            if (GenericValidation.IsGreaterThanZero<bool>(await DomainRulesServices.ValidatePropertyNameIdIntoSystem(
                                                            responseDb, entityPropertyUpd.IdProperty, entityPropertyUpd.Name)))
                return ServiceResponses.Conflict409<bool>(string.Concat(StaticDefination.ConflictMessage, StaticDefination.Separator, StaticDefination.DuplicateRegistration));

            await unitOfWork.PropertyRepository.Update(id, entityPropertyUpd, cancellationToken);
            return GenericValidation.IsGreaterThanZero<bool>(await unitOfWork.SaveChangesAsync(cancellationToken) > 0) ?
                        ServiceResponses.SuccessfulResponse200<bool>(true) :
                        ServiceResponses.BadRequestResponse400<bool>(StaticDefination.NotImplemented);
        }

        public async Task<ResponseDto<IEnumerable<PropertyDto>>> ListPropertiesAsync(PropertyFilterDto filterDto, CancellationToken cancellationToken)
        {
            decimal minPrice = filterDto.MinPrice ?? 0;
            decimal maxPrice = filterDto.MaxPrice ?? 0;

            IEnumerable<Property?> data = await unitOfWork.PropertyRepository.ReadAll(cancellationToken, x => (string.IsNullOrEmpty(filterDto.City) || x.City.Contains(filterDto.City)) &&
                                                                                        (x.Price >= minPrice && (x.Price <= maxPrice) || minPrice == maxPrice) ||
                                                                                        (string.IsNullOrEmpty(filterDto.City) && x.Price >= minPrice && maxPrice == 0),
                                                                                        includeProperties: StaticDefination.PropertyRelations);

            IEnumerable<PropertyDto> response = mapper.Map<IEnumerable<PropertyDto>>(data);

            return GenericValidation.HasRecords(response) ?
                ServiceResponses.SuccessfulResponse200(response) :
                ServiceResponses.NotFound404<IEnumerable<PropertyDto>>(StaticDefination.NoData);
        }

        public async Task<ResponseDto<bool>> ChangePriceAsync(Guid id, ChangePriceDto requestDto, CancellationToken cancellationToken)
        {
            Property? entityDb = await unitOfWork.PropertyRepository.ReadById(cancellationToken, x => x.IdProperty.Equals(id), includeProperties: StaticDefination.PropertyRelations);
            if (GenericValidation.IsNotNull(entityDb))
            {
                if (requestDto.NewPrice >= 0)
                {
                    entityDb.Price = requestDto.NewPrice;
                    await unitOfWork.PropertyRepository.Update(id, entityDb, cancellationToken);
                    return GenericValidation.IsGreaterThanZero<bool>(await unitOfWork.SaveChangesAsync(cancellationToken) > 0) ?
                                ServiceResponses.NoContent204<bool>(string.Empty) :
                                ServiceResponses.BadRequestResponse400<bool>(StaticDefination.NotImplemented);

                }
                return ServiceResponses.Conflict409<bool>(StaticDefination.IsNotGreaterThanZero);
            }
            return ServiceResponses.NotFound404<bool>(StaticDefination.NotFoundMessage);
        }

        public async Task<ResponseDto<bool>> Delete(Guid id, CancellationToken cancellationToken)
        {
            await unitOfWork.PropertyRepository.Delete(id, cancellationToken);
            return GenericValidation.IsGreaterThanZero<bool>(await unitOfWork.SaveChangesAsync(cancellationToken) > 0) ?
                        ServiceResponses.SuccessfulResponse200<bool>(true) :
                        ServiceResponses.BadRequestResponse400<bool>(StaticDefination.NotImplemented);
        }
    }
}
