using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CricketVerse.Models
{
    public class Player
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = "";

        [StringLength(50)]
        public string Country { get; set; } = "";

        [Required]
        [StringLength(20)]
        public string Role { get; set; } = "";

        [Required]
        [StringLength(3)]
        public string Team { get; set; } = "";

        public decimal Price { get; set; }
        public int Points { get; set; }
        
        [StringLength(500)]
        public string ImageUrl { get; set; } = "";

        // Batting Statistics
        public int Matches { get; set; }
        public int Runs { get; set; }
        public decimal BattingAverage { get; set; }
        public decimal StrikeRate { get; set; }
        public int Centuries { get; set; }
        public int Fifties { get; set; }

        // Bowling Statistics
        public int Wickets { get; set; }
        public decimal BowlingAverage { get; set; }
        public decimal EconomyRate { get; set; }
        public int FiveWicketHauls { get; set; }

        public List<Team> Teams { get; set; } = new();
    }
} 