using PetLuvSystem.SharedLibrary.Interfaces;
using PetLuvSystem.SharedLibrary.Responses;
using ServiceApi.Domain.Entities;

namespace ServiceApi.Application.Interfaces
{
    public interface IServiceComboVariant : IGenericInterface<ServiceComboVariant>
    {
        public Task<ServiceComboVariant> FindByKey(Guid serviceComboId, Guid breedId, string WeightRange);
        public Task<Response> GetByKeyAsync(Guid serviceComboId, Guid breedId, string WeightRange);
        public Task<Response> UpdateAsync(Guid serviceComboId, Guid breedId, string WeightRange, decimal comboPrice);
        public Task<Response> DeleteAsync(Guid serviceComboId, Guid breedId, string WeightRange);
        public Task<Response> GetByServiceComboAsync(Guid serviceComboId);
    }
}
