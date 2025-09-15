using System.ComponentModel.DataAnnotations.Schema;

namespace AppleStore_MVC.Models
{
    public class User
    {
        
        public int UserId { get; set; }

        public string UserName { get; set; } = null!;

        public string Password { get; set; } = null!;

        public int IsAdmin { get; set; }
    }
}
