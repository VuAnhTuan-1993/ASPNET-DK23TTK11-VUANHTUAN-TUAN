using System;
using System.Collections.Generic;

namespace AppleStore_MVC.Data;

public partial class User
{
    public int UserId { get; set; }

    public string UserName { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int IsAdmin { get; set; }
    // 1 User có nhiều Cart
    public virtual ICollection<Cart> Carts_of_User { get; set; } = new List<Cart>();
}
