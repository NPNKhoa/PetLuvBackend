namespace BookingApi.Application.Interfaces
{
    public interface ICheckCustomerService
    {
        public Task<bool> CheckCustomerAsync(Guid customerId);
    }
}
