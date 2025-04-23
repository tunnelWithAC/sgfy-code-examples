using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

public class ScheduledJobService : BackgroundService
{
    private readonly IMasterOnlyOperations _masterOps;
    private readonly LeaderElectionService _leaderElection;
    private Timer _timer;
    
    public ScheduledJobService(
        IMasterOnlyOperations masterOps,
        LeaderElectionService leaderElection)
    {
        _masterOps = masterOps;
        _leaderElection = leaderElection;
    }
    
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _timer = new Timer(async _ => 
        {
            if (_leaderElection.IsLeader)
            {
                await _masterOps.PerformOperationAsync();
            }
        }, null, TimeSpan.Zero, TimeSpan.FromMinutes(5));
        
        return stoppingToken.AsTask();
    }
    
    public override void Dispose()
    {
        _timer?.Dispose();
        base.Dispose();
    }
}
