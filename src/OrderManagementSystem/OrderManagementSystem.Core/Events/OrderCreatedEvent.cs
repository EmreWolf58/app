namespace OrderManagementSystem.Core.Events;

public class OrderCreatedEvent
{
    public OrderCreatedEvent() { } // MassTransit için önemli

    public Guid OrderId { get; set; }
    public string CustomerEmail { get; set; } = "";
    public decimal TotalAmount { get; set; }
    public DateTime CreatedAtUtc { get; set; }
}