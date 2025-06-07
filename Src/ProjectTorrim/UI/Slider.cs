
// Type: BrawlerSource.UI.Slider
// Assembly: ProjectTorrim, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC8AE86A-8582-48C1-AE0D-A25C0B7D84A4
//

using BrawlerSource.Graphics;
using Microsoft.Xna.Framework;
using System;

#nullable disable
namespace BrawlerSource.UI
{
  internal class Slider : DrawableGameObject
  {
    public UniqueDraggable Cursor;
    protected Position mySize;
    private Sprite mySprite;

    public event EventHandler OnValueChanged;

    public float Value { get; protected set; }

    public Slider(GameObject parent, Position area, int size, Position position)
      : base(parent)
    {
      this.Construct(area, size, position);
    }

    public Slider(Layer layer, Position area, int size, Position position)
      : base(layer)
    {
      this.Construct(area, size, position);
    }

    protected virtual void Construct(Position area, int size, Position position)
    {
      this.Position = position;
      this.mySize = area;
      UniqueDraggable uniqueDraggable = new UniqueDraggable((GameObject) this, this.Position, new Position((float) size), new Sequence()
      {
        TexturePath = "Circle_32",
        Width = 32,
        Height = 32
      }, 0.5f);
      uniqueDraggable.MinPosition = new Position(this.Position.X - this.mySize.X / 2f, this.Position.Y);
      uniqueDraggable.MaxPosition = new Position(this.Position.X + this.mySize.X / 2f, this.Position.Y);
      this.Cursor = uniqueDraggable;
      this.Cursor.OnMove += new EventHandler(this.Cursor_OnMove);
      Sprite sprite = new Sprite((GameObject) this);
      sprite.Sequence = new Sequence()
      {
        TexturePath = "Square_256",
        Width = 256,
        Height = 256
      };
      sprite.Position = this.Position;
      sprite.Scale = Vector2.Divide(this.mySize.ToVector2(), 256f);
      sprite.Colour = Color.White;
      sprite.Depth = 0.4f;
      this.mySprite = sprite;
    }

    private void Cursor_OnMove(object sender, EventArgs e)
    {
      float num = (this.Cursor.Position.X - this.Cursor.MinPosition.X) / this.mySize.X;
      if ((double) this.Value == (double) num)
        return;
      EventHandler onValueChanged = this.OnValueChanged;
      if (onValueChanged != null)
        onValueChanged((object) this, (EventArgs) new ValueEventArgs()
        {
          Value = num
        });
      this.Value = num;
    }
  }
}
