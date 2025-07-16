using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

public interface IMasterOnlyOperations
{
    Task PerformOperationAsync();
    // Other operations that should only be executed by the leader
}

public class MasterOnlyOperations : IMasterOnlyOperations
{
    private readonly LeaderElectionService _leaderElection;
    private readonly ILogger<MasterOnlyOperations> _logger;
    
    public MasterOnlyOperations(
        LeaderElectionService leaderElection,
        ILogger<MasterOnlyOperations> logger)
    {
        _leaderElection = leaderElection;
        _logger = logger;
    }
    
    public async Task PerformOperationAsync()
    {
        // Check if this instance is the leader
        if (!_leaderElection.IsLeader)
        {
            _logger.LogInformation("Skipping operation as this instance is not the leader");
            return;
        }
        
        // Perform leader-only operations
        _logger.LogInformation("Performing leader-only operation");
        
        // Implement your actual operation logic here
        await Task.Delay(1000); // Simulate work
    }
}
