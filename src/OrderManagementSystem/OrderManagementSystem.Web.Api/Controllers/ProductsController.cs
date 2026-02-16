using Microsoft.AspNetCore.Mvc;
using OrderManagementSystem.Infrastructure.Data;
using OrderManagementSystem.Core.Entities;
using OrderManagementSystem.Core.Interface;
using OrderManagementSystem.Core.Dtos;
using Microsoft.AspNetCore.Authorization;

namespace OrderManagementSystem.Web.Api.Controllers
{
    [Authorize] //token yoksa 401 döner
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _service;

        public ProductsController(IProductService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _service.GetAllAsync());
        }

        [HttpPost]
        public async Task<IActionResult> Create (CreateProductRequest request)
        {
            var product = new Product
            {
                Name = request.Name,
                Price = request.Price,
                Stock = request.Stock
            };
            var created = await _service.CreateAsync(product);
            return CreatedAtAction(nameof(GetById), new {id =created.Id}, created);
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById (int id)
        {
            var product = await _service.GetByIdAsync(id);
            if (product == null) return NotFound();
            return Ok(product);
        }

        [Authorize(Roles ="Admin")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            //demo
            return Ok(new { message = $"Admin deleted product {id}" });
        }

        [Authorize]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update (int id, UpdateProductRequest request, [FromServices] AppDbContext db)
        {
            var product = await db.Products.FindAsync(id);
            if (product == null) NotFound();

            product.Name = request.Name;
            product.Price = request.Price;
            product.Stock = request.Stock;

            await db.SaveChangesAsync();

            return NoContent();
        }
    }
}
