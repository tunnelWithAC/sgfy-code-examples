using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

public class LeaderElectionService : BackgroundService
{
    private readonly ILogger<LeaderElectionService> _logger;
    private readonly IDbContextFactory<AppDbContext> _contextFactory;
    private readonly int _lockId = 1000; // Arbitrary lock ID
    private Timer _heartbeatTimer;
    private bool _isLeader = false;
    
    public bool IsLeader => _isLeader;
    
    public LeaderElectionService(
        ILogger<LeaderElectionService> logger,
        IDbContextFactory<AppDbContext> contextFactory)
    {
        _logger = logger;
        _contextFactory = contextFactory;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // Try to acquire leadership immediately
        await TryAcquireLeadershipAsync();
        
        // Set up periodic heartbeat/leadership check
        _heartbeatTimer = new Timer(async _ => 
        {
            await TryAcquireLeadershipAsync();
        }, null, TimeSpan.Zero, TimeSpan.FromSeconds(10));
        
        await stoppingToken.AsTask();
        
        // Release resources
        _heartbeatTimer?.Dispose();
        await ReleaseLeadershipAsync();
    }
    
    private async Task<bool> TryAcquireLeadershipAsync()
    {
        try
        {
            using var dbContext = await _contextFactory.CreateDbContextAsync();
            
            // Try to acquire an advisory lock (non-blocking)
            var result = await dbContext.Database
                .ExecuteSqlRawAsync("SELECT pg_try_advisory_lock({0});", _lockId)
                .ConfigureAwait(false);
            
            // Since ExecuteSqlRaw returns affected rows (0), 
            // we need to query the result directly
            var lockAcquired = await dbContext.Database
                .SqlQueryRaw<bool>("SELECT pg_try_advisory_lock({0}) as Result;", _lockId)
                .FirstOrDefaultAsync()
                .ConfigureAwait(false);
                
            var wasLeader = _isLeader;
            _isLeader = lockAcquired;
            
            if (_isLeader && !wasLeader)
            {
                _logger.LogInformation("This instance is now the leader");
            }
            else if (!_isLeader && wasLeader)
            {
                _logger.LogInformation("This instance is no longer the leader");
            }
            
            return _isLeader;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while attempting to acquire leadership");
            _isLeader = false;
            return false;
        }
    }
    
    private async Task ReleaseLeadershipAsync()
    {
        if (!_isLeader) return;
        
        try
        {
            using var dbContext = await _contextFactory.CreateDbContextAsync();
            
            // Release the advisory lock
            await dbContext.Database
                .ExecuteSqlRawAsync("SELECT pg_advisory_unlock({0});", _lockId)
                .ConfigureAwait(false);
                
            _isLeader = false;
            _logger.LogInformation("Leadership released");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error releasing leadership lock");
        }
    }
}

// Define the DbContext class
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) 
        : base(options)
    {
    }
    
    // Add your DbSets here if needed
    // public DbSet<YourEntity> YourEntities { get; set; }
}