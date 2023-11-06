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
    public class OrderDetailRepository : IOrderDetailRepository
    {
        public IEnumerable<OrderDetail> GetOrderDetailsByOrderId(int orderId) => OrderDetailDAO.Instance.GetOrderDetailsByOrderId(orderId);
        public void AddOrderDetailtoOrder(OrderDetail detail) => OrderDetailDAO.Instance.AddOrderDetailtoOrder(detail);
    }
}
