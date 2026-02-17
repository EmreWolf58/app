using OrderManagementSystem.Core.Dtos;
using OrderManagementSystem.Core.Services;
using Xunit;

namespace OrderManagementSystem.Tests.Unit
{
    public class OrderPricingServiceTests
    {
        [Fact]
        public void CalculateTotal_ShouldReturnSumOfUnitPriceTimesQuantity()
        {
            var svc = new OrderPricingService();

            var lines = new[]
            {
                new OrderLineDto { ProductName = "A", UnitPrice = 10m, Quantity = 2 }, // 20
                new OrderLineDto { ProductName = "B", UnitPrice = 5.5m, Quantity = 3 } // 16.5
            };

            var total = svc.CalculateTotal(lines);

            Assert.Equal(36.5m, total);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void CalculateTotal_ShouldThrow_WhenQuantityInvalid(int qty)
        {
            var svc = new OrderPricingService();
            var lines = new[]
            {
                new OrderLineDto { ProductName = "A", UnitPrice = 10m, Quantity = qty }
            };

            Assert.Throws<ArgumentException>(() => svc.CalculateTotal(lines));
        }
    }
}
