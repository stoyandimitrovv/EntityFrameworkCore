using Newtonsoft.Json;

namespace ProductShop.DTOs.Product
{
    public class ExportSoldProductsShortInfoDto
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("price")]
        public decimal Price { get; set; }
    }
}
