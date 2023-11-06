using BusinessObject;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Layer.DAO
{
    public class OrderDetailDAO
    {
        ProductDAO productRepository = ProductDAO.Instance;
        private static OrderDetailDAO instance = null;
        private static readonly object instanceLock = new object();
        public static OrderDetailDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new OrderDetailDAO();
                    }
                    return instance;
                }
            }
        }
        //-------------------------------------------------------------

        public IEnumerable<OrderDetail> GetOrderDetailsByOrderId(int id)
        {
            var orders = new List<OrderDetail>();
            try
            {
                using var context = new ClothesShoppingContext();
                orders = context.OrderDetails.Include(o => o.Product).Where(o => o.OrderId == id).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return orders;
        }
        public void AddOrderDetailtoOrder(OrderDetail detail)
        {
            try
            {
                using var context = new ClothesShoppingContext();
                context.OrderDetails.Add(detail);
                context.SaveChanges();
                SubtractStock(detail.ProductId, detail.Quantity);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        private void SubtractStock(int productId, int quantity)
        {
            try
            {
                using var context = new ClothesShoppingContext();
                var product = productRepository.GetProductById(productId);
                if (product != null)
                {
                    context.Products.Attach(product);
                    product.Quantity -= quantity;
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
