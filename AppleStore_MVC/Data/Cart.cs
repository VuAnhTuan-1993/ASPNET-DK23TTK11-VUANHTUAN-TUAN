using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppleStore_MVC.Data;

public partial class Cart
{
    [Key]
    public int id { get; set; }
    public int u_id { get; set; }
    //public DateTime? buy_date { get; set; }
    public DateTime buy_date { get; set; }

    // 1 Cart có nhiều CartItem
    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    [ForeignKey("u_id")]
    public User User_Of_Cart { get; set; } = null!;
}
