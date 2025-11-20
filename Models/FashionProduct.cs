using System.Collections.Generic;

namespace ASPNET.Models
{
    public class FashionProduct
    {
        public string Name { get; set; } = string.Empty;        // Khởi tạo mặc định để không null
        public string Category { get; set; } = string.Empty;    
        public string Brand { get; set; } = string.Empty;
        public int BasePrice { get; set; }                       // int không null
        public string Description { get; set; } = string.Empty;
        public List<string> Tags { get; set; } = new List<string>(); // Khởi tạo list trống
    }
}
