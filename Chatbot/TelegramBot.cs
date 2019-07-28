using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
using BotClient = Telegram.Bot.TelegramBotClient;

namespace RingNotify.Chatbot
{
  class TelegramBot : IChatbot
  {
    /// <summary>
    /// Telegram Bot Client.
    /// </summary>
    public BotClient Client { get; }

    public TelegramBot(string ApiToken)
    {
      Client = new BotClient(ApiToken);
    }

    public async Task SendMessage(string chatId, string message)
    {
      await Client.SendTextMessageAsync(chatId, message);
    }

    public async Task SendMessage(string chatId, string message, Bitmap image)
    {
      using var stream = new MemoryStream();
      image.Save(stream, ImageFormat.Png);
      stream.Position = 0;
      await Client.SendPhotoAsync(chatId, stream, message);
    }
  }
}
