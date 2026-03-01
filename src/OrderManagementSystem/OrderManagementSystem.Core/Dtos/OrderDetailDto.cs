using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementSystem.Core.Dtos
{
    public class OrderDetailDto
    {
        public int Id { get; set; }
        public string OrderNo { get; set; } = null!;
        public string CustomerName { get; set; }= null!;
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAtUtc { get; set; }
    }
}
