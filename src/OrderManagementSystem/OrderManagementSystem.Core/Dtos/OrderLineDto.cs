namespace OrderManagementSystem.Core.Dtos
{
    public class OrderLineDto
    {
        public string ProductName { get; set; } = null!;
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
    }
}