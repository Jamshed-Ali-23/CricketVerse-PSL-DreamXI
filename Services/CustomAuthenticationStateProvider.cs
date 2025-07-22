using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using CricketVerse.Models;
using CricketVerse.Data;
using System.Security.Cryptography;
using System.Text;

namespace CricketVerse.Services;

public class CustomAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly IJSRuntime _jsRuntime;
    private readonly NavigationManager _navigationManager;
    private readonly IUserContext _userContext;
    private const string StorageKey = "cricketverse_user";
    private ClaimsPrincipal currentUser = new(new ClaimsIdentity());
    private readonly CricketVerseContext _context;

    public CustomAuthenticationStateProvider(
        IJSRuntime jsRuntime,
        NavigationManager navigationManager,
        IUserContext userContext,
        CricketVerseContext context)
    {
        _jsRuntime = jsRuntime;
        _navigationManager = navigationManager;
        _userContext = userContext;
        _context = context;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            var storedUser = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", StorageKey);
            if (!string.IsNullOrEmpty(storedUser))
            {
                var userInfo = JsonSerializer.Deserialize<StoredUserInfo>(storedUser);
                if (userInfo != null)
                {
                    var user = await Task.Run(() => _context.Users.FirstOrDefault(u => u.Email == userInfo.Email));
                    if (user != null)
                    {
                        var claims = new[]
                        {
                            new Claim(ClaimTypes.Name, user.Username),
                            new Claim(ClaimTypes.Email, user.Email),
                            new Claim("UserId", user.Id.ToString())
                        };
                        var identity = new ClaimsIdentity(claims, "CricketVerse");
                        currentUser = new ClaimsPrincipal(identity);
                        _userContext.UserId = user.Id.ToString();
                    }
                }
            }
        }
        catch (Exception)
        {
            // If there's any error reading the stored user, we'll just return the current user state
        }

        return new AuthenticationState(currentUser);
    }

    private class StoredUserInfo
    {
        public string Email { get; set; } = "";
        public string Username { get; set; } = "";
    }

    public async Task<AuthResult> SignIn(string email, string password, bool rememberMe)
    {
        try
        {
            var result = await LoginUser(email, password);
            
            if (result.Success && rememberMe)
            {
                await _jsRuntime.InvokeVoidAsync("localStorage.setItem", StorageKey, JsonSerializer.Serialize(new { email }));
            }

            return result;
        }
        catch (Exception ex)
        {
            return new AuthResult { Success = false, Message = $"Login failed: {ex.Message}" };
        }
    }

    public async Task<AuthResult> Register(string email, string password, string username, string firstName, string lastName)
    {
        try
        {
            // First check if user already exists
            var existingUser = await Task.Run(() => _context.Users.FirstOrDefault(u => 
                u.Username == username || u.Email == email));

            if (existingUser != null)
            {
                return new AuthResult 
                { 
                    Success = false, 
                    Message = existingUser.Username == username ? 
                        "Username is already taken" : 
                        "Email is already registered" 
                };
            }

            // Simulate network delay for registration
            await Task.Delay(500);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Email, email),
                new Claim("FirstName", firstName),
                new Claim("LastName", lastName),
                new Claim(ClaimTypes.Role, "User")
            };

            var identity = new ClaimsIdentity(claims, "CricketVerse");
            currentUser = new ClaimsPrincipal(identity);

            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", StorageKey, JsonSerializer.Serialize(new { email, username }));

            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());

            return new AuthResult { Success = true };
        }
        catch (Exception ex)
        {
            return new AuthResult { Success = false, Message = ex.Message };
        }
    }

    public async Task SignOut()
    {
        await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", StorageKey);
        currentUser = new ClaimsPrincipal(new ClaimsIdentity());
        _userContext.UserId = null;
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    public async Task<AuthResult> RegisterUser(string username, string email, string password)
    {
        try
        {
            // Check if username is taken
            bool usernameTaken = await Task.Run(() => _context.Users.Any(u => u.Username == username));
            if (usernameTaken)
            {
                return new AuthResult { Success = false, Message = "Username is already taken" };
            }

            // Check if email is taken
            bool emailTaken = await Task.Run(() => _context.Users.Any(u => u.Email == email));
            if (emailTaken)
            {
                return new AuthResult { Success = false, Message = "Email is already registered" };
            }

            // Create new user
            var user = new User
            {
                Username = username,
                Email = email,
                PasswordHash = HashPassword(password),
                Balance = 1000 // Initial balance for new users
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new AuthResult { Success = true };
        }
        catch (Exception ex)
        {
            return new AuthResult { Success = false, Message = $"Registration failed: {ex.Message}" };
        }
    }

    public async Task<AuthResult> LoginUser(string username, string password)
    {
        try
        {
            var user = await Task.Run(() => _context.Users.FirstOrDefault(u => 
                u.Username == username || u.Email == username
            ));

            if (user == null || user.PasswordHash != HashPassword(password))
            {
                return new AuthResult { Success = false, Message = "Invalid username or password" };
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("UserId", user.Id.ToString())
            };

            var identity = new ClaimsIdentity(claims, "CricketVerse");
            currentUser = new ClaimsPrincipal(identity);
            _userContext.UserId = user.Id.ToString();

            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());

            return new AuthResult { Success = true };
        }
        catch (Exception ex)
        {
            return new AuthResult { Success = false, Message = $"Login failed: {ex.Message}" };
        }
    }

    private string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(hashedBytes);
    }
}

public class AuthResult
{
    public bool Success { get; set; }
    public string Message { get; set; } = "";
} 