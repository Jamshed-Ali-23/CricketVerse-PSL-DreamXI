using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CricketVerse.Data;
using CricketVerse.Models;

namespace CricketVerse.Services
{
    public class TeamService
    {
        private readonly CricketVerseContext _context;
        private readonly IUserContext _userContext;

        public TeamService(CricketVerseContext context, IUserContext userContext)
        {
            _context = context;
            _userContext = userContext;
        }

        public async Task<List<Team>> GetUserTeamsAsync()
        {
            if (!int.TryParse(_userContext.UserId, out int userId))
            {
                return new List<Team>();
            }

            return await _context.Teams
                .Where(t => t.UserId == userId)
                .Include(t => t.Players)
                .Include(t => t.Captain)
                .Include(t => t.ViceCaptain)
                .ToListAsync();
        }

        public async Task<Team> GetTeamByIdAsync(int id)
        {
            return await _context.Teams
                .Include(t => t.Players)
                .Include(t => t.Captain)
                .Include(t => t.ViceCaptain)
                .FirstOrDefaultAsync(t => t.Id == id) 
                ?? throw new KeyNotFoundException($"Team with ID {id} not found");
        }

        public async Task<Team> CreateTeamAsync(Team team, IEnumerable<int> playerIds, int captainId, int viceCaptainId)
        {
            if (!int.TryParse(_userContext.UserId, out int userId))
            {
                throw new UnauthorizedAccessException("User is not authenticated");
            }

            // Set the user ID
            team.UserId = userId;
            team.CreatedAt = DateTime.UtcNow;
            team.IsComplete = true;
            
            // Get all players that will be in the team
            var players = await _context.Players
                .Where(p => playerIds.Contains(p.Id))
                .ToListAsync();

            if (players.Count != playerIds.Count())
            {
                throw new ArgumentException("One or more player IDs are invalid");
            }

            var captain = players.FirstOrDefault(p => p.Id == captainId);
            var viceCaptain = players.FirstOrDefault(p => p.Id == viceCaptainId);

            if (captain == null || viceCaptain == null)
            {
                throw new ArgumentException("Invalid captain or vice-captain ID");
            }

            team.Players = players;
            team.CaptainId = captainId;
            team.ViceCaptainId = viceCaptainId;
            
            // Calculate team points
            team.Points = players.Sum(p => p.Points);
            
            _context.Teams.Add(team);
            await _context.SaveChangesAsync();

            return team;
        }

        public async Task<bool> DeleteTeamAsync(int teamId)
        {
            if (!int.TryParse(_userContext.UserId, out int userId))
            {
                throw new UnauthorizedAccessException("User is not authenticated");
            }

            var team = await _context.Teams
                .FirstOrDefaultAsync(t => t.Id == teamId && t.UserId == userId);

            if (team == null)
            {
                return false;
            }

            _context.Teams.Remove(team);
            await _context.SaveChangesAsync();
            return true;
        }

        public bool ValidateTeamComposition(IEnumerable<Player> players)
        {
            // Ensure we have exactly 11 players
            if (players.Count() != 11)
                return false;

            var roleCount = players.GroupBy(p => p.Role)
                                  .ToDictionary(g => g.Key, g => g.Count());

            // Validate minimum requirements
            if (!roleCount.TryGetValue("Batsman", out int batsmen) || batsmen < 3)
                return false;
            
            if (!roleCount.TryGetValue("Bowler", out int bowlers) || bowlers < 3)
                return false;
            
            if (!roleCount.TryGetValue("All-rounder", out int allRounders) || allRounders < 1)
                return false;
            
            if (!roleCount.TryGetValue("Wicket-keeper", out int keepers) || keepers != 1)
                return false;

            // Validate team composition - no more than 4 players from a single team
            var teamCount = players.GroupBy(p => p.Team)
                                   .ToDictionary(g => g.Key, g => g.Count());

            if (teamCount.Values.Any(count => count > 4))
                return false;

            return true;
        }

        public bool CanSelectPlayer(Player player, List<Player> selectedPlayers, decimal remainingBudget)
        {
            if (selectedPlayers.Count >= 11)
                return false;
            
            if (player.Price > remainingBudget)
                return false;

            var teamCount = selectedPlayers.Count(p => p.Team == player.Team);
            if (teamCount >= 4)
                return false;

            var roleCount = selectedPlayers.Count(p => p.Role == player.Role);
            return player.Role switch
            {
                "Batsman" => roleCount < 4,
                "Bowler" => roleCount < 4,
                "All-rounder" => roleCount < 2,
                "Wicket-keeper" => roleCount < 1,
                _ => false
            };
        }
    }
} 