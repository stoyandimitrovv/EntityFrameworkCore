using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace ProductShop.DTOs.Category
{
    public class ImportCategoryDto
    {
        [JsonProperty("name")]
        [Required]
        public string Name { get; set; }
    }
}
