using AutoMapper;
using Context.Entities;
using DataTransferObjects.Dto.Request;
using DataTransferObjets.Configuration;
using DataTransferObjets.Dto.Request;
using DataTransferObjets.Dto.Response;
using DataTransferObjets.Entities;

namespace DataTransferObjets.Profiles
{
    public class PropertySystemProfile : Profile
    {
        public PropertySystemProfile()
        {
            CreateMap<CreatePropertyDto, Property>()
                .ForMember(o => o.IdOwner, b => b.MapFrom(z => Guid.Parse(StaticDefination.IdDefaultOwner)));

            CreateMap<Property, PropertyDto>()
                .ForMember(o => o.OwnerId, b => b.MapFrom(z => z.Owner.IdOwner))
                .ForMember(o => o.OwnerName, b => b.MapFrom(z => z.Owner.Name));
            
            CreateMap<PropertyDto, Property>();

            CreateMap<UpdatePropertyDto, Property>()
                .ForMember(o => o.IdOwner, b => b.MapFrom(z => Guid.Parse(StaticDefination.IdDefaultOwner)))
                .ForMember(o => o.IdProperty, x => x.Ignore());


            CreateMap<PropertyImageDto, PropertyImage>();

            CreateMap<PropertyImage, PropertyImageDto>();

            //CreateMap<UpdatePropertyDto, Context.Entities.Property>()
            //    .ForMember(o => o.IdProperty, x => x.Ignore());
        }
    }
}
