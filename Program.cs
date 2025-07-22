using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using CricketVerse.Services;
using CricketVerse.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

// Add DbContext with SQLite and retry policy
builder.Services.AddDbContext<CricketVerseContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"),
        sqliteOptions => sqliteOptions.CommandTimeout(60)));

// Add services
builder.Services.AddScoped<LanguageService>();
builder.Services.AddScoped<ThemeService>();
builder.Services.AddScoped<UserContextService>();
builder.Services.AddScoped<IUserContext>(sp => sp.GetRequiredService<UserContextService>());
builder.Services.AddScoped<WalletService>();
builder.Services.AddScoped<ToastService>();
builder.Services.AddScoped<PlayerService>();
builder.Services.AddScoped<TeamService>();

// Add authentication services
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<CustomAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(sp => sp.GetRequiredService<CustomAuthenticationStateProvider>());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

// Use try-catch to handle database seeding errors
try
{
    // Seed the database with players if needed
    using (var scope = app.Services.CreateScope())
    {
        var playerService = scope.ServiceProvider.GetRequiredService<PlayerService>();
        await playerService.SeedPlayersAsync();
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Error seeding database: {ex.Message}");
    // Continue anyway
}

app.Run();
