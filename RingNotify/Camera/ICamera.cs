using System.Drawing;
using System.Threading.Tasks;

namespace RingNotify.Camera
{
  interface ICamera
  {
    /// <summary>
    /// Get a snapshot.
    /// </summary>
    Task<(bool result, Bitmap snapshot)> SnapShot();
  }
}
