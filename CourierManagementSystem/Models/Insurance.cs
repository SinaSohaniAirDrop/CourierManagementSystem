namespace CourierManagementSystem.Models
{
    public class Insurance
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public double MinVal { get; set; } = 0;
        [Required]
        public double MaxVal { get; set; } = 0;
        [Required]
        public double Tariff { get; set; } = 0;
    }
}
