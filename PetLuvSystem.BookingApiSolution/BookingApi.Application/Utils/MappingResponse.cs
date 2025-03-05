namespace BookingApi.Application.Utils
{
    public class MappingResponse<T>
    {
        public bool Flag { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public List<T> Data { get; set; }
    }
}
