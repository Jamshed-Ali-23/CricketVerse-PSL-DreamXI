namespace CricketVerse.Models;

public class Reward
{
    public string Id { get; set; } = "";
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
    public int PointsCost { get; set; }
    public string ImageUrl { get; set; } = "";
    public DateTime ExpiryDate { get; set; }
    public bool IsAvailable { get; set; }
    public string Category { get; set; } = "";
} 