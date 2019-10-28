using System;
using System.Runtime.InteropServices;

namespace RingNotify.Watchdog
{
  class WatchdogLib
  {
    /// <summary>
    /// Check if the service manager expects keep-alive notifications.
    /// </summary>
    /// <param name="usec">Interval of expected notifcations.</param>
    /// <returns>True -> Notification are expected.</returns>
    public static WatchDogResponse Enabled(out ulong usec)
    {
      try
      {
        return sd_watchdog_enabled(0, out usec).ToWatchDogResponse();
      }
      catch (DllNotFoundException)
      {
        usec = 0;
        return WatchDogResponse.Error;
      }
    }

    /// <summary>
    /// Tells the service manager to update the watchdog timestamp.
    /// This is the keep-alive ping that services need to issue in regular intervals.
    /// </summary>
    public static WatchDogResponse Notify()
    {
      try
      {
        return sd_notify(0, "WATCHDOG=1").ToWatchDogResponse();
      }
      catch (DllNotFoundException)
      {
        return WatchDogResponse.Error;
      }
    }

    /// <summary>
    /// sd_watchdog_enabled — Check whether the service manager expects watchdog keep-alive notifications from a service
    /// https://www.freedesktop.org/software/systemd/man/sd_watchdog_enabled.html
    /// </summary>
    /// <param name="unset_environment"></param>
    /// <param name="usec"></param>
    /// <returns>
    /// On failure, this call returns a negative errno-style error code. 
    /// If the service manager expects watchdog keep-alive notification messages to be sent,
    ///  > 0 is returned, otherwise 0 is returned. Only if the return value is > 0,
    ///  the usec parameter is valid after the call.
    /// </returns>
    [DllImport("libsystemd.so.0")]
    private static extern int sd_watchdog_enabled(int unset_environment, out ulong usec);

    /// <summary>
    /// sd_notify() may be called by a service to notify the service manager about state changes. It can be used to
    ///  send arbitrary information, encoded in an environment-block-like string.
    ///  Most importantly, it can be used for start-up completion notification.
    /// https://www.freedesktop.org/software/systemd/man/sd_notify.html
    /// </summary>
    /// <param name="unset_environment"></param>
    /// <param name="state"></param>
    /// <returns>
    /// On failure, these calls return a negative errno-style error code. If $NOTIFY_SOCKET
    ///  was not set and hence no status message could be sent, 0 is returned.
    ///  If the status was sent, these functions return a positive value.
    /// </returns>
    [DllImport("libsystemd.so.0")]
    private static extern int sd_notify(int unset_environment, string state);

  }
}
