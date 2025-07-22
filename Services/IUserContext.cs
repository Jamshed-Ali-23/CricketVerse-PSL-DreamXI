using System;

namespace CricketVerse.Services;

public interface IUserContext
{
    string? UserId { get; set; }
    bool IsAuthenticated { get; }
    event Action? OnUserChanged;
} 