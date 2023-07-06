namespace CourierManagementSystem.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        public string Status { get; set; } = "Pending...";
        [ForeignKey("Package")]
        public int PackageId { get; set; }
        public Package? Package { get; set; }
        public double Cost { get; set; }
        public double Discount { get; set; }
    }
}
