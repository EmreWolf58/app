using OrderManagementSystem.Core.Dtos;

namespace OrderManagementSystem.Core.Services
{
    public class OrderPricingService
    {
        public decimal CalculateTotal(IEnumerable<OrderLineDto> lines)
        {
            if (lines == null) throw new ArgumentNullException(nameof(lines));

            decimal total = 0;

            foreach (var l in lines)
            {
                if (l.Quantity <= 0) throw new ArgumentException("Quantity must be > 0");
                if (l.UnitPrice < 0) throw new ArgumentException("UnitPrice must be >= 0");

                total += l.UnitPrice * l.Quantity;
            }

            return total;
        }
    }
}
