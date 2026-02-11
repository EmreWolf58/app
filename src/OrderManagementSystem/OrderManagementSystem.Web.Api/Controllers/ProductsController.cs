using Microsoft.AspNetCore.Mvc;
using OrderManagementSystem.Infrastructure.Data;
using OrderManagementSystem.Core.Entities;
using OrderManagementSystem.Core.Interface;
using OrderManagementSystem.Core.Dtos;

namespace OrderManagementSystem.Web.Api.Controllers
{
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
    }
}
