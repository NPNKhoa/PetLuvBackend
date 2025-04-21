using PetLuvSystem.SharedLibrary.Responses;

namespace BookingApi.Application.Interfaces
{
    public interface IStatistic
    {
        public Task<Response> GetServicesBookedAsync(DateTime? startDate, DateTime? endDate, int? month, int? year, string serviceType = "service");
        //public Task<Response> GetCombosBookedAsync(DateTime? startDate, DateTime? endDate, int? month, int? year);
        //public Task<Response> GetRoomsBookedAsync(DateTime? startDate, DateTime? endDate, int? month, int? year);
        public Task<Response> GetBreedsBookedAsync(DateTime? startDate, DateTime? endDate, int? month, int? year);
        public Task<Response> GetRevenue(DateTime? startDate, DateTime? endDate, int? month, int? year);
    }
}
