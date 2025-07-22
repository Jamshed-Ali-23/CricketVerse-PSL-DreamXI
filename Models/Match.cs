using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CricketVerse.Models
{
    public class Match
    {
        public string Id { get; set; } = "";
        public string MatchNumber { get; set; } = "";
        public string MatchType { get; set; } = "PSL";
        public string Status { get; set; } = "Upcoming";
        public string Result { get; set; } = "";
        public Team? WinningTeam { get; set; }
        public Team Team1 { get; set; } = new();
        public Team Team2 { get; set; } = new();
        public int Team1Score { get; set; }
        public int Team1Wickets { get; set; }
        public decimal Team1Overs { get; set; }
        public int Team2Score { get; set; }
        public int Team2Wickets { get; set; }
        public decimal Team2Overs { get; set; }
        public string Venue { get; set; } = "";
        public DateTime MatchDate { get; set; }
        public string RequiredRunRate { get; set; } = "";
        public List<string> Commentary { get; set; } = new();
        public List<BallByBall> BallByBallData { get; set; } = new List<BallByBall>();
    }

    public class MatchSimulation
    {
        [Required]
        public required Match Match { get; set; }
        public List<BallByBall> BallByBallData { get; set; } = new List<BallByBall>();
        public bool IsComplete { get; set; }
    }

    public class BallByBall
    {
        public int Id { get; set; }
        public int Over { get; set; }
        public int Ball { get; set; }
        [Required]
        public required string Batsman { get; set; } = "";
        [Required]
        public required string Bowler { get; set; } = "";
        public int Runs { get; set; }
        [Required]
        public required string Event { get; set; } = ""; // Normal, Wicket, Wide, No-ball, etc.
        [Required]
        public required string Commentary { get; set; } = "";
        public DateTime Timestamp { get; set; }
    }
} 