using System.ComponentModel.DataAnnotations;

namespace UserApi.Application.DTOs.AuthDTOs
{
    public record LoginDTO
    (
        [Required, EmailAddress]
        string Email,
        [Required, MinLength(8)]
        string Password
    );
}
