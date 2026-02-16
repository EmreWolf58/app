using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementSystem.Core.Dtos
{
    public class OrderListItemDto
    {
        public int Id { get; set; }
        public string OrderNo { get; set; } = null!;
        public string CustomerName { get; set; }= null!;
        public DateTime CreatedAtUtc { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = null!;
    }
}
