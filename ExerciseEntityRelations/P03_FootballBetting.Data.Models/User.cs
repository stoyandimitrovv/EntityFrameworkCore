﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using P03_FootballBetting.Data.Common;

namespace P03_FootballBetting.Data.Models
{
    public class User
    {
        public User()
        {
            this.Bets = new HashSet<Bet>();
        }

        [Key]
        public int UserId { get; set; }

        [Required]
        [MaxLength(GlobalConstants.UserNameMaxLength)]
        public string Username { get; set; }

        [Required]
        [MaxLength(GlobalConstants.UserPasswordMaxLength)]
        public string Password { get; set; }

        [Required]
        [MaxLength(GlobalConstants.UserEmailMaxLength)]
        public string Email { get; set; }

        [Required]
        [MaxLength(GlobalConstants.UserRealNameMaxLength)]
        public string Name { get; set; }

        public decimal Balance { get; set; }

        public virtual ICollection<Bet> Bets { get; set; }
    }
}
