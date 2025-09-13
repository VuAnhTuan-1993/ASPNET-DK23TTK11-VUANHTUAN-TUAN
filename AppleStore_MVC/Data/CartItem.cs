using System.ComponentModel.DataAnnotations.Schema;

namespace AppleStore_MVC.Data;

public partial class CartItem
{
    public int cart_id { get; set; } // đây chính là CartID (FK)
    public int CartItemId { get; set; }// khóa chính riêng
    public int quantity { get; set; }
    public double unitPrice { get; set; }
    public int pro_id { get; set; }

    [ForeignKey("cart_id")]
    public Cart Cart { get; set; } = null!;

    [ForeignKey("pro_id")]
    public Product Product { get; set; } = null!;
}

//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations.Schema;

//namespace AppleStore_MVC.Data.DataModel;
////[Table("CartItem")]
//public partial class CartItem
//{
//    public int id { get; set; }
//    public string quantity { get; set; }
//    public float unitPrice { get; set; }
//    public int pro_id { get; set; }
//    [ForeignKey("id")]
//    public Cart Cart { get; set; } = null!;
//    [ForeignKey("pro_id")]
//    public Product Product { get; set; } = null!;
//}
