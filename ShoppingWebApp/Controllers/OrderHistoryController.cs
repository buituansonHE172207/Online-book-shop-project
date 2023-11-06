using BusinessObject;
using Data_Layer.Repository.Implementation;
using Data_Layer.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ShoppingWebApp.Controllers
{
    public class OrderHistoryController : Controller
    {
        IOrderRepository orderRepository;
        IOrderDetailRepository orderDetailRepository;

        public OrderHistoryController()
        {
            orderRepository = new OrderRepository();
            orderDetailRepository = new OrderDetailRepository();
        }
        // GET: OrderHistoryController
        [Authorize(Roles = "User")]
        public ActionResult Index()
        {
            string email = User.Claims.SingleOrDefault(c => c.Type.Equals(ClaimTypes.Email)).Value;
            IUserRepository userRepository = new UserRepository();
            User user = userRepository.GetUser(email);
            var orderList = orderRepository.GetOrderByCustomerId(user.UserId);

            return View(orderList);
        }

        // GET: OrderHistoryController/Details/5
        [Authorize(Roles = "User")]
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

