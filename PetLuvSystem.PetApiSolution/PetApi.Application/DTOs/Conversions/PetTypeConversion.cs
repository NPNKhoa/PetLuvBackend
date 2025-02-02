using PetApi.Application.DTOs.PetBreedDTOs;
using PetApi.Application.DTOs.PetTypeDTOs;
using PetApi.Domain.Entities;

namespace PetApi.Application.DTOs.Conversions
{
    public static class PetTypeConversion
    {
        public static PetType ToEntity(PetTypeDTO entity) => new()
        {
            PetTypeId = entity.PetTypeId,
            PetTypeName = entity.PetTypeName,
            PetTypeDesc = entity.PetTypeDesc,
            IsVisible = entity.IsVisible,
            PetBreeds = entity.PetBreeds.Select(x => new PetBreed
            {
                BreedId = x.BreedId,
                BreedName = x.BreedName,
                BreedDesc = x.BreedDesc,
                IllustrationImage = x.IllustrationImage,
                IsVisible = x.IsVisible
            }).ToList()
        };

        public static PetType ToEntity(CreateUpdatePetTypeDTO entity) => new()
        {
            PetTypeId = Guid.Empty,
            PetTypeName = entity.PetTypeName,
            PetTypeDesc = entity.PetTypeDesc,
            IsVisible = entity.IsVisible
        };

        public static (PetTypeDTO?, IEnumerable<PetTypeDTO>?) FromEntity(PetType? entity, IEnumerable<PetType>? entities)
        {
            if (entity is not null && entities is null)
            {
                var singleDTO = new PetTypeDTO
                (
                    entity.PetTypeId,
                    entity.PetTypeName,
                    entity.PetTypeDesc,
                    entity.IsVisible,
                    entity.PetBreeds?.Select(x => new BriefPetBreedDTO
                    (
                        x.BreedId,
                        x.BreedName,
                        x.BreedDesc,
                        x.IllustrationImage,
                        x.IsVisible,
                        entity.PetTypeName
                    ))!
                );

                return (singleDTO, null);
            }

            if (entities is not null && entity is null)
            {
                var multipleDTOs = entities.Select(x => new PetTypeDTO
                (
                    x.PetTypeId,
                    x.PetTypeName,
                    x.PetTypeDesc,
                    x.IsVisible,
                    x.PetBreeds?.Select(p => new BriefPetBreedDTO
                    (
                        p.BreedId,
                        p.BreedName,
                        p.BreedDesc,
                        p.IllustrationImage,
                        p.IsVisible,
                        x.PetTypeName
                    ))!
                ));

                return (null, multipleDTOs);
            }

            return (null, null);
        }
    }
}
