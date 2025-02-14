using PetApi.Application.DTOs.PetBreedDTOs;
using PetApi.Application.DTOs.PetDTOs;
using PetApi.Application.DTOs.SellingPetDTOs;
using PetApi.Domain.Entities;

namespace PetApi.Application.DTOs.Conversions
{
    public static class SellingPetConversion
    {
        public static SellingPet ToEntity(SellingPetDTO dto) => new()
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
        };

        public static SellingPet ToEntity(CreateUpdateSellingPetDTO dto) => new()
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
        };

        public static (SellingPetDTO?, IEnumerable<SellingPetDTO>?) FromEntity(SellingPet? entity, IEnumerable<SellingPet>? entities)
        {
            if (entity is not null && entities is null)
            {
                var singleDTO = new SellingPetDTO
                (
                    entity.PetId,
                    entity.PetName,
                    entity.PetDateOfBirth,
                    entity.PetGender,
                    entity.PetFurColor,
                    entity.PetWeight,
                    entity.PetDesc ?? string.Empty,
                    entity.PetFamilyRole ?? string.Empty,
                    entity.IsVisible,
                    entity.MotherId ?? null,
                    entity.FatherId ?? null,
                    entity.Mother is not null ? new BriefPetDTO
                    (
                        entity.Mother.PetId,
                        entity.Mother.PetName,
                        entity.Mother.IsVisible
                    ) : null,
                    entity.Father is not null ? new BriefPetDTO
                    (
                        entity.Father.PetId,
                        entity.Father.PetName,
                        entity.Father.IsVisible
                    ) : null,
                    entity.BreedId,
                    entity.PetBreed is not null ? new BriefPetBreedDTO
                    (
                        entity.PetBreed.BreedId,
                        entity.PetBreed.BreedName,
                        entity.PetBreed.BreedDesc,
                        entity.PetBreed.IllustrationImage,
                        entity.PetBreed.IsVisible,
                        entity.PetBreed?.PetType?.PetTypeName ?? string.Empty
                    ) : null!,
                    entity.PetImagePaths is not null ? entity.PetImagePaths.Select(pi => pi.PetImagePath).ToList() : null!,
                    entity.ChildrenFromMother is not null && entity.ChildrenFromMother.Count > 0
                        ? entity.ChildrenFromMother.Select(cp => new BriefPetDTO
                        (
                            cp.PetId,
                            cp.PetName,
                            cp.IsVisible
                        )).ToList() : null!,
                    entity.ChildrenFromFather is not null && entity.ChildrenFromFather.Count > 0
                        ? entity.ChildrenFromFather.Select(cp => new BriefPetDTO
                        (
                            cp.PetId,
                            cp.PetName,
                            cp.IsVisible
                        )).ToList() : null!,
                    entity.PetHealthBook
                );

                return (singleDTO, null);
            }

            if (entities is not null && entity is null)
            {
                var multiDtos = entities.Select(e => new SellingPetDTO
                    (
                        e.PetId,
                        e.PetName,
                        e.PetDateOfBirth,
                        e.PetGender,
                        e.PetFurColor,
                        e.PetWeight,
                        e.PetDesc ?? string.Empty,
                        e.PetFamilyRole ?? string.Empty,
                        e.IsVisible,
                        e.MotherId ?? null,
                        e.FatherId ?? null,
                        e.Mother is not null ? new BriefPetDTO
                        (
                            e.Mother.PetId,
                            e.Mother.PetName,
                            e.Mother.IsVisible
                        ) : null,
                        e.Father is not null ? new BriefPetDTO
                        (
                            e.Father.PetId,
                            e.Father.PetName,
                            e.Father.IsVisible
                        ) : null,
                        e.BreedId,
                        e.PetBreed is not null ? new BriefPetBreedDTO
                        (
                            e.PetBreed.BreedId,
                            e.PetBreed.BreedName,
                            e.PetBreed.BreedDesc,
                            e.PetBreed.IllustrationImage,
                            e.PetBreed.IsVisible,
                            e.PetBreed?.PetType?.PetTypeName ?? string.Empty
                        ) : null!,
                        e.PetImagePaths is not null ? e.PetImagePaths.Select(pi => pi.PetImagePath).ToList() : null!,
                        e.ChildrenFromMother is not null && e.ChildrenFromMother.Count > 0
                            ? e.ChildrenFromMother.Select(cp => new BriefPetDTO
                            (
                                cp.PetId,
                                cp.PetName,
                                cp.IsVisible
                            )).ToList() : null!,
                        e.ChildrenFromFather is not null && e.ChildrenFromFather.Count > 0
                            ? e.ChildrenFromFather.Select(cp => new BriefPetDTO
                            (
                                cp.PetId,
                                cp.PetName,
                                cp.IsVisible
                            )).ToList() : null!,
                        e.PetHealthBook
                    )
                ).ToList();

                return (null, multiDtos);
            }

            return (null, null);
        }
    }
}
