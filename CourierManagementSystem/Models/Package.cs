namespace CourierManagementSystem.Models
{
    public class Package
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public double Weight { get; set; } = 0;
        [Required]
        public string Size { get; set; } = string.Empty;
        [Required]
        public double Value { get; set; } = 0;
        [Required]
        public DateTime PickupDate { get; set; } = DateTime.Now;
        [Required]
        public DateTime DeliveryDate { get; set; } = DateTime.Now;
        [Required]
        public string PickupCity { get; set; } = string.Empty;
        [Required]
        public string DeliveryCity { get; set; } = string.Empty;
        [Required]
        public string PickupLocation { get; set; } = string.Empty;
        [Required]
        public string DeliveryLocation { get; set; } = string.Empty;
        [Required]
        public bool IsNeighboringCity { get; set; } = false;
        [ForeignKey("AspNetUsers")]
        public string? SenderId { get; set; }
        public IdentityUser? Sender { get; set; }
        [ForeignKey("AspNetUsers")]
        public string? ReceiverId { get; set; }
        public IdentityUser? Receiver { get; set; }
    }
}
