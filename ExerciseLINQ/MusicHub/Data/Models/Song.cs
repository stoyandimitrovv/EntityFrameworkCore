﻿namespace MusicHub.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Enums;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Collections.Generic;

    public class Song
    {
        public Song()
        {
            this.SongPerformers = new HashSet<SongPerformer>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string Name { get; set; }

        [Required]
        public TimeSpan Duration { get; set; }//Required by default

        [Required]
        public DateTime CreatedOn { get; set; }//Required by default

        [Required]
        public Genre Genre { get; set; }//Required by default

        [ForeignKey(nameof(Album))]
        public int? AlbumId { get; set; }
        public virtual Album Album { get; set; }//Navigation prop always virtual

        [ForeignKey(nameof(Writer))]
        public int WriterId { get; set; }
        public virtual Writer Writer { get; set; }

        public decimal Price { get; set; }

        public virtual ICollection<SongPerformer> SongPerformers { get; set; }
    }
}
