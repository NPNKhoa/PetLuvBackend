namespace BookingApi.Application.DTOs.ExternalEntities
{
    public class RoomDTO
    {
        public Guid RoomId { get; set; }
        public string RoomName { get; set; }
        public decimal PricePerHour { get; set; }
        public decimal PricePerDay { get; set; }
        public bool IsVisible { get; set; }
    }
}
