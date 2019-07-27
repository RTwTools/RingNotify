using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace RingNotify.Watchdog
{
  /// <summary>
  /// Service in charge of pinging the systemd watchdog.
  /// </summary>
  class WatchdogService : BackgroundService
  {
    /// <summary>
    /// Logger.
    /// </summary>
    public ILogger Logger { get; }

    public WatchdogService(ILogger<WatchdogService> logger)
    {
      Logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
      Logger.LogInformation($"{nameof(WatchdogService)} Started!");

      if (!Init(out ulong intervalMicroSeconds))
      {
        Logger.LogInformation($"{nameof(WatchdogService)} Stopped!");
        return;
      }

      while(!stoppingToken.IsCancellationRequested)
      {
        if (!Notify()) { break; }

        // Ping every requiredInterval / 2 to be on time.
        await Task.Delay((int)(intervalMicroSeconds * 1e-3 / 2), stoppingToken);
      }

      Logger.LogInformation($"{nameof(WatchdogService)} Stopped!");
      return;
    }

    private bool Init(out ulong intervalMicroSeconds)
    {
      switch (WatchdogLib.Enabled(out intervalMicroSeconds))
      {
        case WatchDogResponse.Ok:
          Logger.LogInformation($"Watchdog expect notify messages every {intervalMicroSeconds * 1e-6} seconds.");
          return true;
        case WatchDogResponse.Error:
          Logger.LogError("Couldn't call watchdog_enabled!");
          break;
        case WatchDogResponse.NoActionRequired:
          Logger.LogInformation("Watchdog doesn't expect notify messages!");
          break;
      }

      return false;
    }

    private bool Notify()
    {
      Logger.LogTrace($"{nameof(WatchdogService)} Notifying watchdog.");
      switch (WatchdogLib.Notify())
      {
        case WatchDogResponse.Error:
          Logger.LogError("Couldn't notify the watchdog, is the library installed?");
          return false;
      }

      return true;
    }
  }
}
