using PetApi.Application.DTOs.PetDTOs;
using PetApi.Domain.Entities;
using PetLuvSystem.SharedLibrary.Interfaces;
using PetLuvSystem.SharedLibrary.Responses;

namespace PetApi.Application.Interfaces
{
    public interface IPet : IGenericInterface<Pet>
    {
        public Task<Response> GetAllAsync(int? pageIndex, int? pageSize);
        public Task<Pet> FindById(Guid id, bool noTracking = false, bool includeOthers = false);
        public Task<Response> GetByUserIdAsync(Guid userId, int pageIndex = 1, int pageSize = 10);
        public Task<Response> UpadteImages(Guid petId, ICollection<string> imagePath);
        public Task<Response> DeleteImage(Guid petId, string imagePath);
        public Task<Response> UpdateFamAsync(Guid id, UpdatePetFamilyDTO entity);
    }
}
