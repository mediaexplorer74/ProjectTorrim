
// Type: BrawlerSource.Physics.PointMass
// Assembly: ProjectTorrim, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC8AE86A-8582-48C1-AE0D-A25C0B7D84A4
//

using BrawlerSource.Collision;
using BrawlerSource.Collision.Intersections;
using Microsoft.Xna.Framework;
using System;

#nullable disable
namespace BrawlerSource.Physics
{
  public class PointMass : Point
  {
    public bool CollideWithOthers;
    public Vector2 Acceleration = Vector2.Zero;
    public Vector2 Gravity = Vector2.Zero;
    public float Mass = 26f;
    private Vector2 Force = Vector2.Zero;

    public float TerminalSpeed => 1000000f;

    private Vector2 Weight => Vector2.Multiply(this.Gravity, new Vector2(this.Mass, this.Mass));

    public PointMass(
      GameObject parent,
      Position position,
      IIntersectionable intersection,
      bool parentIsPoint = false)
      : base(parent, position, intersection, parentIsPoint)
    {
    }

    public PointMass(
      Layer layer,
      Position position,
      IIntersectionable intersection,
      bool parentIsPoint = false)
      : base(layer, position, intersection, parentIsPoint)
    {
    }

    protected override void Construct(
      Position position,
      IIntersectionable intersection,
      bool parentIsPoint)
    {
      base.Construct(position, intersection, parentIsPoint);
      this.myCollider.CollisionLayer = LayerType.Physics;
    }

    public override void FirstUpdate(GameTime gameTime)
    {
      if (this.CollideWithOthers)
        this.myCollider.AddEvent(typeof (PointMass), TouchType.TouchStart, new CollisionFunc(this.PointMass_Collision));
      base.FirstUpdate(gameTime);
    }

    public void PointMass_Collision(object sender, CollisionEventArgs e)
    {
      PointMass trigger = (PointMass) e.Trigger;
      Position position = this.myIntersection.GetClosestEdge(Vector2.Subtract(trigger.Velocity, this.Velocity)) - this.Velocity;
      Position closestEdge = trigger.myIntersection.GetClosestEdge(this.Velocity);
      if (((double) position.X - (double) closestEdge.X) * (double) Math.Sign(this.Velocity.X) < 0.0)
        position.X = closestEdge.X;
      if (((double) position.Y - (double) closestEdge.Y) * (double) Math.Sign(this.Velocity.Y) < 0.0)
        position.Y = closestEdge.Y;
      this.NextVelocity = Vector2.Multiply(this.NextVelocity, (closestEdge - position).Abs().ToVector2().NormalizeToZero());
      this.NextPosition = this.myPosition;
      this.myCollider.RegisterCollision(trigger.myCollider, false);
      trigger.myCollider.RegisterCollision(this.myCollider, false);
    }
  }
}
