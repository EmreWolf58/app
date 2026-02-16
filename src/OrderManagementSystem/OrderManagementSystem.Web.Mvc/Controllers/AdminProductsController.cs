using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderManagementSystem.Core.Dtos;
using OrderManagementSystem.Web.Mvc.Models;
using OrderManagementSystem.Web.Mvc.Services;

namespace OrderManagementSystem.Web.Mvc.Controllers
{
    [Authorize(Roles ="Admin")]
    public class AdminProductsController : Controller
    {
        private readonly ApiClient _api;

        public AdminProductsController(ApiClient api)
        {
            _api = api;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var products = await _api.GetAsync<List<ProductDto>>("api/products");
                return View(products ?? new List<ProductDto>());
            }
            catch (UnauthorizedAccessException)
            {

                return RedirectToAction("Login", "Account");
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new ProductFormVm());
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductFormVm vm)
        {
            try
            {
                var req = new CreateProductRequest { Name = vm.Name, Price = vm.Price, Stock = vm.Stock };
                await _api.PostAsync<CreateProductRequest, ProductDto>("api/products", req);

                TempData["Success"] = "Ürün Yaratıldı.";
                return RedirectToAction(nameof(Index));

            }
            catch (UnauthorizedAccessException)
            {

                return RedirectToAction("Login", "Account");
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return View(vm);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var product = await _api.GetAsync<ProductDto>($"api/products/{id}");
                if (product == null)
                {
                    return NotFound();
                }
                var vm = new ProductFormVm { Id = product.Id, Name = product.Name, Price = product.Price, Stock = product.Stock };
                return View(vm);
            }
            catch (UnauthorizedAccessException)
            {
                return RedirectToAction("Login", "Account");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ProductFormVm vm)
        {
            if (!ModelState.IsValid) return View(vm);

            try
            {
                var req = new UpdateProductRequest { Name = vm.Name, Price = vm.Price, Stock = vm.Stock };
                await _api.PutAsync($"api/products/{vm.Id}",req);

                TempData["Success"] = "Product updated.";
                return RedirectToAction(nameof(Index));
            }
            catch (UnauthorizedAccessException)
            {
                return RedirectToAction("Login", "Account");
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return View(vm);
            }
        }
    }
}
