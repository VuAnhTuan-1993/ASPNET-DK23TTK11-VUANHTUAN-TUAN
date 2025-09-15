using System.ComponentModel.DataAnnotations.Schema;

namespace AppleStore_MVC.Models
{
    [Table("Category")]
    public class Category
    {
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public string icon { get; set; }
    }
}
