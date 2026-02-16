using System.ComponentModel.DataAnnotations;

namespace OrderManagementSystem.Web.Mvc.Models
{
    public class ProductFormVm
    {
        public int? Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = null!;

        [Range(0.01, 999999)]
        public decimal Price { get; set; }

        [Range(0, 999999)]
        public int Stock { get; set; }
    }
}
