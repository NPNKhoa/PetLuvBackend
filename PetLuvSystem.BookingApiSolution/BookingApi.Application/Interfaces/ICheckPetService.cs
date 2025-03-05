namespace BookingApi.Application.Interfaces
{
    public interface ICheckPetService
    {
        public Task<bool> CheckPetAsync(Guid petId);
    }
}
