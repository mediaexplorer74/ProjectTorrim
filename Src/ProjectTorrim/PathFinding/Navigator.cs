
// Type: BrawlerSource.PathFinding.Navigator
// Assembly: ProjectTorrim, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC8AE86A-8582-48C1-AE0D-A25C0B7D84A4
//

using BrawlerSource.Collision;
using BrawlerSource.Collision.Intersections;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

#nullable disable
namespace BrawlerSource.PathFinding
{
  public class Navigator : BrawlerSource.Physics.Point
  {
    public Queue<Node> Path;
    public Node Target;
    public float Speed;

    public Navigator(Layer layer, Position position, IIntersectionable intersection)
      : base(layer, position, intersection)
    {
      this.Construct(position, intersection);
    }

    public Navigator(GameObject parent, Position position, IIntersectionable intersection)
      : base(parent, position, intersection)
    {
      this.Construct(position, intersection);
    }

    protected virtual void Construct(Position position, IIntersectionable intersection)
    {
      this.Speed = 0.0f;
      this.Target = (Node) null;
      this.Path = new Queue<Node>();
      this.myCollider.CollisionLayer = LayerType.Navigation;
    }

    public override void FirstUpdate(GameTime gameTime)
    {
      this.myCollider.AddEvent(typeof (Node), TouchType.TouchStart, new CollisionFunc(this.TargetTouch));
      base.FirstUpdate(gameTime);
    }

    public override void Update(GameTime gameTime)
    {
      if (this.Target == null && this.Path.Count > 0)
        this.SetTarget(this.Path.Dequeue());
      if (this.Target != null)
        this.NextVelocity = Vector2.Multiply((this.Target.Position - this.myPosition).ToVector2().NormalizeToZero(), this.Speed);
      base.Update(gameTime);
    }

    public void SetTarget(Node target)
    {
      this.Target = target;
      if (this.Target != null)
        return;
      this.NextVelocity = new Vector2(0.0f);
    }

    public void TargetTouch(object sender, CollisionEventArgs e)
    {
      if ((Node) e.Trigger != this.Target)
        return;
      this.NextVelocity = new Vector2(0.0f);
      this.Target = (Node) null;
    }
  }
}
