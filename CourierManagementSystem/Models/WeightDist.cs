namespace CourierManagementSystem.Models
{
    public class WeightDist
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public double MinWeight { get; set; } = 0;
        [Required]
        public double MaxWeight { get; set; } = 0;
        [Required]
        public double NeighboringProvince { get; set; } = 0;
        [Required]
        public double OtherProvince { get; set; } = 0;
    }
}
