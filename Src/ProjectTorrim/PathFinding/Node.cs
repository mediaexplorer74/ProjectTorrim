
// Type: BrawlerSource.PathFinding.Node
// Assembly: ProjectTorrim, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC8AE86A-8582-48C1-AE0D-A25C0B7D84A4
//

using BrawlerSource.Collision;
using BrawlerSource.Collision.Intersections;
using BrawlerSource.Framework.LevelEditor;

#nullable disable
namespace BrawlerSource.PathFinding
{
  [Prefab(typeof (Node), "Cross", true, true)]
  public class Node : GameObject
  {
    public Position Position;
    public Collider Collider;

    public Node(Layer layer, Position position, Position dimensions, float depth)
      : base(layer)
    {
      this.Position = position;
      Point i = new Point(this.Position);
      this.Collider = new Collider((GameObject) this)
      {
        Position = this.Position,
        CollisionLayer = LayerType.Navigation
      };
      this.Collider.AddIntersection((IIntersectionable) i);
    }
  }
}
