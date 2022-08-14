using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Newtonsoft.Json;

namespace Footballers.DataProcessor.ImportDto
{
    public class ImportTeamDto
    {
        [Required]
        [MinLength(3)]
        [MaxLength(40)]
        [RegularExpression(@"^[A-Za-z0-9\s.-]+$")]
        [JsonProperty(nameof(Name))]
        public string Name { get; set; }

        [Required]
        [MinLength(2)]
        [MaxLength(40)]
        [JsonProperty(nameof(Nationality))]
        public string Nationality { get; set; }

        [JsonProperty(nameof(Trophies))]
        [Range(1, int.MaxValue)]
        public int Trophies { get; set; }

        [JsonProperty(nameof(Footballers))]
        public int[] Footballers { get; set; }
    }
}
