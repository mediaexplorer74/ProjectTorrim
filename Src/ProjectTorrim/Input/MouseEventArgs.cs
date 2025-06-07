
// Type: BrawlerSource.Input.MouseEventArgs
// Assembly: ProjectTorrim, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC8AE86A-8582-48C1-AE0D-A25C0B7D84A4
//

using System.Collections.Generic;

#nullable disable
namespace BrawlerSource.Input
{
  public class MouseEventArgs : BrawlerEventArgs
  {
    public List<MouseButtons> Buttons { get; set; }

    public InputType Type { get; set; }

    public Position Position { get; set; }
  }
}
