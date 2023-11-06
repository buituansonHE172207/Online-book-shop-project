using System;
using System.Collections.Generic;

namespace BusinessObject;

public partial class Order
{
    public Order()
    {
        OrderDetails = new HashSet<OrderDetail>();
    }

    public int OrderId { get; set; }

    public int CustomerId { get; set; }

    public string Address { get; set; } = null!;

    public decimal TotalPrice { get; set; }

    public int NumberOfItem { get; set; }

    public DateTime CreatedDate { get; set; }

    public virtual User Customer { get; set; } = null!;

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}
