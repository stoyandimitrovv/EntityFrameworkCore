using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace ProductShop.DTOs.Product
{
    public class ImportProductDto
    {
        [JsonProperty(nameof(Name))]
        [Required]
        [MinLength(3)]
        public string Name { get; set; }

        [JsonProperty(nameof(Price))]
        public decimal Price { get; set; }

        [JsonProperty(nameof(SellerId))]
        public int SellerId { get; set; }

        [JsonProperty(nameof(BuyerId))]
        public int? BuyerId { get; set; }
    }
}
