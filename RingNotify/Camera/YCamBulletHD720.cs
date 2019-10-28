using System;
using System.Drawing;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RingNotify.Camera
{
  class YCamBulletHD720 : ICamera
  {
    /// <summary>
    /// Ip-address.
    /// </summary>
    private string Ip { get; set; }

    /// <summary>
    /// Username.
    /// </summary>
    private string Username { get; set; }

    /// <summary>
    /// Password.
    /// </summary>
    private string Password { get; set; }

    /// <summary>
    /// Http client.
    /// </summary>
    private HttpClient Client
    {
      get
      {
        var auth = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{Username}:{Password}"));
        var client = new HttpClient();
        client.DefaultRequestHeaders.Authorization = 
          new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", auth);

        return client;
      }
    }

    public YCamBulletHD720(string ip, string username, string password)
    {
      Ip = ip;
      Username = username;
      Password = password;
    }

    /// <summary>
    /// Get a snapshot from the camera.
    /// </summary>
    /// <returns>
    /// bool result -> True when succeeded to receive snapshot.
    /// Bitmap snapshot -> Snapshot when result is true, null when false.
    /// </returns>
    public async Task<(bool result, Bitmap snapshot)> SnapShot()
    {
      string url = $"http://{Ip}/snapshot.jpg";
      using var client = Client;
      using var response = await client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);

      if (!response.IsSuccessStatusCode) { return (false, null); }

      using var stream = await response.Content.ReadAsStreamAsync();
      return (true, new Bitmap(stream));
    }
  }
}
