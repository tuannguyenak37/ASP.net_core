using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ASPNET.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;

        public ApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            // Ví dụ: _httpClient.BaseAddress = new Uri("http://localhost:7677/api/");
        }

        // Gọi API với endpoint bất kỳ, method POST, data JSON
        public async Task<string> PostAsync(string endpoint, object data)
        {
            var json = JsonConvert.SerializeObject(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(endpoint, content);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }

        // Gọi API với GET, optional query string
        public async Task<string> GetAsync(string endpoint)
        {
            var response = await _httpClient.GetAsync(endpoint);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }

        // --- Thêm phương thức Login riêng để dễ sử dụng ---
        public async Task<string> LoginAsync(string username, string password)
        {
            var data = new { user_name = username, password = password };
            // Endpoint "Login" sẽ nối với BaseAddress nếu đã set
            return await PostAsync("Login", data);
        }
    }
}
