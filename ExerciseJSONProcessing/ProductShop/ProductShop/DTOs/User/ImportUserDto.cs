using System.ComponentModel.DataAnnotations;

namespace ProductShop.DTOs.User
{
    using Newtonsoft.Json;

    public class ImportUserDto
    {
        [JsonProperty("firstName")]
        public string FirstName { get; set; }
        
        [JsonProperty("lastName")]
        [Required]
        [MinLength(3)]
        public string  LastName { get; set; }

        [JsonProperty("age")]
        public int? Age { get; set; }
    }
}
