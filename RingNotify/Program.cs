using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RingNotify.Camera;
using RingNotify.Chatbot;
using RingNotify.NotifyService;
using RingNotify.Watchdog;
using Serilog;
using System;
using System.Globalization;

namespace RingNotify
{
  class Program
  {
    static void Main()
    {
      try
      {
        CreateHostBuilder().Build().Run();
      }
      catch (Exception ex)
      {
        Log.Fatal(ex, "Uncaught exception! Exiting...");
      }
      finally
      {
        Log.CloseAndFlush();
      }
    }

    private static IHostBuilder CreateHostBuilder()
    {
      return new HostBuilder()
        .UseSerilog((hostingContext, loggerConfiguration) =>
        {
          loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration);
        })
        .ConfigureHostConfiguration(config =>
        {
          config.SetBasePath(AppDomain.CurrentDomain.BaseDirectory);
          config.AddJsonFile("appsettings.json", false, true);
        })
        .ConfigureServices((hostContext, services) =>
        {
          services.AddLogging();
          services.AddTransient<IRingNotifyOptions, RingNotifyOptions>(
            x => CreateRingNotifyOptions(hostContext.Configuration));
          services.AddHostedService<RingNotifyService>();
          services.AddHostedService<WatchdogService>();
        });
    }

    private static RingNotifyOptions CreateRingNotifyOptions(IConfiguration configuration)
    {
      return new RingNotifyOptions(
        new YCamBulletHD720(configuration["camera:ip"],
         configuration["camera:username"], configuration["camera:password"]),
        new TelegramBot(configuration["chatbot:apiToken"]),
        configuration["chatbot:chatId"],
        int.Parse(configuration["gpio:notifyPin"], NumberStyles.Integer, CultureInfo.InvariantCulture));
    }
  }
}
