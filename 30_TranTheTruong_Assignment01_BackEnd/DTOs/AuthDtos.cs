using System.ComponentModel.DataAnnotations;

namespace _30_TranTheTruong_Assignment01_BackEnd.DTOs
{
    public class AuthDtos
    {
    }
    public class LoginRequest
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Email format is invalid.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; } = string.Empty;
    }

    public class LoginResponse
    {
        public string Token { get; set; } = string.Empty;
        public short AccountId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }

    public class ProfileRequest
    {
        [Required(ErrorMessage = "Account name is required.")]
        [StringLength(100, ErrorMessage = "Account name must be at most 100 characters.")]
        public string? AccountName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Email format is invalid.")]
        [StringLength(70, ErrorMessage = "Email must be at most 70 characters.")]
        public string? AccountEmail { get; set; }

        [StringLength(70, ErrorMessage = "Password must be at most 70 characters.")]
        public string? NewPassword { get; set; }
    }

    public class ProfileResponse
    {
        public short AccountId { get; set; }
        public string? AccountName { get; set; }
        public string? AccountEmail { get; set; }
    }

}
