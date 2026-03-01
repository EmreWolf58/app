using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderManagementSystem.Core.Dtos;
using OrderManagementSystem.Core.Entities;
using OrderManagementSystem.Core.Events;
using OrderManagementSystem.Infrastructure.Data;

namespace OrderManagementSystem.Web.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles ="Admin")]
    public class OrderController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly IPublishEndpoint _publishEndpoint;

        public OrderController(AppDbContext db, IPublishEndpoint publishEndpoint)
        {
            _db = db;
            _publishEndpoint = publishEndpoint;
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

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] OrderCreateRequestDto req)
        {
            // 1) Order oluştur
            var order = new Order
            {
                OrderNo = req.OrderNo,
                CustomerName = req.CustomerName,
                TotalAmount = req.TotalAmount,
                Status = "Created",
                CreatedAtUtc = DateTime.UtcNow,
                CustomerEmail = req.CustomerEmail // entity'de varsa
            };

            // 2) DB’ye kaydet
            _db.Orders.Add(order);
            await _db.SaveChangesAsync(); // burada order.Id oluşur

            // 3) Event publish
            await _publishEndpoint.Publish(new OrderCreatedEvent
            {
                OrderId = Guid.NewGuid(),
                CustomerEmail = req.CustomerEmail,
                TotalAmount = order.TotalAmount,
                CreatedAtUtc = order.CreatedAtUtc
            });

            // 4) Response
            return CreatedAtAction(nameof(GetById), new { id = order.Id }, new { order.Id });
        }
    }
}
