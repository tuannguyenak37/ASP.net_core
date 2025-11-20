using Microsoft.AspNetCore.Mvc;
using ASPNET.Services;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Http;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json; // ✅ Thêm dòng này

namespace ASPNET.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApiService _apiService;

        public AccountController(ApiService apiService)
        {
            _apiService = apiService;
        }

        // GET: /Account/Login
        [HttpGet]
        public IActionResult Login() => View();

        // POST: /Account/Login
        [HttpPost]
        public async Task<IActionResult> Login(string user_name, string password)
        {
            try
            {
                var result = await _apiService.LoginAsync(user_name, password);
                var json = JObject.Parse(result);

                bool isSuccess = json["status"]?.ToString() == "succes" &&
                                 (json["data"]?["status"]?.ToObject<bool>() ?? false);

                if (!isSuccess)
                {
                    ViewBag.Error = json["data"]?["message"]?.ToString() ?? "Login failed";
                    Console.WriteLine("⚠️ Login thất bại: " + ViewBag.Error);
                    return View();
                }

                var user = json["data"]?["user"];
                if (user != null)
                {
                    HttpContext.Session.SetString("user_id", user["user_id"]?.ToString() ?? "");
                    HttpContext.Session.SetString("user_first_name", user["first_name"]?.ToString() ?? "");
                    HttpContext.Session.SetString("user_last_name", user["last_name"]?.ToString() ?? "");
                    HttpContext.Session.SetString("user_email", user["email"]?.ToString() ?? "");
                    HttpContext.Session.SetString("user_phone", user["phone"]?.ToString() ?? "");
                    HttpContext.Session.SetString("user_name", user["user_name"]?.ToString() ?? "");
                    HttpContext.Session.SetString("user_role", user["role"]?.ToString() ?? "user");
                    HttpContext.Session.SetString("shop_id", user["shop_id"]?.ToString() ?? "");
                    HttpContext.Session.SetString("shop_name", user["ten_shop"]?.ToString() ?? "");

                    var accessToken = json["data"]?["access_Token"]?.ToString();
                    var refreshToken = json["data"]?["refreshToken"]?.ToString();

                    if (!string.IsNullOrEmpty(accessToken))
                        HttpContext.Session.SetString("access_token", accessToken);
                    if (!string.IsNullOrEmpty(refreshToken))
                        HttpContext.Session.SetString("refresh_token", refreshToken);
                }

                Console.WriteLine($"✅ User {user?["user_name"]?.ToString() ?? "unknown"} đăng nhập thành công");
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Login failed: " + ex.Message;
                Console.WriteLine("❌ Lỗi login: " + ex);
                return View();
            }
        }

        // GET: /Account/Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            Console.WriteLine("✅ User đã logout, session cleared.");
            return RedirectToAction("Index", "Home");
        }

        // GET: /Account/Register
        [HttpGet]
        public IActionResult Register() => View();

        // POST: /Account/Register
        [HttpPost]
        public async Task<IActionResult> Register(
            string first_name, string last_name, string email, string phone,
            string user_name, string password, string date_of_birth)
        {
            try
            {
                // Validate email
                if (string.IsNullOrEmpty(email) || !Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                {
                    ViewBag.Result = "Email không hợp lệ";
                    return View();
                }

                // Validate phone
                if (string.IsNullOrEmpty(phone) || !Regex.IsMatch(phone, @"^\d{9,15}$"))
                {
                    ViewBag.Result = "Số điện thoại không hợp lệ (9-15 chữ số)";
                    return View();
                }

                // Kiểm tra các trường bắt buộc
                if (string.IsNullOrEmpty(first_name) || string.IsNullOrEmpty(last_name) ||
                    string.IsNullOrEmpty(user_name) || string.IsNullOrEmpty(password))
                {
                    ViewBag.Result = "Vui lòng điền đầy đủ thông tin";
                    return View();
                }

                // Chuyển date_of_birth sang yyyy-MM-dd
                string isoDate = "2000-01-01";
                if (!string.IsNullOrEmpty(date_of_birth) && DateTime.TryParse(date_of_birth, out DateTime dob))
                    isoDate = dob.ToString("yyyy-MM-dd");

                var data = new
                {
                    first_name,
                    last_name,
                    email,
                    phone,
                    user_name,
                    password,
                    date_of_birth = isoDate
                };

                Console.WriteLine("➡️ Sending Register JSON: " + JsonConvert.SerializeObject(data));

                var result = await _apiService.PostAsync("createUser", data);

                if (!string.IsNullOrEmpty(result))
                {
                    try
                    {
                        var json = JObject.Parse(result);
                        bool success = json["status"]?.ToString() == "success" || json["status"]?.ToString() == "ok";

                        ViewBag.Result = success
                            ? "Đăng ký thành công!"
                            : json["message"]?.ToString() ?? "Đăng ký thất bại";

                        Console.WriteLine("✅ Response từ API Register: " + result);
                    }
                    catch (Exception parseEx)
                    {
                        ViewBag.Result = "Response không hợp lệ từ backend: " + parseEx.Message;
                        Console.WriteLine("❌ Parse response error: " + parseEx);
                    }
                }
                else
                {
                    ViewBag.Result = "Không nhận được response từ backend";
                }

                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Result = "Error: " + ex.Message;
                Console.WriteLine("❌ Lỗi Register: " + ex);
                return View();
            }
        }
    }
}
