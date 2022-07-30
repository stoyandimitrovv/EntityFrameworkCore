using Newtonsoft.Json;

namespace ProductShop.DTOs.User
{
    public class ExportUsersInfoDto
    {
        [JsonProperty("usersCount")]
        public int UsersCount { get; set; }

        [JsonProperty("users")]
        public ExportUsersWithFullProductInfo[] Users { get; set; }
    }
}
