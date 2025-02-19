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
            MotherId = dto.MotherId,
            FatherId = dto.FatherId,
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
            MotherId = dto.MotherId,
            FatherId = dto.FatherId,
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
                    entity.PetDesc,
                    entity.PetFamilyRole,
                    entity.IsVisible,
                    entity.MotherId,
                    entity.FatherId,
                    entity.Mother is not null ? new BriefPetDTO(
                        entity.Mother.PetId,
                        entity.Mother.PetName,
                        entity.Mother.PetImagePaths?.First().PetImagePath,
                        entity.Mother.IsVisible
                    ) : null,
                   entity.Father is not null ? new BriefPetDTO(
                        entity.Father.PetId,
                        entity.Father.PetName,
                        entity.Father.PetImagePaths?.First().PetImagePath,
                        entity.Father.IsVisible
                    ) : null,
                    entity.BreedId,
                    entity.PetBreed is not null ? entity.PetBreed.BreedName : string.Empty,
                    entity.PetBreed?.PetType is not null ? entity.PetBreed.PetType.PetTypeName : string.Empty,
                    entity.CustomerId,
                    entity.PetImagePaths!,
                    entity.ChildrenFromMother is not null ? entity.ChildrenFromMother.Select(cm => new BriefPetDTO(
                        cm.PetId,
                        cm.PetName,
                        cm.PetImagePaths?.First().PetImagePath,
                        cm.IsVisible
                    )).ToList() : new List<BriefPetDTO>(),
                    entity?.ChildrenFromFather is not null ? entity.ChildrenFromFather.Select(cm => new BriefPetDTO(
                        cm.PetId,
                        cm.PetName,
                        cm.PetImagePaths?.First().PetImagePath,
                        cm.IsVisible
                    )).ToList() : new List<BriefPetDTO>()
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
                    p.PetDesc,
                    p.PetFamilyRole,
                    p.IsVisible,
                    p.MotherId,
                    p.FatherId,
                    p.Mother is not null ? new BriefPetDTO(
                        p.Mother.PetId,
                        p.Mother.PetName,
                        string.Empty,
                        p.Mother.IsVisible
                    ) : null,
                   p.Father is not null ? new BriefPetDTO(
                        p.Father.PetId,
                        p.Father.PetName,
                        string.Empty,
                        p.Father.IsVisible
                    ) : null,
                    p.BreedId,
                    p.PetBreed is not null ? p.PetBreed.BreedName : string.Empty,
                    p.PetBreed?.PetType is not null ? p.PetBreed.PetType.PetTypeName : string.Empty,
                    p.CustomerId,
                    p.PetImagePaths!,
                    p.ChildrenFromMother is not null ? p.ChildrenFromMother.Select(cm => new BriefPetDTO(
                        cm.PetId,
                        cm.PetName,
                        string.Empty,
                        cm.IsVisible
                    )).ToList() : new List<BriefPetDTO>(),
                    p?.ChildrenFromFather is not null ? p.ChildrenFromFather.Select(cm => new BriefPetDTO(
                        cm.PetId,
                        cm.PetName,
                        string.Empty,
                        cm.IsVisible
                    )).ToList() : new List<BriefPetDTO>()
                ));

                return (null, multipleDTOs);
            }

            return (null, null);
        }
    }
}
