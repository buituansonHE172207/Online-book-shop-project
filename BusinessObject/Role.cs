using System;
using System.Collections.Generic;

namespace BusinessObject;

public partial class Role
{
    public Role()
    {
        Users = new HashSet<User>();
    }

    public int RoleId { get; set; }

    public string RoleName { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
