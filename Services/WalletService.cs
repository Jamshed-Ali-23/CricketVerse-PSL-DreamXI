using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CricketVerse.Models;
using CricketVerse.Data;

namespace CricketVerse.Services;

public class WalletService
{
    private readonly IUserContext _userContext;
    private readonly CricketVerseContext _context;
    public event Action? OnBalanceChanged;

    public WalletService(IUserContext userContext, CricketVerseContext context)
    {
        _userContext = userContext;
        _context = context;
    }

    public async Task<decimal> GetBalanceAsync()
    {
        var userIdStr = _userContext.UserId;
        if (string.IsNullOrEmpty(userIdStr) || !int.TryParse(userIdStr, out var userId)) return 0;

        var user = await _context.Users.FindAsync(userId);
        return user?.Balance ?? 0;
    }

    public decimal GetBalance()
    {
        var userIdStr = _userContext.UserId;
        if (string.IsNullOrEmpty(userIdStr) || !int.TryParse(userIdStr, out var userId)) return 0;

        var user = _context.Users.Find(userId);
        return user?.Balance ?? 0;
    }

    public async Task<List<Transaction>> GetTransactionsAsync()
    {
        var userIdStr = _userContext.UserId;
        if (string.IsNullOrEmpty(userIdStr) || !int.TryParse(userIdStr, out var userId)) 
            return new List<Transaction>();

        return await _context.Transactions
            .Where(t => t.UserId == userId)
            .OrderByDescending(t => t.Timestamp)
            .ToListAsync();
    }

    public List<Transaction> GetTransactions()
    {
        var userIdStr = _userContext.UserId;
        if (string.IsNullOrEmpty(userIdStr) || !int.TryParse(userIdStr, out var userId)) 
            return new List<Transaction>();

        return _context.Transactions
            .Where(t => t.UserId == userId)
            .OrderByDescending(t => t.Timestamp)
            .ToList();
    }

    public async Task<bool> HasSufficientFundsAsync(decimal amount)
    {
        var userIdStr = _userContext.UserId;
        if (string.IsNullOrEmpty(userIdStr) || !int.TryParse(userIdStr, out var userId)) return false;

        var user = await _context.Users.FindAsync(userId);
        return user?.Balance >= amount;
    }

    public bool HasSufficientFunds(decimal amount)
    {
        var userIdStr = _userContext.UserId;
        if (string.IsNullOrEmpty(userIdStr) || !int.TryParse(userIdStr, out var userId)) return false;

        var user = _context.Users.Find(userId);
        return user?.Balance >= amount;
    }

    public async Task<bool> AddFundsAsync(decimal amount, string description)
    {
        var userIdStr = _userContext.UserId;
        if (string.IsNullOrEmpty(userIdStr) || !int.TryParse(userIdStr, out var userId) || amount <= 0) 
            return false;

        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return false;

            // Update user balance
            user.Balance += amount;
            
            // Create transaction record
            var transactionRecord = new Transaction
            {
                Amount = amount,
                Description = description,
                Timestamp = DateTime.UtcNow,
                Type = TransactionType.Deposit,
                Balance = user.Balance,
                UserId = userId
            };
            
            await _context.Transactions.AddAsync(transactionRecord);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            OnBalanceChanged?.Invoke();
            return true;
        }
        catch
        {
            await transaction.RollbackAsync();
            return false;
        }
    }

    public bool AddFunds(decimal amount, string description)
    {
        var userIdStr = _userContext.UserId;
        if (string.IsNullOrEmpty(userIdStr) || !int.TryParse(userIdStr, out var userId) || amount <= 0) 
            return false;

        using var transaction = _context.Database.BeginTransaction();
        try
        {
            var user = _context.Users.Find(userId);
            if (user == null) return false;

            // Update user balance
            user.Balance += amount;
            
            // Create transaction record
            var transactionRecord = new Transaction
            {
                Amount = amount,
                Description = description,
                Timestamp = DateTime.UtcNow,
                Type = TransactionType.Deposit,
                Balance = user.Balance,
                UserId = userId
            };
            
            _context.Transactions.Add(transactionRecord);
            _context.SaveChanges();
            transaction.Commit();

            OnBalanceChanged?.Invoke();
            return true;
        }
        catch
        {
            transaction.Rollback();
            return false;
        }
    }

    public async Task<bool> DeductFundsAsync(decimal amount, string description)
    {
        var userIdStr = _userContext.UserId;
        if (string.IsNullOrEmpty(userIdStr) || !int.TryParse(userIdStr, out var userId) || amount <= 0) 
            return false;

        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null || user.Balance < amount) return false;

            // Update user balance
            user.Balance -= amount;
            
            // Create transaction record
            var transactionRecord = new Transaction
            {
                Amount = amount,
                Description = description,
                Timestamp = DateTime.UtcNow,
                Type = TransactionType.Withdrawal,
                Balance = user.Balance,
                UserId = userId
            };
            
            await _context.Transactions.AddAsync(transactionRecord);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            OnBalanceChanged?.Invoke();
            return true;
        }
        catch
        {
            await transaction.RollbackAsync();
            return false;
        }
    }

    public bool DeductFunds(decimal amount, string description)
    {
        var userIdStr = _userContext.UserId;
        if (string.IsNullOrEmpty(userIdStr) || !int.TryParse(userIdStr, out var userId) || amount <= 0) 
            return false;

        using var transaction = _context.Database.BeginTransaction();
        try
        {
            var user = _context.Users.Find(userId);
            if (user == null || user.Balance < amount) return false;

            // Update user balance
            user.Balance -= amount;
            
            // Create transaction record
            var transactionRecord = new Transaction
            {
                Amount = amount,
                Description = description,
                Timestamp = DateTime.UtcNow,
                Type = TransactionType.Withdrawal,
                Balance = user.Balance,
                UserId = userId
            };
            
            _context.Transactions.Add(transactionRecord);
            _context.SaveChanges();
            transaction.Commit();

            OnBalanceChanged?.Invoke();
            return true;
        }
        catch
        {
            transaction.Rollback();
            return false;
        }
    }

    // Additional methods to fetch statistics
    public async Task<decimal> GetTotalDepositsAsync(int? days = null)
    {
        var userIdStr = _userContext.UserId;
        if (string.IsNullOrEmpty(userIdStr) || !int.TryParse(userIdStr, out var userId)) 
            return 0;

        var query = _context.Transactions
            .Where(t => t.UserId == userId && t.Type == TransactionType.Deposit);
            
        if (days.HasValue)
        {
            var startDate = DateTime.UtcNow.AddDays(-days.Value);
            query = query.Where(t => t.Timestamp >= startDate);
        }

        return await query.SumAsync(t => t.Amount);
    }

    public async Task<decimal> GetTotalSpentAsync(int? days = null)
    {
        var userIdStr = _userContext.UserId;
        if (string.IsNullOrEmpty(userIdStr) || !int.TryParse(userIdStr, out var userId)) 
            return 0;

        var query = _context.Transactions
            .Where(t => t.UserId == userId && t.Type == TransactionType.Withdrawal);
            
        if (days.HasValue)
        {
            var startDate = DateTime.UtcNow.AddDays(-days.Value);
            query = query.Where(t => t.Timestamp >= startDate);
        }

        return await query.SumAsync(t => t.Amount);
    }

    public async Task<int> GetTeamsCountAsync()
    {
        var userIdStr = _userContext.UserId;
        if (string.IsNullOrEmpty(userIdStr) || !int.TryParse(userIdStr, out var userId)) 
            return 0;

        return await _context.Teams
            .CountAsync(t => t.UserId == userId);
    }
} 