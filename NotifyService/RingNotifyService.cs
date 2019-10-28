using System.Device.Gpio;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RingNotify.Camera;
using RingNotify.Chatbot;

namespace RingNotify.NotifyService
{
  class RingNotifyService : BackgroundService
  {
    private const string NotificationText = "Doorbell Notification";
    private const int DelayAfterRingEventMs = 6000;

    /// <summary>
    /// GPIO input pin for doorbell notifications.
    /// </summary>
    public int NotifyPin { get; }

    /// <summary>
    /// Id of the chat to send notification to.
    /// </summary>
    public string ChatId { get; }

    /// <summary>
    /// Camera to retrieve a screenshot.
    /// </summary>
    private ICamera Camera { get; set; }

    /// <summary>
    /// Chatbot to send a message to notify the user.
    /// </summary>
    public IChatbot Chatbot { get; }

    /// <summary>
    /// Logger.
    /// </summary>
    public ILogger Logger { get; }

    /// <summary>
    /// Gpio controller.
    /// </summary>
    public GpioController GpioController { get; } = new GpioController();

    public RingNotifyService(IRingNotifyOptions options, ILogger<RingNotifyService> logger)
    {
      Camera = options.Camera;
      Chatbot = options.Chatbot;
      ChatId = options.ChatId;
      NotifyPin = options.NotifyPin;
      Logger = logger;

      // Init notify gpio pin.
      GpioController.OpenPin(NotifyPin, PinMode.InputPullDown);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
      Logger.LogInformation($"{nameof(RingNotifyService)} Started!");

      stoppingToken.Register(() =>
      {
        GpioController.ClosePin(NotifyPin);
        Logger.LogInformation($"{nameof(RingNotifyService)} Stopping!");
      });

      while (!stoppingToken.IsCancellationRequested)
      {
        if (GpioController.Read(NotifyPin) == PinValue.High)
        {
          Logger.LogInformation($"{nameof(RingNotifyService)} Received doorbell signal!");
          (bool result, Bitmap screenshot) = await Camera.SnapShot();

          if (result)
          {
            await Chatbot.SendMessage(ChatId, NotificationText, screenshot);
          }
          else
          {
            Logger.LogWarning($"{nameof(RingNotifyService)} Couldn't get screenshot from camera!");
            await Chatbot.SendMessage(ChatId, NotificationText);
          }

          await Task.Delay(DelayAfterRingEventMs, stoppingToken);
        }

        await Task.Delay(50, stoppingToken);
      }

      GpioController.ClosePin(NotifyPin);
      Logger.LogInformation($"{nameof(RingNotifyService)} Stopping!");
    }

    public override void Dispose()
    {
      GpioController.Dispose();
      base.Dispose();
    }
  }
}
