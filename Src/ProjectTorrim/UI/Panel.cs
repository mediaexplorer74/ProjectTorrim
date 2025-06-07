
// Type: BrawlerSource.UI.Panel
// Assembly: ProjectTorrim, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC8AE86A-8582-48C1-AE0D-A25C0B7D84A4
//

using BrawlerSource.Graphics;
using Microsoft.Xna.Framework;
using System;

#nullable disable
namespace BrawlerSource.UI
{
  public class Panel : Element
  {
    private Vector2 myDimensionPercentage;
    private float myPercentileMultiplier;

    public Panel(Layer layer, Vector2 dimensionPercentage, Align alignment, Position position)
      : base(layer, dimensionPercentage, alignment, position)
    {
      this.Construct(dimensionPercentage, alignment, position);
    }

    public Panel(
      GameObject parent,
      Vector2 dimensionPercentage,
      Align alignment,
      Position position)
      : base(parent, dimensionPercentage, alignment, position)
    {
      this.Construct(dimensionPercentage, alignment, position);
    }

    protected override void Construct(
      Vector2 dimensionPercentage,
      Align alignment,
      Position position)
    {
      this.Layer.ViewCamera.OnResize += new EventHandler(this.ViewCamera_OnResize);
      this.myDimensionPercentage = dimensionPercentage;
      this.myPercentileMultiplier = 0.01f;
      Sprite sprite = new Sprite((GameObject) this);
      sprite.Sequence = new Sequence()
      {
        TexturePath = "Square_256",
        Width = 256,
        Height = 256
      };
      sprite.Position = new Position(0.0f);
      sprite.Scale = Vector2.Divide(this.Scale, 256f);
      this.mySprite = sprite;
      base.Construct(Vector2.Multiply(this.Layer.ViewCamera.WorldSize, Vector2.Multiply(this.myDimensionPercentage, this.myPercentileMultiplier)), alignment, position);
    }

    private void ViewCamera_OnResize(object sender, EventArgs e)
    {
      this.Scale = Vector2.Multiply(this.Layer.ViewCamera.WorldSize, Vector2.Multiply(this.myDimensionPercentage, this.myPercentileMultiplier));
      this.mySprite.Scale = Vector2.Divide(this.Scale, 256f);
    }
  }
}
