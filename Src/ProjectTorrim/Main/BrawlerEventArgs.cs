
// Type: BrawlerSource.BrawlerEventArgs
// Assembly: ProjectTorrim, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC8AE86A-8582-48C1-AE0D-A25C0B7D84A4
//

using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

#nullable disable
namespace BrawlerSource
{
  public class BrawlerEventArgs : EventArgs
  {
    public GameTime GameTime { get; set; }

    public List<object> Args { get; set; }
  }
}
