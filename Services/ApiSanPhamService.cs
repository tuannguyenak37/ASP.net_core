using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace ASPNET.Services
{
    public class ApiSanPhamService
    {
        private readonly HttpClient _httpClient;

        public ApiSanPhamService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<JToken>> GetSanPhamAsync()
        {
            var response = await _httpClient.GetAsync("SP");
            response.EnsureSuccessStatusCode();

            var jsonString = await response.Content.ReadAsStringAsync();
            var json = JObject.Parse(jsonString);

            if (json["status"]?.ToString() == "success")
                return json["data"]?.ToObject<List<JToken>>() ?? new List<JToken>();

            return new List<JToken>();
        }
        // 🏆 Lấy danh sách sản phẩm bán chạy nhất
        // 🏆 Lấy danh sách sản phẩm bán chạy nhất
        public async Task<List<JToken>> GetBestSellerAsync()
        {
            var response = await _httpClient.GetAsync("bestseller");
            response.EnsureSuccessStatusCode();

            var jsonString = await response.Content.ReadAsStringAsync();
            var json = JObject.Parse(jsonString);

            if (json["status"]?.ToString() == "success")
                return json["data"]?.ToObject<List<JToken>>() ?? new List<JToken>();

            return new List<JToken>();
        }
        // 🧾 Lấy chi tiết sản phẩm theo ID
// ApiSanPhamService.cs
public async Task<JObject?> GetSanPhamByIdAsync(string sanpham_id)
{
    var response = await _httpClient.GetAsync($"SPCT/{sanpham_id}");
    response.EnsureSuccessStatusCode();

    var jsonString = await response.Content.ReadAsStringAsync();
    var json = JObject.Parse(jsonString);

    if (json["status"]?.ToString() == "success")
        return json["data"] as JObject; // ✅ ép kiểu sang JObject

    return null;
}


    }
}
