using BusinessObject;
using Data_Layer.DAO;
using Data_Layer.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Layer.Repository.Implementation
{
    public class OrderRepository : IOrderRepository
    {
        public IEnumerable<Order> GetAllOrders() => OrderDAO.Instance.GetAllOrderList();
        public IEnumerable<Order> GetOrderByCustomerId(int id) => OrderDAO.Instance.GetOrdersByCustomerId(id);
        public Order GetOrderById(int id) => OrderDAO.Instance.GetOrderById(id);
        int IOrderRepository.CreateOrder(Order order) => OrderDAO.Instance.CreateOrder(order);
    }
}