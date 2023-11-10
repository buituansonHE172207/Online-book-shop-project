using BusinessObject;
using Data_Layer.Repository.Implementation;
using Data_Layer.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Ocsp;
using ShoppingWebApp.Models;

namespace ShoppingWebApp.Controllers
{
    public class GuestController : Controller
    {
        IProductRepository productRepository = null;
        ICategoryRepository categoryRepository = null;
        public GuestController()
        {
            productRepository = new ProductRepository();
            categoryRepository = new CategoryRepository();
        }
        [Authorize(Policy = "UserPolicy")]
        public IActionResult Index(int? categoryId, string? name)
        {   
            IEnumerable<Product> products = productRepository.GetProductList();
            if (categoryId != null)
            {
                products = products.Where(product => product.CategoryId.Equals(categoryId)).ToList();
            }
            if (!string.IsNullOrEmpty(name))  {
                products = products.Where(product => product.ProductName.ToLower().Contains(name.ToLower()));
            }
            IEnumerable<Category> listCategory = categoryRepository.GetCategoryList();
            GuestViewModel models = new GuestViewModel();
            models.Products = products.ToList();
            models.Categories = listCategory;
            if (TempData["CheckoutMessage"] != null)
            {
                ViewBag.Message = TempData["CheckoutMessage"];
                TempData.Remove("CheckoutMessage");
            }
            return View(models);
        }
    }
}


