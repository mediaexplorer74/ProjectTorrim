
// Type: BrawlerSource.UI.PercentageBar
// Assembly: ProjectTorrim, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC8AE86A-8582-48C1-AE0D-A25C0B7D84A4
//

using BrawlerSource.Graphics;
using Microsoft.Xna.Framework;
using System;

#nullable disable
namespace BrawlerSource.UI
{
  public class PercentageBar : DrawableGameObject
  {
    private Sprite myRedSprite;
    private Sprite myGreenSprite;
    public int Value;
    private float myAmount;
    public int Maximum;
    public Position Dimensions;
    private Align myAlignment;

    public PercentageBar(
      GameObject parent,
      Position position,
      Position dimensions,
      int maximum,
      Align alignment)
      : base(parent)
    {
      this.Position = position;
      this.Dimensions = dimensions;
      this.Maximum = maximum;
      this.myAlignment = alignment;
      Sprite sprite1 = new Sprite((GameObject) this);
      sprite1.Sequence = new Sequence()
      {
        TexturePath = "Square_256",
        Width = 256,
        Height = 256
      };
      sprite1.Position = this.Position;
      sprite1.Scale = (this.Dimensions / 256f).ToVector2();
      sprite1.Colour = Color.Red;
      this.myRedSprite = sprite1;
      Sprite sprite2 = new Sprite((GameObject) this);
      sprite2.Sequence = new Sequence()
      {
        TexturePath = "Square_256",
        Width = 256,
        Height = 256
      };
      sprite2.Position = this.Position;
      sprite2.Scale = (this.Dimensions / 256f).ToVector2();
      sprite2.Depth = this.Depth;
      sprite2.Colour = Color.Green;
      this.myGreenSprite = sprite2;
    }

    public override void FirstUpdate(GameTime gameTime)
    {
      this.myRedSprite.Depth = this.Depth - 0.1f;
      this.myGreenSprite.Depth = this.Depth;
      base.FirstUpdate(gameTime);
    }

    public override void Update(GameTime gameTime)
    {
      this.myAmount = (float) this.Value / (float) this.Maximum;
      Vector2 vector2= new Vector2(!this.myAlignment.HasFlag((Enum) Align.Top) 
          && !this.myAlignment.HasFlag((Enum) Align.Bottom) 
          || this.myAlignment.HasFlag((Enum) Align.Left) 
          || this.myAlignment.HasFlag((Enum) Align.Left) 
          ? this.myAmount : 1f, !this.myAlignment.HasFlag((Enum) Align.Left)
          && !this.myAlignment.HasFlag((Enum) Align.Right) 
          || this.myAlignment.HasFlag((Enum) Align.Top) 
          || this.myAlignment.HasFlag((Enum) Align.Bottom) ? this.myAmount : 1f);

      this.myGreenSprite.Scale = Vector2.Multiply((this.Dimensions / 256f).ToVector2(), vector2);
      Position position = new Position(this.myAlignment.HasFlag((Enum) Align.Left) ? -1f
          : (this.myAlignment.HasFlag((Enum) Align.Right) ? 1f : 0.0f), 
          this.myAlignment.HasFlag((Enum) Align.Top) ? -1f : (this.myAlignment.HasFlag((Enum) Align.Bottom) ? 1f : 0.0f));
      this.myGreenSprite.Position = this.Position + 
                this.Dimensions / 2f * position + new Position(this.myGreenSprite.Scale)
                * 128f * -position;

      this.myRedSprite.Position = this.Position;
      this.myRedSprite.Scale = (this.Dimensions / 256f).ToVector2();
      base.Update(gameTime);
    }
  }
}
