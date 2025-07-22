using Microsoft.JSInterop;

namespace CricketVerse.Services;

public class ThemeService
{
    private readonly IJSRuntime _jsRuntime;
    private const string StorageKey = "theme";
    public string CurrentTheme { get; private set; } = "light";
    public event Func<Task>? OnThemeChanged;
    private bool _isInitialized;

    public ThemeService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public async Task InitializeTheme()
    {
        if (_isInitialized) return;
        
        try
        {
            var theme = await _jsRuntime.InvokeAsync<string?>("localStorage.getItem", StorageKey);
            if (!string.IsNullOrEmpty(theme))
            {
                CurrentTheme = theme;
                await ApplyTheme(theme);
            }
            _isInitialized = true;
        }
        catch (InvalidOperationException)
        {
            // Ignore JavaScript interop errors during prerendering
        }
    }

    public async Task ToggleTheme()
    {
        CurrentTheme = CurrentTheme == "light" ? "dark" : "light";
        await ApplyTheme(CurrentTheme);
        await _jsRuntime.InvokeVoidAsync("localStorage.setItem", StorageKey, CurrentTheme);
        if (OnThemeChanged != null)
        {
            await OnThemeChanged.Invoke();
        }
    }

    private async Task ApplyTheme(string theme)
    {
        try
        {
            await _jsRuntime.InvokeVoidAsync("document.documentElement.setAttribute", "data-theme", theme);
        }
        catch (InvalidOperationException)
        {
            // Ignore JavaScript interop errors during prerendering
        }
    }
} 