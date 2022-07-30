using System.Linq;
using Newtonsoft.Json;

namespace ProductShop.DTOs.Product
{
    public class ExportSoldProductsFullInfoDto
    {
        [JsonProperty("count")] public int ProductsCount => SoldProducts.Any() ? SoldProducts.Length : 0;

        [JsonProperty("products")]
        public ExportSoldProductsShortInfoDto[] SoldProducts { get; set; }
    }
}
