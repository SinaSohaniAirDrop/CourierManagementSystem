namespace CourierManagementSystem.Models.Authentication.SignUp
{
    public class RegisterUser
    {
        [EmailAddress]
        [Required]
        public string? Email { get; set; } = string.Empty;
        [Required]
        public string? Username { get; set; } = string.Empty;
        [Required]
        public string? Password { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; } = string.Empty;
    }
}
