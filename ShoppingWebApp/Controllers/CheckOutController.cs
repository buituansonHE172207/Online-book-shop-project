using BusinessObject;
using Data_Layer.Repository.Implementation;
using Data_Layer.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoppingWebApp.Models;

namespace ShoppingWebApp.Controllers
{
    public class CheckoutController : Controller
    {
        IOrderRepository orderRepository;
        IOrderDetailRepository orderDetailRepository;
        IProductRepository productRepository;
        public CheckoutController()
        {
            orderRepository = new OrderRepository();
            orderDetailRepository = new OrderDetailRepository();
            productRepository = new ProductRepository();
        }

        [Authorize(Roles = "User")]
        [HttpPost]
        public IActionResult Checkout(decimal TotalPrice, int NumOfItems, int UserId, string Address)
        {
            bool quantitycheck = false; //false = OK, true = Exception
            var cart = HttpContext.Session.GetComplexData<List<CartItem>>("CART");
            List<CartItem> list = new List<CartItem>();
            if (cart != null)
            {
                list = (List<CartItem>)cart;
            }
            foreach (var item in list)
            {
                var pro = productRepository.GetProductById(item.ProductId);
                if (item.Quantity > pro.Quantity)
                {
                    quantitycheck = true;
                    break;
                }
            }
            if (quantitycheck == true)
            {
                TempData["CheckoutMessage"] = "You're trying to order more than what we have in stock, please try again!";
                return RedirectToAction("Checkout", "Cart");
            }
            else
            {
                Order order = new Order()
                {
                    NumberOfItem = NumOfItems,
                    Address = Address,
                    TotalPrice = TotalPrice,
                    CustomerId = UserId,
                    CreatedDate = DateTime.Now
                };
                int orderId = orderRepository.CreateOrder(order);
                OrderDetail orderDetail;
                foreach (var item in list)
                {
                    orderDetail = new OrderDetail()
                    {
                        OrderId = orderId,
                        ProductId = item.ProductId,
                        ItemPrice = item.Price,
                        Quantity = item.Quantity
                    };
                    orderDetailRepository.AddOrderDetailtoOrder(orderDetail);
                }
                list.Clear();
                HttpContext.Session.SetComplexData("CART", list);
                TempData["CheckoutMessage"] = "Your order has been placed!";
            }
            return RedirectToAction("Index", "Guest");
        }
    }
}

