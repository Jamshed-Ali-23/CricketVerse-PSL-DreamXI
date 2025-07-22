using System.ComponentModel.DataAnnotations;

namespace CricketVerse.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Username { get; set; } = "";

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; } = "";

        [Required]
        public string PasswordHash { get; set; } = "";

        public decimal Balance { get; set; }

        public virtual ICollection<Team> Teams { get; set; } = new List<Team>();
        public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
} 