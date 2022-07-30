using Newtonsoft.Json;
using ProductShop.DTOs.Product;

namespace ProductShop.DTOs.User
{
    public class ExportUsersWithSoldProductsDto
    {
        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("lastName")]
        public string LastName  { get; set; }

        [JsonProperty("soldProducts")]
        public ExportUserSoldProductsDto[] SoldProducts { get; set; }
    }
}
