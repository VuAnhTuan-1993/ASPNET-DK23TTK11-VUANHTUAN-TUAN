using AppleStore_MVC.Data;

namespace AppleStore_MVC.Data
{
    public class HomeViewModel
    {
        public List<Product> NewestProducts { get; set; }

        public List<Product> TopSellingProducts { get; set; }  

        public Product? BestSellerProduct { get; set; } 
    }
}
