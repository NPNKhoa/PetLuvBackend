using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace UserApi.Application.DTOs.AuthDTOs
{
    public record RegisterDTO
    (
        [Required, EmailAddress]
        string Email,
        [Required, MinLength(8)]
        string Password,
        [Required]
        string FullName,
        bool Gender,
        DateTime? DateOfBirth,
        [Required, Phone]
        string PhoneNumber,
        string? Address,
        IFormFile? Avatar
    );
}
