using BusinessObject;

namespace ShoppingWebApp.Models
{
    public class GuestViewModel
    {
        public IEnumerable<Product> Products { get; set; }
        public IEnumerable<Category> Categories { get; set; }
    }
}
                                