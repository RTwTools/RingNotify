using Xunit;
using RingNotify.Chatbot;
using System.Threading.Tasks;
using System.Drawing;

namespace RingNotify.Tests.Chatbot
{
  /// <summary>
  /// Nuget dependency tests to check if the implementation of the Telegrambot still works and actual messages are sent to the telegram chat.
  /// Jenkins should replace the placeholder appsettings file with the real development appsettings file before running these tests.
  /// </summary>
  [Collection(nameof(TelegramBotTests))]
  public class TelegramBotTests : TestBase
  {
    [Fact]
    public async Task SendImageTest()
    {
      var bot = new TelegramBot(AppSettings["chatbot:apiToken"]); 
      using var image = Image();

      await bot.SendMessage(AppSettings["chatbot:chatId"], $"Unittest: {nameof(SendImageTest)}.", Image());
    }

    [Fact]
    public async Task SendTextTest()
    {
      var bot = new TelegramBot(AppSettings["chatbot:apiToken"]);
      await bot.SendMessage(AppSettings["chatbot:chatId"], $"Unittest: {nameof(SendTextTest)}.");
    }

    private Bitmap Image()
    { 
      var image = new Bitmap(100, 100);
      using var graphics = Graphics.FromImage(image);
      using var brushGreen = new SolidBrush(Color.DarkGreen);
      using var brushBlue = new SolidBrush(Color.DarkBlue);

      graphics.FillRectangle(brushGreen, 0, 0, 100, 50);
      graphics.FillRectangle(brushBlue, 0, 50, 100, 50);

      return image;
    }
  }
}
