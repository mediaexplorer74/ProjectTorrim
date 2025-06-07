
// Type: BrawlerSource.Input.KeyEventArgs
// Assembly: ProjectTorrim, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC8AE86A-8582-48C1-AE0D-A25C0B7D84A4
//

using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

#nullable disable
namespace BrawlerSource.Input
{
  public class KeyEventArgs : BrawlerEventArgs
  {
    public List<Keys> Buttons { get; set; }

    public InputType Type { get; set; }
  }
}
