using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CricketVerse.Models
{
    public class FantasyContest
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public required string Name { get; set; }

        [Required]
        public required string CreatorId { get; set; }

        [Required]
        public required string Description { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public int MaxParticipants { get; set; }
        public List<string> ParticipantIds { get; set; } = new List<string>();

        public decimal EntryFee { get; set; }
        public decimal PrizeMoney { get; set; }

        [Required]
        public required string Status { get; set; } // Open, In Progress, Completed

        public Dictionary<string, int> UserScores { get; set; } = new Dictionary<string, int>();
    }

    public class FantasyTeam
    {
        public int Id { get; set; }

        [Required]
        public required string UserId { get; set; }

        [Required]
        public int ContestId { get; set; }

        [Required]
        [StringLength(50)]
        public required string Name { get; set; }

        [Required]
        public List<Player> Players { get; set; } = new List<Player>();

        public int TotalPoints { get; set; }
        public int Rank { get; set; }
    }

    public class PlayerPoints
    {
        public int PlayerId { get; set; }
        public int Points { get; set; }
        public Dictionary<string, int> PointBreakdown { get; set; } = new Dictionary<string, int>();
    }
} 