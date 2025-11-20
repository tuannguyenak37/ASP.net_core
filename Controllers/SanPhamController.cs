using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ASPNET.Services;
using Newtonsoft.Json.Linq;

namespace ASPNET.Controllers
{
    public class SanPhamController : Controller
    {
        private readonly ApiSanPhamService _apiSanPhamService;

        public SanPhamController(ApiSanPhamService apiSanPhamService)
        {
            _apiSanPhamService = apiSanPhamService;
        }

        // 🧾 Trang chi tiết sản phẩm
        // URL: /SanPham/ChiTiet/SP_xxx
        [HttpGet("SanPham/ChiTiet/{id}")]
        public async Task<IActionResult> ChiTiet(string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound("Thiếu mã sản phẩm.");

            JObject? data = await _apiSanPhamService.GetSanPhamByIdAsync(id);

            if (data == null)
                return NotFound("Không tìm thấy sản phẩm.");

            return View("ChiTiet", data);
        }
    }
}
