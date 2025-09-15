namespace AppleStore_MVC.Models
{
    public class Product
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; } = null!;

        public string Description { get; set; } = null!;

        public int Price { get; set; }

        public string ImageLink { get; set; } = null!;

        public int CategoryId { get; set; }// Khóa ngoại


        public int Amount { get; set; }

        public int SoldQuantity { get; set; }

        public virtual Category Category { get; set; }
            = null!;//Navigation property
        //public virtual ICollection<CartItem> CartItems_Product { get; set; } = new List<CartItem>();
    }
}
