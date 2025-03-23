using BookingApi.Application.DTOs.ExternalEntities;

namespace BookingApi.Application.Interfaces
{
    public interface IServiceService
    {
        public Task<ServiceVariantDTO?> GetServiceVariantByKey(Guid serviceId, Guid BreedId, string PetWeightRange);
        public Task<ServiceComboVariant?> GetServiceComboVariantByKey(Guid serviceId, Guid BreedId, string PetWeightRange);
        public Task<Dictionary<Guid, string>> GetServiceMappings(List<Guid> serviceIds);
    }
}
