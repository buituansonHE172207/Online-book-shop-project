using Data_Layer.Repository.Implementation;
using Data_Layer.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using ShoppingWebApp.Models;

namespace ShoppingWebApp.Controllers
{
    public class StatisticController : Controller 
    {
        IProductRepository productRepository;

        public StatisticController()
        {
            productRepository = new ProductRepository();
        }

        public ActionResult Index(DateTime? from, DateTime? to)
        {
            if (from == null) from = DateTime.Today;
            if (to == null) to = DateTime.Today;
            TempData["from"] = from;
            TempData["to"] = to;
            var productSales = productRepository
            .GetProductList()
            .Select(p => new StatisticModel{
                ProductName = p.ProductName,
                Quantity = p.OrderDetails
                            .Where(od => od.Order.CreatedDate >= from && od.Order.CreatedDate <= to)
                            .Sum(od => od.Quantity),
                Price = p.Price,
            })
            .Where(p => p.Quantity > 0);
            return View(productSales);
        }
    }
}