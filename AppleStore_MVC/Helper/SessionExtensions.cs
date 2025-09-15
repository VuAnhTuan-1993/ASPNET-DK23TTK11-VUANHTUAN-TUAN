using System.Text.Json;
using AppleStore_MVC.Data;

namespace AppleStore_MVC.Helper
{
    public static class SessionExtensions
    {
        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonSerializer.Serialize(value));
        }

        public static T? Get<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null
                ? default
                : JsonSerializer.Deserialize<T>(value);
        }

        public static int GetCartCount(this ISession session, string cartKey = "MYCART")
        {
            var cart = session.Get<List<CartItemViewModel>>(cartKey);
            return cart?.Count ?? 0;
        }
    }
}
