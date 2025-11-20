using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace ASPNET.Models
{
    public class HomeViewModel
    {
        public List<JToken> SanPhams { get; set; } = new List<JToken>();
    public List<JToken> BestSellers { get; set; } = new List<JToken>();
    }
}
