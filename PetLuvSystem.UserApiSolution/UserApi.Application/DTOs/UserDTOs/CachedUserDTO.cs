namespace UserApi.Application.DTOs.UserDTOs
{
    public class CachedUserDTO
    {
        public Guid UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
    }
}
