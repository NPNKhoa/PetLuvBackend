namespace BookingApi.Application.Interfaces
{
    public interface ICheckPaymentStatusService
    {
        public Task<bool> CheckPaymentStatusAsync(Guid paymentStatusId);
        public Task<Guid> GetPaymentStatusIdByName(string paymentStatusName);
    }
}
