using Microsoft.AspNetCore.Mvc;
using ASPNET.Services;
using Newtonsoft.Json.Linq;

namespace ASPNET.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApiService _apiService;

        public AccountController(ApiService apiService)
        {
            _apiService = apiService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string user_name, string password)
        {
            try
            {
                var result = await _apiService.LoginAsync(user_name, password);

                // Parse JSON
                var json = JObject.Parse(result);

                bool isSuccess = json["status"]?.ToString() == "succes" &&
                                 (json["data"]?["status"]?.ToObject<bool>() ?? false);

                if (isSuccess)
                {
                    var user = json["data"]?["user"];
                    if (user != null)
                    {
                        // Lưu session
                        HttpContext.Session.SetString("user_last_name", user["last_name"]?.ToString() ?? "");
                        HttpContext.Session.SetString("user_first_name", user["first_name"]?.ToString() ?? "");
                        HttpContext.Session.SetString("user_id", user["user_id"]?.ToString() ?? "");
                    }

                    return RedirectToAction("Index", "Home");
                }

                ViewBag.Error = json["data"]?["message"]?.ToString() ?? "Login failed";
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Login failed: " + ex.Message;
                return View();
            }
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}
