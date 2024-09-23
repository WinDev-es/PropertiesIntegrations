using AutoMapper;
using BusinessLogic.AbstractLogic.Domain;
using BusinessLogic.Contracts;
using Context.Entities;
using DataTransferObjects.Dto.Request;
using DataTransferObjects.Dto.Response;
using DataTransferObjects.Dto.System;
using DataTransferObjets.Configuration;
using DataTransferObjets.Dto.Response;
using DataTransferObjets.Entities;
using Microsoft.AspNetCore.Http;
using Repository.GenericRepository.Interfaces;

namespace BusinessLogic.ServicesLogic
{
    public class PropertyImageService : IPropertyImageService
    {
        private readonly IMapper mapper;
        private readonly IUnitOfWork unitOfWork;

        public PropertyImageService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
        }
        public async Task<ResponseDto<IEnumerable<DownloadImageDto>>> GetImagesByPropertyIdAsync(Guid propertyId, CancellationToken cancellationToken)
        {
            IEnumerable<PropertyImage?> data = await unitOfWork.PropertyImageRepository.ReadAll(cancellationToken, x => x.IdProperty.Equals(propertyId),
                                                                                                            includeProperties: StaticDefination.PropertyImageRelations);
            IEnumerable<DownloadImageDto> downloadImageDtos = DomainRulesServices.ConvertToDownloadImageDtos(data);

            return GenericValidation.HasRecords(downloadImageDtos) ?
                ServiceResponses.SuccessfulResponse200(downloadImageDtos) :
                ServiceResponses.NotFound404<IEnumerable<DownloadImageDto>>(StaticDefination.NoData);
        }

        public async Task<ResponseDto<bool>> UploadImageAsync(LoadImageDto requestDto, CancellationToken cancellationToken)
        {
            var results = await ProcessImagesAsync(requestDto.AddImageDto.Files,
                                                    requestDto.PropertyImageDto.IdProperty,
                                                    requestDto.PropertyImageDto.Enabled,
                                                    cancellationToken);

            bool success = results.Any(result => result.IsSuccess);
            bool hasErrors = results.Any(result => !result.IsSuccess);

            return success ? hasErrors
                    ? ServiceResponses.MultiStatus207<bool>(StaticDefination.WebDAVMessage)
                    : ServiceResponses.CreateResponse201(true) :
                    ServiceResponses.BadRequestResponse400<bool>(StaticDefination.NotImplemented);
        }

        private async Task<List<OperationResult>> ProcessImagesAsync(List<IFormFile> files, Guid propertyId, bool enabled, CancellationToken cancellationToken)
        {
            var results = new List<OperationResult>();

            await foreach (var loadImage in DomainRulesServices.UpLoadImages(files, propertyId))
            {
                var propertyImageEntity = new PropertyImage
                {
                    IdPropertyImage = Guid.NewGuid(),
                    File = loadImage.PropertyImageDto.File,
                    Img = loadImage.AddImageDto.Img.FirstOrDefault(),
                    Enabled = loadImage.PropertyImageDto.Enabled,
                    IdProperty = loadImage.PropertyImageDto.IdProperty
                };

                await unitOfWork.PropertyImageRepository.Create(propertyImageEntity, cancellationToken);
                await unitOfWork.SaveChangesAsync(cancellationToken);

                results.Add(new OperationResult { IsSuccess = true });
            }

            return results;
        }
    }
}
