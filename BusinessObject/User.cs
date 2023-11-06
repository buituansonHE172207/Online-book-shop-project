using System;
using System.Collections.Generic;

namespace BusinessObject;

public partial class User
{
    public User()
    {
        Orders = new HashSet<Order>();
    }

    public int UserId { get; set; }

    public string FullName { get; set; } = null!;

    public string? PhoneNumber { get; set; }

    public string Email { get; set; } = null!;

    public string? Address { get; set; }

    public string Password { get; set; } = null!;

    public bool? Gender { get; set; }

    public DateTime? Birthday { get; set; }

    public string? Avatar { get; set; }

    public int Role { get; set; }

    public bool Status { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual Role RoleNavigation { get; set; } = null!;
}
