using System;
using System.Collections.Generic;

namespace AppleStore_MVC.Data;

public partial class Category
{
    public int CategoryId { get; set; }

    public string CategoryName { get; set; } = null!;

    public string Icon { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
