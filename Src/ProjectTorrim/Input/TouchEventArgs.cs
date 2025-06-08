// Type: BrawlerSource.Input.TouchEventArgs
// Assembly: ProjectTorrim, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC8AE86A-8582-48C1-AE0D-A25C0B7D84A4

using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace BrawlerSource.Input
{
    public class TouchEventArgs : BrawlerEventArgs
    {
        public List<int> TouchIds { get; set; }
        public InputType Type { get; set; }
        public Position Position { get; set; }
    }
}