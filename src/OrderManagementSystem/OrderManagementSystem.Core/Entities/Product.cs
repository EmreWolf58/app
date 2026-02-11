using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementSystem.Core.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }= null!; //Ben biliyorum bu sonradan dolacak demektir, compiler’ı susturuyorum. (Runtime’da otomatik dolmaz; sen doldurmazsan hata alırsın.)
        public decimal Price { get; set; }
        public int Stock { get; set; }
    }
}
