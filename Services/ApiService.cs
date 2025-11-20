using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System;

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

        // POST bất kỳ endpoint với payload JSON
        public async Task<string> PostAsync(string endpoint, object data)
        {
            try
            {
                var json = JsonConvert.SerializeObject(data);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(endpoint, content);

                string responseBody = await response.Content.ReadAsStringAsync();

                // Log nếu backend trả lỗi
                if (!response.IsSuccessStatusCode)
                    Console.WriteLine($"⚠ API trả lỗi {response.StatusCode}: {responseBody}");

                return responseBody;
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Lỗi gọi API: " + ex);
                throw;
            }
        }

        // GET bất kỳ endpoint
        public async Task<string> GetAsync(string endpoint)
        {
            try
            {
                var response = await _httpClient.GetAsync(endpoint);
                string responseBody = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                    Console.WriteLine($"⚠ API trả lỗi {response.StatusCode}: {responseBody}");

                return responseBody;
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Lỗi gọi API GET: " + ex);
                throw;
            }
        }

        // Login riêng
        public async Task<string> LoginAsync(string username, string password)
        {
            var data = new { user_name = username, password = password };
            return await PostAsync("Login", data);
        }
    }
}
