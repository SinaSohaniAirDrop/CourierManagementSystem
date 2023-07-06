namespace CourierManagementSystem.Models
{
    public class ComCost
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public double FixedCost { get; set; } = 0;
        [Required]
        public double tax { get; set; } = 0;
        [Required]
        public double HQCost { get; set; } = 0;
        [Required]
        public double InsiderFee { get; set; } = 0;
        [Required]
        public double OutsiderFee { get; set; } = 0;
    }
}
