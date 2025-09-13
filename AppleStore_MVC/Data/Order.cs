using Microsoft.AspNetCore.Identity;
namespace AppleStore_MVC.Data
{
    public class Order
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public IdentityUser User { get; set; }
        //public Address Address { get; set; }
        public double Amount { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime DeliveriedAt { get; set; }
        //public List <OrderProduct> Items { get; set; }
    }
}
