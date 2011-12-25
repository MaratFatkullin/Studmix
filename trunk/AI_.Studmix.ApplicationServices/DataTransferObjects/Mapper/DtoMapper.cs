using System.Collections.Generic;
using AI_.Studmix.Domain.Entities;

namespace AI_.Studmix.ApplicationServices.DataTransferObjects.Mapper
{
    public static class DtoMapper
    {
        static DtoMapper()
        {
            AutoMapper.Mapper.CreateMap<ContentFile, ContentFileDto>()
                .ConstructUsing(f => new ContentFileDto(f.ID, f.Name, f.IsPreview));

            AutoMapper.Mapper.CreateMap<ContentPackage, ContentPackageDto>();

            AutoMapper.Mapper.CreateMap<Property, PropertyDto>();

            AutoMapper.Mapper.CreateMap<PropertyState, PropertyStateDto>()
                .ConstructUsing(f => new PropertyStateDto(f.Property.ID, f.Value));
        }

        public static TDestionation Map<TDestionation>(object source)
        {
            return AutoMapper.Mapper.Map<TDestionation>(source);
        }

        public static IEnumerable<TDestionation> MapSequence<TDestionation>(IEnumerable<object> source)
        {
            return AutoMapper.Mapper.Map<IEnumerable<TDestionation>>(source);
        }
    }
}