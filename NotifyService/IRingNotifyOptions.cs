using RingNotify.Camera;
using RingNotify.Chatbot;

namespace RingNotify.NotifyService
{
  interface IRingNotifyOptions
  {
    ICamera Camera { get; }
    IChatbot Chatbot { get; }
    string ChatId { get; }
    int NotifyPin { get; }
  }
}
