using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CricketVerse.Data;
using CricketVerse.Models;

namespace CricketVerse.Services
{
    public class PlayerService
    {
        private readonly CricketVerseContext _context;

        public PlayerService(CricketVerseContext context)
        {
            _context = context;
        }

        public async Task<List<Player>> GetAllPlayersAsync()
        {
            return await _context.Players.ToListAsync();
        }

        public async Task<Player> GetPlayerByIdAsync(int id)
        {
            return await _context.Players.FindAsync(id) ?? throw new KeyNotFoundException($"Player with ID {id} not found");
        }

        public async Task<List<Player>> GetPlayersByTeamAsync(string teamCode)
        {
            return await _context.Players
                .Where(p => p.Team == teamCode)
                .ToListAsync();
        }

        public async Task<List<Player>> GetPlayersByRoleAsync(string role)
        {
            return await _context.Players
                .Where(p => p.Role == role)
                .ToListAsync();
        }

        public async Task<List<Player>> SearchPlayersAsync(string query, string teamFilter = "", string roleFilter = "", string sortBy = "points")
        {
            var players = _context.Players.AsQueryable();

            // Apply filters
            if (!string.IsNullOrWhiteSpace(query))
            {
                players = players.Where(p => 
                    p.Name.Contains(query) || 
                    p.Team.Contains(query) || 
                    p.Role.Contains(query));
            }

            if (!string.IsNullOrWhiteSpace(teamFilter))
            {
                players = players.Where(p => p.Team == teamFilter);
            }

            if (!string.IsNullOrWhiteSpace(roleFilter))
            {
                players = players.Where(p => p.Role == roleFilter);
            }

            // Apply sorting
            players = sortBy.ToLower() switch
            {
                "points" => players.OrderByDescending(p => p.Points),
                "price" => players.OrderByDescending(p => p.Price),
                "name" => players.OrderBy(p => p.Name),
                _ => players.OrderByDescending(p => p.Points)
            };

            return await players.ToListAsync();
        }

        // Method to seed players if the database is empty
        public async Task SeedPlayersAsync()
        {
            if (!await _context.Players.AnyAsync())
            {
                var players = new List<Player>
                {
                    // Islamabad United
                    new Player { Name = "Shadab Khan", Role = "All-rounder", Team = "ISL", Country = "Pakistan", Price = 15.5M, Points = 850 },
                    new Player { Name = "Alex Hales", Role = "Batsman", Team = "ISL", Country = "England", Price = 14.0M, Points = 780 },
                    new Player { Name = "Faheem Ashraf", Role = "All-rounder", Team = "ISL", Country = "Pakistan", Price = 12.5M, Points = 720 },
                    new Player { Name = "Azam Khan", Role = "Wicket-keeper", Team = "ISL", Country = "Pakistan", Price = 11.0M, Points = 680 },
                    new Player { Name = "Colin Munro", Role = "Batsman", Team = "ISL", Country = "New Zealand", Price = 13.5M, Points = 760 },
                    
                    // Karachi Kings
                    new Player { Name = "Mohammad Amir", Role = "Bowler", Team = "KAR", Country = "Pakistan", Price = 13.5M, Points = 750 },
                    new Player { Name = "Imad Wasim", Role = "All-rounder", Team = "KAR", Country = "Pakistan", Price = 13.0M, Points = 740 },
                    new Player { Name = "James Vince", Role = "Batsman", Team = "KAR", Country = "England", Price = 12.5M, Points = 710 },
                    new Player { Name = "Hasan Ali", Role = "Bowler", Team = "KAR", Country = "Pakistan", Price = 12.0M, Points = 700 },
                    new Player { Name = "Jimmy", Role = "Batsman", Team = "KAR", Country = "England", Price = 17.0M, Points = 900 },
                    
                    // Lahore Qalandars
                    new Player { Name = "Shaheen Afridi", Role = "Bowler", Team = "LAH", Country = "Pakistan", Price = 16.0M, Points = 880 },
                    new Player { Name = "Fakhar Zaman", Role = "Batsman", Team = "LAH", Country = "Pakistan", Price = 13.0M, Points = 760 },
                    new Player { Name = "Haris Rauf", Role = "Bowler", Team = "LAH", Country = "Pakistan", Price = 14.5M, Points = 800 },
                    new Player { Name = "David Wiese", Role = "All-rounder", Team = "LAH", Country = "Namibia", Price = 12.0M, Points = 720 },
                    new Player { Name = "Sikandar Raza", Role = "All-rounder", Team = "LAH", Country = "Zimbabwe", Price = 11.5M, Points = 690 },
                    
                    // Multan Sultans
                    new Player { Name = "Mohammad Rizwan", Role = "Wicket-keeper", Team = "MUL", Country = "Pakistan", Price = 15.0M, Points = 840 },
                    new Player { Name = "Rilee Rossouw", Role = "Batsman", Team = "MUL", Country = "South Africa", Price = 12.5M, Points = 730 },
                    new Player { Name = "Khushdil Shah", Role = "All-rounder", Team = "MUL", Country = "Pakistan", Price = 11.0M, Points = 680 },
                    new Player { Name = "Shan Masood", Role = "Batsman", Team = "MUL", Country = "Pakistan", Price = 11.5M, Points = 670 },
                    new Player { Name = "Abbas Afridi", Role = "Bowler", Team = "MUL", Country = "Pakistan", Price = 10.5M, Points = 650 },
                    
                    // Peshawar Zalmi
                    new Player { Name = "Wahab Riaz", Role = "Bowler", Team = "PES", Country = "Pakistan", Price = 12.0M, Points = 700 },
                    new Player { Name = "Tom Kohler-Cadmore", Role = "Wicket-keeper", Team = "PES", Country = "England", Price = 11.5M, Points = 690 },
                    new Player { Name = "Saim Ayub", Role = "Batsman", Team = "PES", Country = "Pakistan", Price = 10.0M, Points = 620 },
                    new Player { Name = "Jimmy Neesham", Role = "All-rounder", Team = "PES", Country = "New Zealand", Price = 12.5M, Points = 730 },
                    
                    // Quetta Gladiators
                    new Player { Name = "Mohammad Nawaz", Role = "All-rounder", Team = "QUE", Country = "Pakistan", Price = 13.0M, Points = 750 },
                    new Player { Name = "Jason Roy", Role = "Batsman", Team = "QUE", Country = "England", Price = 14.5M, Points = 820 },
                    new Player { Name = "Naseem Shah", Role = "Bowler", Team = "QUE", Country = "Pakistan", Price = 13.5M, Points = 770 },
                    new Player { Name = "Mohammad Hasnain", Role = "Bowler", Team = "QUE", Country = "Pakistan", Price = 11.0M, Points = 660 },
                    new Player { Name = "Will Smeed", Role = "Batsman", Team = "QUE", Country = "England", Price = 10.5M, Points = 640 }
                };

                await _context.Players.AddRangeAsync(players);
                await _context.SaveChangesAsync();
            }
        }
    }
} 