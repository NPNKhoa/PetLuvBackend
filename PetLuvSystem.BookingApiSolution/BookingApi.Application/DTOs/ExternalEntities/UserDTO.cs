﻿namespace BookingApi.Application.DTOs.ExternalEntities
{
    public class UserDTO
    {
        public Guid UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
    }
}
