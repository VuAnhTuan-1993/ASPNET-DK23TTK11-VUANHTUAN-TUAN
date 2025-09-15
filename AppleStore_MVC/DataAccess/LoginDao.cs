using AppleStore_MVC.Data;
using Microsoft.EntityFrameworkCore;

namespace AppleStore_MVC.DataAccess
{
    public class LoginDao
    {
        private readonly AppleStoreContext _context;
        public LoginDao(AppleStoreContext context)
        {
            _context = context;
        }

        public async Task<User> GetUserAsync(string username, string password)
        {
            return await _context.Users
                .FirstOrDefaultAsync(a => a.UserName == username && a.Password == password);
        }

    }
}
