using BookingApi.Application.DTOs.ExternalEntities;

namespace BookingApi.Application.Interfaces
{
    public interface IServiceService
    {
        public Task<ServiceVariantDTO?> GetServiceVariantById(Guid serviceId);
        public Task<ServiceVariantDTO?> GetServiceVariantByKey(Guid serviceId, Guid BreedId, string PetWeightRange);
        public Task<ServiceComboVariant?> GetServiceComboVariantById(Guid serviceId);
    }
}
