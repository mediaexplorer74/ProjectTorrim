
// Type: BrawlerSource.Physics.Point
// Assembly: ProjectTorrim, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC8AE86A-8582-48C1-AE0D-A25C0B7D84A4
//

using BrawlerSource.Collision;
using BrawlerSource.Collision.Intersections;
using Microsoft.Xna.Framework;

#nullable disable
namespace BrawlerSource.Physics
{
  public class Point : GameObject
  {
    protected Position myPosition;
    protected Vector2 myVelocity;
    public Collider myCollider;
    public IIntersectionable myIntersection;

    public Position NextPosition { get; protected set; }

    public Position Position
    {
      get => this.myPosition;
      set => this.NextPosition = value;
    }

    public Vector2 NextVelocity { get; protected set; }

    public Vector2 Velocity
    {
      get => this.myVelocity;
      set => this.NextVelocity = value;
    }

    public Point(
      GameObject parent,
      Position position,
      IIntersectionable intersection,
      bool parentIsPoint = false)
      : base(parent)
    {
      this.Construct(position, intersection, parentIsPoint);
    }

    public Point(
      Layer layer,
      Position position,
      IIntersectionable intersection,
      bool parentIsPoint = false)
      : base(layer)
    {
      this.Construct(position, intersection, parentIsPoint);
    }

    public void SetCurrent(Position position, Vector2 velocity)
    {
      this.myPosition = position;
      this.NextPosition = position;
      this.myVelocity = velocity;
      this.NextVelocity = velocity;
      this.myCollider.Position = position;
    }

    protected virtual void Construct(
      Position position,
      IIntersectionable intersection,
      bool parentIsPoint)
    {
      this.myPosition = position;
      this.NextPosition = position;
      this.myVelocity = Vector2.Zero;
      this.NextVelocity = Vector2.Zero;
      this.myIntersection = intersection;
      this.myCollider = new Collider(parentIsPoint ? (GameObject) this : this.Parent)
      {
        Position = this.NextPosition
      };
      this.myCollider.AddIntersection(this.myIntersection);
    }

    public override void Update(GameTime gameTime)
    {
      float totalSeconds = (float) gameTime.ElapsedGameTime.TotalSeconds;
      this.myVelocity = this.NextVelocity;
      this.myPosition = this.NextPosition;
      this.NextPosition += Vector2.Multiply(this.myVelocity, totalSeconds);
      this.myCollider.Position = this.NextPosition;
      base.Update(gameTime);
    }
  }
}
