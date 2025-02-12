using System.ComponentModel.DataAnnotations;

namespace UserApi.Application.DTOs.AuthDTOs
{
    public record ChangePasswordDTO
    (
        string? Email,
        [Required, MinLength(8)]
        string OldPassword,
        [Required, MinLength(8)]
        string NewPassword,
        [Required, MinLength(8)]
        string ConfirmPassword
    );
}
