using System.ComponentModel.DataAnnotations;

namespace CricketVerse.Models
{
    public class Transaction
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public decimal Amount { get; set; }
        public string Description { get; set; } = "";
        public TransactionType Type { get; set; }
        public decimal Balance { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }

    public enum TransactionType
    {
        Deposit,
        Withdrawal
    }
} 