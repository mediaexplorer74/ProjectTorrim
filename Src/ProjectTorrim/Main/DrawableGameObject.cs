
// Type: BrawlerSource.DrawableGameObject
// Assembly: ProjectTorrim, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC8AE86A-8582-48C1-AE0D-A25C0B7D84A4
//

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#nullable disable
namespace BrawlerSource
{
  public class DrawableGameObject : ContentGameObject
  {
    public Color Colour = Color.White;
    public float Rotation;
    public Vector2 Origin = Vector2.Zero;
    public Vector2 Scale = Vector2.One;
    public SpriteEffects Effect;
    public float Depth;
    public bool IsHidden;

    public Position Position { get; set; }

    protected Vector2 myPosition
    {
      get => this.Layer.ViewCamera.GetScreenPosition(this.Position).ToVector2();
    }

    protected Vector2 myScale => Vector2.Multiply(this.Scale, this.Layer.ViewCamera.Scale);

    public DrawableGameObject()
    {
    }

    public DrawableGameObject(Layer layer)
      : base(layer)
    {
    }

    public DrawableGameObject(GameObject parent)
      : base(parent)
    {
    }

    public virtual void Draw(SpriteBatch spriteBatch)
    {
      if (this.IsHidden)
        return;
      this.SubGameObjects.Draw(spriteBatch);
    }

    public override void Dispose()
    {
      this.RemoveFromDraw();
      base.Dispose();
    }

    public virtual void AddToDraw() => this.Layer.DrawableGameObjects.Add((GameObject) this);

    public virtual void RemoveFromDraw()
    {
      this.Layer.DrawableGameObjects.Remove((GameObject) this);
    }
  }
}
