using BusinessObject;
using Data_Layer.Repository.Implementation;
using Data_Layer.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ShoppingWebApp.Controllers
{
    public class OrdersController : Controller
    {
        IOrderRepository orderRepository = null;
        IOrderDetailRepository orderDetailRepository = null;

        public OrdersController()
        {
            orderRepository = new OrderRepository();
            orderDetailRepository = new OrderDetailRepository();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            var orderList = orderRepository.GetAllOrders();
            return View(orderList);
        }

        // GET: ProductsController/Details/5
        [Authorize(Roles = "Admin")]
        public ActionResult Details(int id)
        {
            IEnumerable<OrderDetail> orderDetails;
            var order = orderRepository.GetOrderById(id);
            if (order == null)
            {
                return NotFound();
            }
            orderDetails = orderDetailRepository.GetOrderDetailsByOrderId(id);
            ViewData["OrderDetailList"] = orderDetails;
            return View(order);
        }
    }
}
