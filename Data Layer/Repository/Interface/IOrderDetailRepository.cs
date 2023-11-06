using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Layer.Repository.Interface
{
    public interface IOrderDetailRepository
    {
        IEnumerable<OrderDetail> GetOrderDetailsByOrderId(int orderId);
        void AddOrderDetailtoOrder(OrderDetail detail);
    }
}

