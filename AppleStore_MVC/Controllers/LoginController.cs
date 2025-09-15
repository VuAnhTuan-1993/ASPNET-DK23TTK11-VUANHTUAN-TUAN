using AppleStore_MVC.DataAccess;
using AppleStore_MVC.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AppleStore_MVC.Models;
using Newtonsoft.Json;
using System.Reflection.Metadata.Ecma335;

namespace AppleStore_MVC.Controllers
{
    public class LoginController : Controller
    {
        private readonly LoginDao _loginDao;

        public LoginController()
        {
            _loginDao = new LoginDao(new AppleStoreContext());
        }

        public IActionResult Index()
        {
            return View();
        }
        // POST: /login
        [HttpPost]
        public ActionResult DoLogin()
        {
            string name = Request.Form["name"];
            string pass = Request.Form["pass"];
            var user = _loginDao.GetUserAsync(name, pass).Result;

            if (user == null)
            {
                ViewBag.Msg = "Tài khoản hoặc mật khẩu không chính xác";
                return View("Index");
            }
            else
            {
                var userSession = new UserSession
                {
                    Id = user.UserId,
                    UserName = user.UserName
                };

                HttpContext.Session.SetString("acc", JsonConvert.SerializeObject(userSession));
                if (user.IsAdmin == 1) // Fixed: Compare integer value instead of string
                {
                    return RedirectToAction("Index", "Dashboard"); // sang trang admin
                }
                else
                {
                    return RedirectToAction("Index", "Home"); // sang trang user thường
                }
            }
        }

        public ActionResult LogoutAccount()
        {
            HttpContext.Session.Remove("acc");
            return RedirectToAction("Index", "Home");
        }

        // POST: /Login/SignUp
        [HttpPost]
        public ActionResult SignUp()
        {
            string name = Request.Form["user-name"];
            string pass = Request.Form["user-pass"];
            string repass = Request.Form["user-repass"];
            if (pass != repass)
            {
                ViewBag.SignUpMsg = "Mật khẩu không khớp";
                ViewBag.ActiveForm = "signup";
                return View("Index");
            }
            var existingUser = _loginDao.GetUserAsync(name, pass).Result;
            if (existingUser != null)
            {
                ViewBag.SignUpMsg = "Tài khoản đã tồn tại";
                ViewBag.ActiveForm = "signup";
                return View("Index");
            }
            var newUser = new Data.User
            {
                UserName = name,
                Password = pass,
                IsAdmin = 0 // Mặc định là user thường
            };
            using (var context = new AppleStoreContext())
            {
                context.Users.Add(newUser);
                context.SaveChanges();
            }
            ViewBag.Msg = "Đăng ký thành công! Vui lòng đăng nhập.";
            return View("Index");
        }
    }
}
