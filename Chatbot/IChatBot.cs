using System.Drawing;
using System.Threading.Tasks;

namespace RingNotify.Chatbot
{
  interface IChatbot
  {
    /// <summary>
    /// Send a text message.
    /// </summary>
    /// <param name="chatId">Id of the chat.</param>
    /// <param name="message">Text message.</param>
    Task SendMessage(string chatId, string message);

    /// <summary>
    /// Send an image with an description.
    /// </summary>
    /// <param name="chatId">Id of the chat.</param>
    /// <param name="message">Text message send with the image.</param>
    /// <param name="image">Image.</param>
    Task SendMessage(string chatId, string message, Bitmap image);
  }
}
