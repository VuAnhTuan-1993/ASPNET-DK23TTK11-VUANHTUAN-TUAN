using AppleStore_MVC.DataAccess;

namespace AppleStore_MVC.Data
{
    public class ProductViewModel
    {
        public List<Product> Products { get; set; }

        public Product BestSellerProduct { get; set; }

        public int EndPage { get; set; }

        public int CurrentPage { get; set; }

        public int Cid { get; set; }

        public string name { get; set; } = "none";
    }
}
