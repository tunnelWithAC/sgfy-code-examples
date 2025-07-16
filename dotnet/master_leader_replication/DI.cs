using Microsoft.Extensions.DependencyInjection;

public void ConfigureServices(IServiceCollection services)
{
    // Other service configurations...
    
    services.AddSingleton<LeaderElectionService>();
    services.AddHostedService(sp => sp.GetRequiredService<LeaderElectionService>());
    
    // Register leader-dependent services
    services.AddScoped<IMasterOnlyOperations, MasterOnlyOperations>();
}
