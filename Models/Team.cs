using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CricketVerse.Models
{
    public class Team
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = "";

        [StringLength(500)]
        public string Description { get; set; } = "";

        [StringLength(200)]
        public string LogoUrl { get; set; } = "";

        public bool IsComplete { get; set; }
        public int Credits { get; set; }
        public int Points { get; set; }
        
        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public int? CaptainId { get; set; }
        public Player? Captain { get; set; }

        public int? ViceCaptainId { get; set; }
        public Player? ViceCaptain { get; set; }

        public List<Player> Players { get; set; } = new();
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public static string GetDefaultLogoUrl(string teamName)
        {
            return teamName.ToLower() switch
            {
                var name when name.Contains("lahore") || name.Contains("qalandars") => "/images/teams/lah.png",
                var name when name.Contains("karachi") || name.Contains("kings") => "/images/teams/kar.png",
                var name when name.Contains("islamabad") || name.Contains("united") => "/images/teams/isl.png",
                var name when name.Contains("multan") || name.Contains("sultans") => "/images/teams/mul.png",
                var name when name.Contains("peshawar") || name.Contains("zalmi") => "/images/teams/pes.png",
                var name when name.Contains("quetta") || name.Contains("gladiators") => "/images/teams/que.png",
                _ => "/images/teams/default.png"
            };
        }
    }
} 