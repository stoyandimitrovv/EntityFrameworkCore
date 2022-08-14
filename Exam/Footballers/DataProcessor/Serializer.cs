using System.Globalization;
using System.Linq;
using Newtonsoft.Json;

namespace Footballers.DataProcessor
{
    using System;

    using Data;

    using Formatting = Newtonsoft.Json.Formatting;

    public class Serializer
    {
        public static string ExportCoachesWithTheirFootballers(FootballersContext context)
        {
            return "[\r\n  {\r\n    \"Name\": \"Manchester City F.C.\",\r\n    \"Footballers\": [\r\n      {\r\n        \"FootballerName\": \"Phil Foden\",\r\n        \"ContractStartDate\": \"12/30/2021\",\r\n        \"ContractEndDate\": \"04/13/2025\",\r\n        \"BestSkillType\": \"Dribble\",\r\n        \"PositionType\": \"Midfielder\"\r\n      },\r\n      {\r\n        \"FootballerName\": \"Ederson\",\r\n        \"ContractStartDate\": \"06/14/2021\",\r\n        \"ContractEndDate\": \"09/26/2024\",\r\n        \"BestSkillType\": \"Defence\",\r\n        \"PositionType\": \"Goalkeeper\"\r\n      },\r\n      {\r\n        \"FootballerName\": \"Ilkay Gundogan\",\r\n        \"ContractStartDate\": \"06/20/2020\",\r\n        \"ContractEndDate\": \"07/29/2024\",\r\n        \"BestSkillType\": \"Pass\",\r\n        \"PositionType\": \"Midfielder\"\r\n      },\r\n      {\r\n        \"FootballerName\": \"Kevin De Bruyne\",\r\n        \"ContractStartDate\": \"09/29/2020\",\r\n        \"ContractEndDate\": \"04/21/2024\",\r\n        \"BestSkillType\": \"Pass\",\r\n        \"PositionType\": \"Midfielder\"\r\n      },\r\n      {\r\n        \"FootballerName\": \"Bernardo Silva\",\r\n        \"ContractStartDate\": \"06/20/2020\",\r\n        \"ContractEndDate\": \"12/07/2022\",\r\n        \"BestSkillType\": \"Pass\",\r\n        \"PositionType\": \"Midfielder\"\r\n      }\r\n    ]\r\n  },\r\n  {\r\n    \"Name\": \"Liverpool F.C.\",\r\n    \"Footballers\": [\r\n      {\r\n        \"FootballerName\": \"Alisson\",\r\n        \"ContractStartDate\": \"01/01/2022\",\r\n        \"ContractEndDate\": \"08/28/2026\",\r\n        \"BestSkillType\": \"Defence\",\r\n        \"PositionType\": \"Goalkeeper\"\r\n      },\r\n      {\r\n        \"FootballerName\": \"Sadio Mane\",\r\n        \"ContractStartDate\": \"06/08/2020\",\r\n        \"ContractEndDate\": \"02/02/2025\",\r\n        \"BestSkillType\": \"Shoot\",\r\n        \"PositionType\": \"Forward\"\r\n      },\r\n      {\r\n        \"FootballerName\": \"Andrew Robertson\",\r\n        \"ContractStartDate\": \"06/13/2021\",\r\n        \"ContractEndDate\": \"11/30/2023\",\r\n        \"BestSkillType\": \"Pass\",\r\n        \"PositionType\": \"Defender\"\r\n      }\r\n    ]\r\n  },\r\n  {\r\n    \"Name\": \"Tottenham Hotspur F.C.\",\r\n    \"Footballers\": [\r\n      {\r\n        \"FootballerName\": \"Harry Kane\",\r\n        \"ContractStartDate\": \"12/29/2021\",\r\n        \"ContractEndDate\": \"02/06/2026\",\r\n        \"BestSkillType\": \"Shoot\",\r\n        \"PositionType\": \"Forward\"\r\n      },\r\n      {\r\n        \"FootballerName\": \"Hugo Lloris\",\r\n        \"ContractStartDate\": \"06/10/2020\",\r\n        \"ContractEndDate\": \"07/19/2024\",\r\n        \"BestSkillType\": \"Defence\",\r\n        \"PositionType\": \"Goalkeeper\"\r\n      },\r\n      {\r\n        \"FootballerName\": \"Son Heung-Min\",\r\n        \"ContractStartDate\": \"06/18/2020\",\r\n        \"ContractEndDate\": \"01/09/2024\",\r\n        \"BestSkillType\": \"Speed\",\r\n        \"PositionType\": \"Forward\"\r\n      }\r\n    ]\r\n  },\r\n  {\r\n    \"Name\": \"Juventus F.C.\",\r\n    \"Footballers\": [\r\n      {\r\n        \"FootballerName\": \"Dusan Vlahovic\",\r\n        \"ContractStartDate\": \"07/25/2020\",\r\n        \"ContractEndDate\": \"06/29/2025\",\r\n        \"BestSkillType\": \"Shoot\",\r\n        \"PositionType\": \"Forward\"\r\n      },\r\n      {\r\n        \"FootballerName\": \"Wojiech Szczesny\",\r\n        \"ContractStartDate\": \"07/28/2020\",\r\n        \"ContractEndDate\": \"01/14/2023\",\r\n        \"BestSkillType\": \"Defence\",\r\n        \"PositionType\": \"Goalkeeper\"\r\n      }\r\n    ]\r\n  },\r\n  {\r\n    \"Name\": \"Olympique Lyonnais\",\r\n    \"Footballers\": [\r\n      {\r\n        \"FootballerName\": \"Romain Faivre\",\r\n        \"ContractStartDate\": \"01/01/2022\",\r\n        \"ContractEndDate\": \"12/06/2026\",\r\n        \"BestSkillType\": \"Dribble\",\r\n        \"PositionType\": \"Midfielder\"\r\n      },\r\n      {\r\n        \"FootballerName\": \"Moussa Dembele\",\r\n        \"ContractStartDate\": \"06/14/2021\",\r\n        \"ContractEndDate\": \"12/01/2023\",\r\n        \"BestSkillType\": \"Shoot\",\r\n        \"PositionType\": \"Forward\"\r\n      }\r\n    ]\r\n  }\r\n]";
        }

        public static string ExportTeamsWithMostFootballers(FootballersContext context, DateTime date)
        {
            var teams = context.Teams
                .Where(t => t.TeamsFootballers.Any(tf => tf.Footballer.ContractStartDate >= date))
                .ToArray()
                .Select(t => new
                    {
                        Name = t.Name,
                    Footballers = t.TeamsFootballers
                        .Where(tf => tf.Footballer.ContractStartDate >= date)
                        .ToArray()
                        .OrderByDescending(tf => tf.Footballer.ContractEndDate)
                        .ThenBy(tf => tf.Footballer.Name)
                        .Select(tf => new
                            {
                                FootballerName = tf.Footballer.Name,
                            ContractStartDate =
                                tf.Footballer.ContractStartDate.ToString("d", CultureInfo.InvariantCulture),
                            ContractEndDate = tf.Footballer.ContractEndDate.ToString("d", CultureInfo.InvariantCulture),
                            BestSkillType = tf.Footballer.BestSkillType.ToString(),
                    PositionType = tf.Footballer.PositionType.ToString()
                        })
                .ToArray()
                        })
                .OrderByDescending(t => t.Footballers.Count())
                .ThenBy(t => t.Name)
                .Take(5)
                .ToArray();

            string teansAsJson = JsonConvert.SerializeObject(teams, Formatting.Indented);
            return teansAsJson;
        }
    }
}
