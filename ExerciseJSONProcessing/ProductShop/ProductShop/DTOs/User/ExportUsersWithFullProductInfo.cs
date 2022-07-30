using Newtonsoft.Json;
using ProductShop.DTOs.Product;

namespace ProductShop.DTOs.User
{
    public class ExportUsersWithFullProductInfo
    {
        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [JsonProperty("age")]
        public int? Age { get; set; }

        [JsonProperty("soldProducts")]
        public ExportSoldProductsFullInfoDto SoldProductsInfo { get; set; }
    }
}
