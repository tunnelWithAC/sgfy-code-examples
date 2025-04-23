using Npgsql;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

public class LeaderElectionService : BackgroundService
{
    private readonly ILogger<LeaderElectionService> _logger;
    private readonly string _connectionString;
    private readonly int _lockId = 1000; // Arbitrary lock ID
    private Timer _heartbeatTimer;
    private NpgsqlConnection _connection;
    private bool _isLeader = false;
    
    public bool IsLeader => _isLeader;
    
    public LeaderElectionService(ILogger<LeaderElectionService> logger, string connectionString)
    {
        _logger = logger;
        _connectionString = connectionString;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _connection = new NpgsqlConnection(_connectionString);
        await _connection.OpenAsync(stoppingToken);
        
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
        await _connection.CloseAsync();
    }
    
    private async Task<bool> TryAcquireLeadershipAsync()
    {
        try
        {
            // Try to acquire an advisory lock (non-blocking)
            using var cmd = new NpgsqlCommand("SELECT pg_try_advisory_lock(@lockId);", _connection);
            cmd.Parameters.AddWithValue("lockId", _lockId);
            var result = await cmd.ExecuteScalarAsync();
            
            var wasLeader = _isLeader;
            _isLeader = (bool)result;
            
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
            // Release the advisory lock
            using var cmd = new NpgsqlCommand("SELECT pg_advisory_unlock(@lockId);", _connection);
            cmd.Parameters.AddWithValue("lockId", _lockId);
            await cmd.ExecuteScalarAsync();
            _isLeader = false;
            _logger.LogInformation("Leadership released");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error releasing leadership lock");
        }
    }
}
