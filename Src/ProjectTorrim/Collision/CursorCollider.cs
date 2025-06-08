
// Type: BrawlerSource.Collision.CursorCollider
// Assembly: ProjectTorrim, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC8AE86A-8582-48C1-AE0D-A25C0B7D84A4
//

using BrawlerSource.Collision.Intersections;
using BrawlerSource.Input;
using Microsoft.Xna.Framework;

#nullable disable
namespace BrawlerSource.Collision
{
  public class CursorCollider : GameObject
  {
    public Position ScreenPosition;
    public Position Position;
    public Collider myCollider;
    public BrawlerSource.Collision.Intersections.Point myIntersection;

    public CursorCollider(Layer layer)
      : base(layer)
    {
      this.myIntersection = new BrawlerSource.Collision.Intersections.Point();
      this.myCollider = new Collider((GameObject) this)
      {
        Position = new Position()
      };
      this.myCollider.AddIntersection((IIntersectionable) this.myIntersection);
    }

    public override void Update(GameTime gameTime)
    {
      Position pos1 = MouseInput.GetPosition();
      if (pos1.ToVector2() != Vector2.Zero)
        this.UpdatePosition(pos1);
      
      Position pos2 = MobileInput.GetPosition(); 
      if (pos2.ToVector2() != Vector2.Zero)
        this.UpdatePosition(pos2);

      base.Update(gameTime);
    }

    public void UpdatePosition(Position position)
    {
      this.Position = this.Layer.ViewCamera.GetWorldPosition(position);
      this.ScreenPosition = position;
      this.myCollider.Position = this.Position;
    }
  }
}
