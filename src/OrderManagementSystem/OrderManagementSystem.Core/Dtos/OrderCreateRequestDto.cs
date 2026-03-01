namespace OrderManagementSystem.Core.Dtos
{
    public class OrderCreateRequestDto
    {
        public string OrderNo { get; set; } = default!;
        public string CustomerName { get; set; } = default!;
        public string CustomerEmail { get; set; } = default!;
        public decimal TotalAmount { get; set; }
    }
}
