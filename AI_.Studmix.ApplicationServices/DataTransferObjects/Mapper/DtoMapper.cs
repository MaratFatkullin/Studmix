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

        public static ContentFileDto Map(ContentFile file)
        {
            return AutoMapper.Mapper.Map<ContentFile, ContentFileDto>(file);
        }

        public static ContentPackageDto Map(ContentPackage package)
        {
            return AutoMapper.Mapper.Map<ContentPackage, ContentPackageDto>(package);
        }

        public static IEnumerable<ContentPackageDto> Map(IEnumerable<ContentPackage> packages)
        {
            return AutoMapper.Mapper.Map<IEnumerable<ContentPackage>, IEnumerable<ContentPackageDto>>(packages);
        }

        public static PropertyDto Map(Property property)
        {
            return AutoMapper.Mapper.Map<Property, PropertyDto>(property);
        }

        public static PropertyStateDto Map(PropertyState propertyState)
        {
            return AutoMapper.Mapper.Map<PropertyState, PropertyStateDto>(propertyState);
        }
    }
}