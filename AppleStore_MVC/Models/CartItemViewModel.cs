namespace AppleStore_MVC.Data
{
    public class CartItemViewModel
    {
        public int productId { get; set; }
        public string imageLink { get; set; }
        public string productName { get; set; }

        public int price { get; set; }
        public int amount { get; set; }
        public int totalPrice
        {
            get { return price * amount; }
        }
    }
}

