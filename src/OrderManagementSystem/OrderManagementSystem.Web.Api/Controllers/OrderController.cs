using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderManagementSystem.Core.Dtos;
using OrderManagementSystem.Infrastructure.Data;

namespace OrderManagementSystem.Web.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles ="Admin")]
    public class OrderController : ControllerBase
    {
        private readonly AppDbContext _db;

        public OrderController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _db.Orders.OrderByDescending(x => x.Id).Select(x => new OrderListItemDto
            {
                Id = x.Id,
                OrderNo = x.OrderNo,
                CustomerName = x.CustomerName,
                TotalAmount = x.TotalAmount,
                Status = x.Status,
                CreatedAtUtc = x.CreatedAtUtc
            }).ToListAsync();

            return Ok(list);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById (int id)
        {
            var o = await _db.Orders.FindAsync(id);

            if (o == null) return NotFound();

            return Ok(new OrderDetailDto
            {
                Id = o.Id,
                OrderNo = o.OrderNo,
                CustomerName = o.CustomerName,
                TotalAmount = o.TotalAmount,
                Status = o.Status,
                CreatedAtUtc = o.CreatedAtUtc
            });
        }
    }
}
