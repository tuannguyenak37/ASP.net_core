using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Json;

namespace ASPNET.Controllers
{
    public class MallController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;
        private const string BASE_API = "http://localhost:7677/api";

        public MallController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<IActionResult> Index()
        {
            var client = _clientFactory.CreateClient();

            var spList = new List<Product>();
            var randomList = new List<Product>();
            var bestsellerList = new List<Product>();

            try
            {
                // Sản phẩm giảm giá
                var spResponse = await client.GetFromJsonAsync<ApiResponse>($"{BASE_API}/SP");
                if (spResponse?.data != null)
                    spList = spResponse.data;

                // Sản phẩm gợi ý
                var randomResponse = await client.GetFromJsonAsync<ApiResponse>($"{BASE_API}/sp20");
                if (randomResponse?.data != null)
                    randomList = randomResponse.data;

                // Sản phẩm bán chạy
                var bestsellerResponse = await client.GetFromJsonAsync<ApiResponse>($"{BASE_API}/bestseller");
                if (bestsellerResponse?.data != null)
                    bestsellerList = bestsellerResponse.data;
            }
            catch (HttpRequestException ex)
            {
                // Log lỗi API
                Console.WriteLine("❌ Lỗi khi gọi API: " + ex.Message);
            }
            catch (NotSupportedException ex)
            {
                Console.WriteLine("❌ Dữ liệu không hỗ trợ: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Lỗi khác: " + ex.Message);
            }

            // Truyền dữ liệu xuống View
            var model = new MallViewModel
            {
                SPList = spList,
                RandomList = randomList,
                BestsellerList = bestsellerList
            };

            return View(model);
        }
    }

    // ViewModel
    public class MallViewModel
    {
        public List<Product> SPList { get; set; } = new List<Product>();
        public List<Product> RandomList { get; set; } = new List<Product>();
        public List<Product> BestsellerList { get; set; } = new List<Product>();
    }

    // Product model
 public class Product
{
    public int sanpham_id { get; set; }
    public string? ten_sanpham { get; set; }  // <-- cho phép null
    public decimal gia_ban { get; set; }
    public string? url_sanpham { get; set; }  // cũng có thể null
}

    // Response API chung
    public class ApiResponse
    {
        public List<Product> data { get; set; } = new List<Product>();
    }
}
