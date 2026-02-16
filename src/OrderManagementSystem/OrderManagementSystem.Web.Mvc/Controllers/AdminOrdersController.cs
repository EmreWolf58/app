using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderManagementSystem.Core.Dtos;
using OrderManagementSystem.Web.Mvc.Services;

namespace OrderManagementSystem.Web.Mvc.Controllers
{
    [Authorize(Roles ="Admin")]
    public class AdminOrdersController:Controller
    {
        private readonly ApiClient _api;

        public AdminOrdersController(ApiClient api)
        {
            _api = api;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var orders = await _api.GetAsync<List<OrderListItemDto>>("api/order");
                return View(orders ?? new List<OrderListItemDto>());
            }
            catch (UnauthorizedAccessException)
            {
                return RedirectToAction("Login", "Account");
            }
        }

        public async Task<IActionResult> Detail(int id)
        {
            try
            {
                var order = await _api.GetAsync<OrderDetailDto>($"api/order/{id}");
                if (order == null) return NotFound();
                return View(order);
            }
            catch (UnauthorizedAccessException)
            {
                return RedirectToAction("Login", "Account");
            }
        }
    }
}
