using System;
using System.Security.Claims;

namespace CricketVerse.Services;

public class UserContextService : IUserContext
{
    private string? _userId;
    public event Action? OnUserChanged;

    public string? UserId 
    { 
        get => _userId;
        set
        {
            if (_userId != value)
            {
                _userId = value;
                OnUserChanged?.Invoke();
            }
        }
    }

    public bool IsAuthenticated => !string.IsNullOrEmpty(UserId);
} 