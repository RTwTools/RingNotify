using RingNotify.Camera;
using RingNotify.Chatbot;

namespace RingNotify.NotifyService
{
  class RingNotifyOptions : IRingNotifyOptions
  {
    public ICamera Camera { get; }
    public IChatbot Chatbot { get; }
    public string ChatId { get; }
    public int NotifyPin { get; }

    public RingNotifyOptions(ICamera camera, IChatbot chatbot, string chatId, int notifyPin)
    {
      Camera = camera;
      Chatbot = chatbot;
      ChatId = chatId;
      NotifyPin = notifyPin;
    }
  }
}
