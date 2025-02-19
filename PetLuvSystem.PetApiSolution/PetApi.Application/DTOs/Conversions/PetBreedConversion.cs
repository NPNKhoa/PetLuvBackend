using PetApi.Application.DTOs.PetBreedDTOs;
using PetApi.Application.DTOs.PetDTOs;
using PetApi.Domain.Entities;

namespace PetApi.Application.DTOs.Conversions
{
    public static class PetBreedConversion
    {
        public static PetBreed ToEntity(PetBreedDTO dto) => new()
        {
            BreedId = dto.BreedId,
            BreedName = dto.BreedName,
            BreedDesc = dto.BreedDesc,
            IllustrationImage = dto.IllustrationImage,
            PetTypeId = dto.PetTypeId,
            IsVisible = dto.IsVisible
        };

        public static PetBreed ToEntity(CreateUpdatePetBreedDTO dto, string imagePath) => new()
        {
            BreedId = Guid.Empty,
            BreedName = dto.BreedName,
            BreedDesc = dto.BreedDesc,
            IllustrationImage = imagePath,
            PetTypeId = dto.PetTypeId,
            IsVisible = dto.IsVisible
        };

        public static (PetBreedDTO?, IEnumerable<PetBreedDTO>?) FromEntity(PetBreed? entity, IEnumerable<PetBreed>? entities)
        {
            if (entity is not null && entities is null)
            {
                var singleDTO = new PetBreedDTO
                (
                    entity.BreedId,
                    entity.BreedName,
                    entity.BreedDesc,
                    entity.IllustrationImage,
                    entity.IsVisible,
                    entity.PetTypeId,
                    entity?.PetType?.PetTypeName!,
                    entity?.Pets?.Select(x => new BriefPetDTO
                    (
                        x.PetId,
                        x.PetName,
                        string.Empty,
                        x.IsVisible
                    ))!
                );

                return (singleDTO, null);
            }

            if (entities is not null && entity is null)
            {
                var multipleDTOs = entities.Select(p => new PetBreedDTO
                (
                    p.BreedId,
                    p.BreedName,
                    p.BreedDesc,
                    p.IllustrationImage,
                    p.IsVisible,
                    p.PetTypeId,
                    p.PetType.PetTypeName,
                    p.Pets?.Select(x => new BriefPetDTO
                    (
                        x.PetId,
                        x.PetName,
                        string.Empty,
                        x.IsVisible
                    ))!
                ));

                return (null, multipleDTOs);
            }

            return (null, null);
        }
    }
}
