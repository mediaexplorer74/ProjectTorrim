
// Type: BrawlerSource.UI.Element
// Assembly: ProjectTorrim, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC8AE86A-8582-48C1-AE0D-A25C0B7D84A4
//

using BrawlerSource.Graphics;
using Microsoft.Xna.Framework;
using System;

#nullable disable
namespace BrawlerSource.UI
{
  public class Element : DrawableGameObject
  {
    public Position Offset;
    private Element myAnchor;
    private Align myAlignment;
    public Sprite mySprite;

    public event EventHandler OnRealign;

    public Element Anchor
    {
      get => this.myAnchor;
      set
      {
        this.myAnchor = value;
        if (this.myAnchor == null)
          return;
        this.Offset = this.Position - this.myAnchor.Position;
      }
    }

    public Align Alignment
    {
      get => this.myAlignment;
      set
      {
        this.myAlignment = value;
        this.Position = new Position();
        if (this.myAlignment.HasFlag((Enum) Align.Top))
          this.Position.Y = this.Layer.ViewCamera.TopLeft.Y + this.Scale.Y / 2f;
        if (this.myAlignment.HasFlag((Enum) Align.Bottom))
          this.Position.Y = this.Layer.ViewCamera.BottomRight.Y - this.Scale.Y / 2f;
        if (this.myAlignment.HasFlag((Enum) Align.Left))
          this.Position.X = this.Layer.ViewCamera.TopLeft.X + this.Scale.X / 2f;
        if (this.myAlignment.HasFlag((Enum) Align.Right))
          this.Position.X = this.Layer.ViewCamera.BottomRight.X - this.Scale.X / 2f;
        this.Position = this.Position + this.Offset;
        if (this.mySprite == null)
          return;
        this.mySprite.Position = this.Position;
      }
    }

    public Element(Layer layer, Vector2 scale, Align alignment, Position position)
      : base(layer)
    {
      this.Construct(scale, alignment, position);
    }

    public Element(GameObject parent, Vector2 scale, Align alignment, Position position)
      : base(parent)
    {
      this.Construct(scale, alignment, position);
    }

    protected virtual void Construct(Vector2 scale, Align alignment, Position position)
    {
      this.Offset = position;
      this.Scale = scale;
      this.Alignment = alignment;
      this.Layer.ViewCamera.OnResize += new EventHandler(this.ViewCamera_OnResize);
    }

    private void ViewCamera_OnResize(object sender, EventArgs e)
    {
      this.Alignment = this.Alignment;
      EventHandler onRealign = this.OnRealign;
      if (onRealign == null)
        return;
      onRealign((object) this, (EventArgs) new AlignEventArgs()
      {
        Position = this.Position
      });
    }

    public override void Update(GameTime gameTime)
    {
      if (this.myAnchor != null)
        this.Position = this.myAnchor.Position + this.Offset;
      base.Update(gameTime);
    }
  }
}
