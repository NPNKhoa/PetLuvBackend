using PetApi.Application.DTOs.PetDTOs;
using PetApi.Domain.Entities;

namespace PetApi.Application.DTOs.Conversions
{
    public static class PetConversion
    {
        public static Pet ToEntity(PetDTO dto) => new()
        {
            PetId = dto.PetId,
            PetName = dto.PetName,
            PetDateOfBirth = dto.PetDateOfBirth,
            PetGender = dto.PetGender,
            PetFurColor = dto.PetFurColor,
            PetWeight = dto.PetWeight,
            PetDesc = dto.PetDesc,
            PetFamilyRole = dto.PetFamilyRole,
            IsVisible = dto.IsVisible,
            ParentPetId = dto.ParentPetId,
            BreedId = dto.BreedId,
            CustomerId = dto.CustomerId
        };

        public static Pet ToEntity(CreateUpdatePetDTO dto) => new()
        {
            PetId = Guid.Empty,
            PetName = dto.PetName,
            PetDateOfBirth = dto.PetDateOfBirth,
            PetGender = dto.PetGender,
            PetFurColor = dto.PetFurColor,
            PetWeight = dto.PetWeight,
            PetDesc = dto.PetDesc,
            PetFamilyRole = dto.PetFamilyRole,
            IsVisible = dto.IsVisible,
            ParentPetId = dto.ParentPetId,
            BreedId = dto.BreedId,
            CustomerId = dto.CustomerId
        };

        public static (PetDTO?, IEnumerable<PetDTO>?) FromEntity(Pet? entity, IEnumerable<Pet>? entities)
        {
            if (entity is not null && entities is null)
            {
                var singleDTO = new PetDTO
                (
                    entity.PetId,
                    entity.PetName,
                    entity.PetDateOfBirth,
                    entity.PetGender,
                    entity.PetFurColor,
                    entity.PetWeight,
                    entity.PetDesc!,
                    entity?.PetFamilyRole!,
                    entity.IsVisible,
                    entity?.ParentPetId,
                    null,
                    entity.BreedId,
                    entity?.PetBreed?.BreedName!,
                    entity.CustomerId,
                    entity.PetImagePaths,
                    entity?.ChildrenPets,
                    entity?.PetHealthBooks
                );

                return (singleDTO, null);
            }

            if (entities is not null && entity is null)
            {
                var multipleDTOs = entities.Select(p => new PetDTO
                (
                    p.PetId,
                    p.PetName,
                    p.PetDateOfBirth,
                    p.PetGender,
                    p.PetFurColor,
                    p.PetWeight,
                    p.PetDesc!,
                    p.PetFamilyRole!,
                    p.IsVisible,
                    p?.ParentPetId,
                    null,
                    p.BreedId,
                    p?.PetBreed?.BreedName!,
                    p.CustomerId,
                    p.PetImagePaths,
                    p?.ChildrenPets,
                    p?.PetHealthBooks
                ));

                return (null, multipleDTOs);
            }

            return (null, null);
        }
    }
}
