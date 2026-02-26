namespace OrderManagementSystem.Core.Dtos
{
    public class ProductQuery
    {
        public int Page { get; set; } = 1;
        public int PageSizeq { get; set; } = 20;
        public string? Search { get; set; }
        public string? Sort { get; set; } //name, price gibi alanlara göre sıralama yapılacak. "name_asc", "price_desc" gibi değerler beklenebilir.
    }
}
