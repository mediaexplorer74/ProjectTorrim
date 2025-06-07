
// Type: BrawlerSource.Collision.TouchState
// Assembly: ProjectTorrim, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC8AE86A-8582-48C1-AE0D-A25C0B7D84A4
//

using System;

#nullable disable
namespace BrawlerSource.Collision
{
  [Flags]
  public enum TouchState
  {
    None = 0,
    IsTouching = 1,
    WasTouching = 2,
  }
}
