
// Type: BrawlerSource.Audio.Slider
// Assembly: ProjectTorrim, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC8AE86A-8582-48C1-AE0D-A25C0B7D84A4
//

using BrawlerSource.UI;
using System;

#nullable disable
namespace BrawlerSource.Audio
{
  internal class Slider : BrawlerSource.UI.Slider
  {
    private Channel myTarget;

    public Slider(Layer layer, Position size, Position position, Channel target)
      : base(layer, size, 32, position)
    {
      this.Construct(size, position, target);
    }

    public Slider(GameObject parent, Position size, Position position, Channel target)
      : base(parent, size, 32, position)
    {
      this.Construct(size, position, target);
    }

    protected virtual void Construct(Position size, Position position, Channel target)
    {
      this.myTarget = target;
      this.Cursor.Position = new Position(this.Cursor.MinPosition.X + this.mySize.X * this.myTarget.Volume, this.Cursor.Position.Y);
      this.Value = this.myTarget.Volume;
      this.OnValueChanged += new EventHandler(this.Slider_OnValueChanged);
    }

    private void Slider_OnValueChanged(object sender, EventArgs e)
    {
      this.myTarget.Volume = ((ValueEventArgs) e).Value;
    }
  }
}
