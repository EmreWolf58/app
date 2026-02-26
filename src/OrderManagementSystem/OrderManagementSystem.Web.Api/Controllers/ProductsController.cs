using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderManagementSystem.Core.Dtos;
using OrderManagementSystem.Core.Entities;
using OrderManagementSystem.Core.Interface;
using OrderManagementSystem.Infrastructure.Data;

namespace OrderManagementSystem.Web.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private const string ProductsVersionKey = "cache:products:version";

        private readonly IProductService _service;
        private readonly ICacheService _cache;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(IProductService service, ICacheService cache, ILogger<ProductsController> logger)
        {
            _service = service;
            _cache = cache;
            _logger = logger;
        }

        // ✅ GET /api/products  (TTL 60s cache)
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var version = await _cache.GetVersionAsync(ProductsVersionKey);
            var cacheKey = BuildProductsCacheKey(version);

            // Burada cache'lediğimiz tip: GetAllAsync ne dönüyorsa o.
            // Sende muhtemelen List<Product> dönüyor.
            var cached = await _cache.GetAsync<List<Product>>(cacheKey);

            if (cached != null)
            {
                _logger.LogInformation("CACHE HIT: {Key}", cacheKey);
                return Ok(cached);
            }

            _logger.LogInformation("CACHE MISS: {Key}", cacheKey);

            var result = await _service.GetAllAsync();

            await _cache.SetAsync(cacheKey, result, TimeSpan.FromSeconds(60));

            return Ok(result);
        }

        private static string BuildProductsCacheKey(long version)
            => $"cache:products:v{version}:all";

        [HttpPost]
        public async Task<IActionResult> Create(CreateProductRequest request)
        {
            var product = new Product
            {
                Name = request.Name,
                Price = request.Price,
                Stock = request.Stock
            };

            var created = await _service.CreateAsync(product);

            // ✅ Ürün listesi değişti -> cache versiyonunu artır
            await _cache.BumpVersionAsync(ProductsVersionKey);
            _logger.LogInformation("Products cache version bumped (CREATE).");

            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _service.GetByIdAsync(id);
            if (product == null) return NotFound();
            return Ok(product);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteAsync(id);
            if (!deleted) return NotFound();

            await _cache.BumpVersionAsync(ProductsVersionKey);
            _logger.LogInformation("Products cache version bumped (DELETE).");

            return NoContent();
        }

        [Authorize]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, UpdateProductRequest request, [FromServices] AppDbContext db)
        {
            var product = await db.Products.FindAsync(id);
            if (product == null) return NotFound();  // ✅ burada bug vardı (return yoktu)

            product.Name = request.Name;
            product.Price = request.Price;
            product.Stock = request.Stock;

            await db.SaveChangesAsync();

            // ✅ Ürün listesi değişti -> cache versiyonunu artır
            await _cache.BumpVersionAsync(ProductsVersionKey);
            _logger.LogInformation("Products cache version bumped (UPDATE).");

            return NoContent();
        }
    }
}