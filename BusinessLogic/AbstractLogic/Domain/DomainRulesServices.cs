using Context.Entities;
using DataTransferObjects.Dto.Request;
using DataTransferObjects.Dto.Response;
using DataTransferObjets.Dto.Request;
using DataTransferObjets.Dto.Response;
using Microsoft.AspNetCore.Http;

namespace BusinessLogic.AbstractLogic.Domain
{
    public static class DomainRulesServices
    {
        public static async Task<bool> ValidatePropertyNameIdIntoSystem(IEnumerable<PropertyDto> properties, Guid id, string name)
        {
            return GenericValidation
                .ValidateDuplicateNameField(properties, id, name);
        }
        public static IEnumerable<DownloadImageDto> ConvertToDownloadImageDtos(IEnumerable<PropertyImage> propertyImages)
        {
            return propertyImages
                .Where(image => image != null)
                .Select(image => new DownloadImageDto
                {
                    AddImageDto = new AddImageDto
                    {
                        Img = new List<byte[]> { image.Img },
                        ImageUrl = image.File
                    },
                    PropertyImageDto = new PropertyImageDto
                    {
                        File = image.File,
                        Enabled = image.Enabled,
                        IdProperty = image.IdProperty
                    }
                });
        }
        public async static IAsyncEnumerable<LoadImageDto> UpLoadImages(List<IFormFile> files, Guid propertyId)
        {
            if (files is not null && files.Any())
            {
                foreach (var file in files)
                {
                    if (file.Length > 0)
                    {
                        var fileBytes = await ProcessFileAsync(file);
                        var addImageDto = CreateAddImageDto(file, fileBytes);
                        var propertyImageDto = CreatePropertyImageDto(propertyId, true, file.FileName);
                        var loadImageDto = CombineDtos(addImageDto, propertyImageDto);

                        yield return loadImageDto;
                    }
                }
            }
        }

        #region private services
        private static async Task<byte[]> ProcessFileAsync(IFormFile file)
        {
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                return memoryStream.ToArray();
            }
        }

        private static AddImageDto CreateAddImageDto(IFormFile file, byte[] fileBytes)
        {
            return new AddImageDto
            {
                ImageUrl = $"https://yourstoragepath/{file.FileName}", // Simula la URL de almacenamiento Cloud (Mejor práctica) Primera opcion
                Img = new List<byte[]> { fileBytes }, // Simula la URL de almacenamiento en binario - Segunda opcion
                Files = new List<IFormFile> { file } // Simula la URL de almacenamiento original - Tercera opcion
            };
        }

        private static PropertyImageDto CreatePropertyImageDto(Guid propertyId, bool enabled, string fileName)
        {
            return new PropertyImageDto
            {
                IdProperty = propertyId,
                Enabled = enabled,
                File = fileName
            };
        }
        private static LoadImageDto CombineDtos(AddImageDto addImageDto, PropertyImageDto propertyImageDto)
        {
            return new LoadImageDto
            {
                AddImageDto = addImageDto,
                PropertyImageDto = propertyImageDto
            };
        }
        #endregion
    }
}
