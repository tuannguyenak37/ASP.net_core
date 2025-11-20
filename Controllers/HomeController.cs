using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ASPNET.Models;
using ASPNET.Services;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ASPNET.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApiSanPhamService _sanPhamService;

        public HomeController(ILogger<HomeController> logger, ApiSanPhamService sanPhamService)
        {
            _logger = logger;
            _sanPhamService = sanPhamService;
        }

       public async Task<IActionResult> Index()
{
    List<JToken> sanphams = new List<JToken>();
    List<JToken> bestSellers = new List<JToken>();

    try
    {
        // Gọi danh sách sản phẩm
        sanphams = await _sanPhamService.GetSanPhamAsync();

        // Gọi danh sách sản phẩm bán chạy nhất
        bestSellers = await _sanPhamService.GetBestSellerAsync();
    }
    catch (Exception ex)
    {
        _logger.LogError("Lỗi khi gọi API sản phẩm: {0}", ex.Message);
    }

    // Lấy tối đa 4 sản phẩm hiển thị
    sanphams = sanphams.GetRange(0, System.Math.Min(4, sanphams.Count));

    var model = new HomeViewModel
    {
        SanPhams = sanphams,
        BestSellers = bestSellers
    };

    return View(model);
}


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
    }
}
