using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.Controllers
{
    [AllowAnonymous]
    public class LoginController : Controller
    {
        // GET: Login
        public IActionResult Login(bool IsRightAccount = true)
        {
            ViewData["Message"]="";
            if (!IsRightAccount)
            {
                ViewData["Message"] = "user or password is wrong!";
            }
            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public IActionResult Login(string user, string password)
        {
            if ("admin".Equals(user) && "admin".Equals(password))
            {
                HttpContext.Session.SetString("userName", user);
                return RedirectToAction("Index", "Employees");
            }
            return RedirectToAction(nameof(Login), new { IsRightAccount = false });
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("userName");
            return RedirectToAction(nameof(Login));
        }

    }
}