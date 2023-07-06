namespace CourierManagementSystem.Models
{
    public class Request
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Type { get; set; } = "Suggestion";
        public string? Text { get; set; }
        public DateTime RegistrationDate { get; set; } = DateTime.Now;
        public bool IsDone { get; set; } = false;
        [ForeignKey("AspNetUsers")]
        public string? UserId { get; set; }
        public IdentityUser? User { get; set; }
    }
}
