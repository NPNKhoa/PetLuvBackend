using PetApi.Application.DTOs.PetBreedDTOs;
using PetApi.Domain.Entities;

namespace PetApi.Application.DTOs.Conversions
{
    public static class BreedMappingConversion
    {
        public static (BreedMappingDTO?, IEnumerable<BreedMappingDTO>?) FromEntity(PetBreed? entity, IEnumerable<PetBreed>? entities)
        {
            if (entity is not null && entities is null)
            {
                var singleDTO = new BreedMappingDTO
                (
                    entity.BreedId,
                    entity.BreedName
                );

                return (singleDTO, null);
            }

            if (entities is not null && entity is null)
            {
                var multipleDTOs = entities.Select(p => new BreedMappingDTO
                (
                    p.BreedId,
                    p.BreedName
                ));

                return (null, multipleDTOs);
            }

            return (null, null);
        }
    }
}
