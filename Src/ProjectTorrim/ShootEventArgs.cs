
// Type: BrawlerSource.ShootEventArgs
// Assembly: ProjectTorrim, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC8AE86A-8582-48C1-AE0D-A25C0B7D84A4
//

using Microsoft.Xna.Framework;

#nullable disable
namespace BrawlerSource
{
  public class ShootEventArgs : BrawlerEventArgs
  {
    public Enemy Target;
    public Vector2 Direction;
  }
}
