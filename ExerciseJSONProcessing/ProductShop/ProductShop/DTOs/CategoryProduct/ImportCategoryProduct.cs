using Newtonsoft.Json;

namespace ProductShop.DTOs.CategoryProduct
{
    public class ImportCategoryProduct
    {
        [JsonProperty(nameof(CategoryId))]
        public int CategoryId { get; set; }

        [JsonProperty(nameof(ProductId))]
        public int ProductId { get; set; }
    }
}
