namespace ServiceApi.Application.Interfaces
{
    public interface IBreedMappingService
    {
        public Task<Dictionary<Guid, string>> GetBreedMappingAsync();
    }
}
