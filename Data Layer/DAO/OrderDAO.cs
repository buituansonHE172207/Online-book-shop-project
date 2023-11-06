using BusinessObject;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Layer.DAO
{
    public class OrderDAO
    {
        private static OrderDAO instance = null;
        private static readonly object instanceLock = new object();
        public static OrderDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new OrderDAO();
                    }
                    return instance;
                }
            }
        }


        public IEnumerable<Order> GetAllOrderList()
        {
            var orders = new List<Order>();
            try
            {
                using var context = new ClothesShoppingContext();
                orders = context.Orders.Include(o => o.Customer).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return orders;
        }

        public IEnumerable<Order> GetOrdersByCustomerId(int id)
        {
            var orders = new List<Order>();
            try
            {
                using var context = new ClothesShoppingContext();
                orders = context.Orders.Where(o => o.CustomerId == id).OrderByDescending(order => order.CreatedDate).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return orders;
        }

        public Order GetOrderById(int id)
        {
            Order order;
            try
            {
                using var context = new ClothesShoppingContext();
                order = context.Orders.Include(o => o.Customer).FirstOrDefault(o => o.OrderId == id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return order;
        }

        public int CreateOrder(Order order)
        {
            int orderId;

            Order orderobj = GetOrderById(order.OrderId);
            if (orderobj == null)
            {
                using var context = new ClothesShoppingContext();
                context.Orders.Add(order);
                context.SaveChanges();
                orderId = order.OrderId;
            }
            else
            {
                throw new Exception("The order is already exist!");
            }

            return orderId;
        }
    }
}
