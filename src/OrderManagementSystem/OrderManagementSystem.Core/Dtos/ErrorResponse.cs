using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementSystem.Core.Dtos
{
    public class ErrorResponse
    {
        public string TraceId { get; set; } = null!;
        public string Message { get; set; } = null!;
        public int Status { get; set; }
    }
}
