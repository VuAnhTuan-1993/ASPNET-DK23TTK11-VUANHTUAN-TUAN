using System;
using System.Collections.Generic;

namespace AppleStore_MVC.Data;

public partial class Product
{
    public int ProductId { get; set; }

    public string ProductName { get; set; } = null!;

    public string Description { get; set; } = null!;

    public int Price { get; set; }

    public string ImageLink { get; set; } = null!;

    public int CategoryId { get; set; }

    public int Amount { get; set; }

    public int SoldQuantity { get; set; }

    public virtual Category Category { get; set; } = null!;
    public virtual ICollection<CartItem> Product_CartItem { get; set; } = new List<CartItem>();
}
