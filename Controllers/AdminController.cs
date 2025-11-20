using Microsoft.AspNetCore.Mvc;
using ASPNET.Models;
using Newtonsoft.Json;
using System.Text;

namespace ASPNET.Controllers
{
    public class AdminController : Controller
    {
          public IActionResult Index()
    {
        return View();
    }
        private readonly IHttpClientFactory _httpClientFactory;

        public AdminController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // Kiểm tra role admin
        private bool IsAdmin() => HttpContext.Session.GetString("user_role") == "admin";

        // Trang quản lý sản phẩm
        public IActionResult Products()
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account");
            return View();
        }

        // GET: AddProduct
        [HttpGet]
        public IActionResult AddProduct()
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account");
            return View();
        }

        // POST: AddProduct
        [HttpPost]
        public async Task<IActionResult> AddProduct(FashionProduct model, string tagsInput)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account");

            if (!ModelState.IsValid)
                return View(model);

            // Tách tags từ input
            model.Tags = tagsInput?.Split(',').Select(t => t.Trim()).ToList() ?? new List<string>();

            var client = _httpClientFactory.CreateClient();
            var json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var response = await client.PostAsync("http://localhost:8500/api/addSP", content);

                if (response.IsSuccessStatusCode)
                {
                    ViewBag.Message = "Thêm sản phẩm thành công!";
                    ModelState.Clear();
                }
                else
                {
                    ViewBag.Message = "Lỗi khi thêm sản phẩm: " + response.StatusCode;
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Lỗi khi kết nối API: " + ex.Message;
            }

            return View();
        }
    }
}
