using Data_Layer.Repository.Implementation;
using Data_Layer.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ShoppingWebApp.Models;

namespace ShoppingWebApp.Controllers
{
    public class CartController : Controller
    {
        IProductRepository productRepository = null;
        public CartController()
        {
            productRepository = new ProductRepository();
        }

        [Authorize(Policy = "UserPolicy")]
        public IActionResult Index()
        {

            var cart = HttpContext.Session.GetComplexData<List<CartItem>>("CART");
            List<CartItem> list = new List<CartItem>();
            if (cart != null)
            {
                list = (List<CartItem>)cart;
            }
            return View(list);
        }
        [Authorize(Roles = "User")]
        public IActionResult Checkout()
        {
            var cart = HttpContext.Session.GetComplexData<List<CartItem>>("CART");
            List<CartItem> list = new List<CartItem>();
            if (cart != null)
            {
                list = (List<CartItem>)cart;
            }
            return View(list);
        }

        //POST - ADD
        [Authorize(Policy = "UserPolicy")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(int ProductId, int Quantity)
        {
            var product = productRepository.GetProductById(ProductId);
            var cart = HttpContext.Session.GetComplexData<List<CartItem>>("CART");
            if (cart != null)
            {
                var list = (List<CartItem>)cart;
                if (list.Exists(i => i.ProductId == product.ProductId))
                {
                    foreach (var item in list)
                    {
                        if (item.ProductId == product.ProductId)
                        {
                            item.Quantity += Quantity;
                        }
                    }
                    HttpContext.Session.SetComplexData("CART", list);
                }

                else 
                {

                    var item = new CartItem();
                    item.ProductId = product.ProductId;
                    item.ProductName = product.ProductName;
                    item.Price = product.Price;
                    item.Image = product.Image;
                    item.Quantity = Quantity;
                    list.Add(item);
                    HttpContext.Session.SetComplexData("CART", list);
                }
            }
            else 
            {
                var item = new CartItem();
                item.ProductId = product.ProductId;
                item.ProductName = product.ProductName;
                item.Price = product.Price;
                item.Image = product.Image;
                item.Quantity = Quantity;
                var list = new List<CartItem>();
                list.Add(item);
                HttpContext.Session.SetComplexData("CART", list);
            }
            return NoContent();
        }

        //GET - REMOVE
        [Authorize(Policy = "UserPolicy")]
        public IActionResult Remove(int? ProductId)
        {
            if (ProductId == null)
            {
                return NotFound();
            }
            var product = productRepository.GetProductById((int)ProductId);
            if (product == null)
            {
                return NotFound();
            }
            var cart = HttpContext.Session.GetComplexData<List<CartItem>>("CART");
            List<CartItem> list = new List<CartItem>();
            if (cart != null)
            {
                list = (List<CartItem>)cart;
                if (list.Exists(x => x.ProductId == ProductId))
                {
                    foreach (var item in list)
                    {
                        if (item.ProductId == ProductId)
                        {
                            list.Remove(item);
                            break;
                        }
                    }
                }
                HttpContext.Session.SetComplexData("CART", list);
            }
            return RedirectToAction("Index");
        }
    }
}

#region ExtendSession
public static class SessionExtensions
{
    public static T GetComplexData<T>(this ISession session, string key)
    {
        var data = session.GetString(key);
        if (data == null)
        {
            return default(T);
        }
        return JsonConvert.DeserializeObject<T>(data);
    }

    public static void SetComplexData(this ISession session, string key, object value)
    {
        session.SetString(key, JsonConvert.SerializeObject(value));
    }
}

#endregion