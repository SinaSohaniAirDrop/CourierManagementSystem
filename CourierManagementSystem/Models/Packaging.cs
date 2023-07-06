namespace CourierManagementSystem.Models
{
    public class Packaging
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Size { get; set; } = string.Empty;
        [Required]
        public double PackagingCost { get; set; } = 0;
    }
}
