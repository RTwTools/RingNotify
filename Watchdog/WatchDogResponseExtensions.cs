namespace RingNotify.Watchdog
{
  static class WatchDogResponseExtensions
  {
    public static WatchDogResponse ToWatchDogResponse(this int response)
    {
      if (response < 0)
      {
        return WatchDogResponse.Error;
      }
      else if (response == 0)
      {
        return WatchDogResponse.NoActionRequired;
      }
      else
      {
        return WatchDogResponse.Ok;
      }
    }
  }
}
