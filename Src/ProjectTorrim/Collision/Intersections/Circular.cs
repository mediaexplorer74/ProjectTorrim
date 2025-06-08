
// Type: BrawlerSource.Collision.Intersections.Circular
// Assembly: ProjectTorrim, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC8AE86A-8582-48C1-AE0D-A25C0B7D84A4
//

using Microsoft.Xna.Framework;
using System;

#nullable disable
namespace BrawlerSource.Collision.Intersections
{
  public class Circular : IIntersectionable
  {
    public Position Position;
    public float Radius;

    public void SetPosition(Position p) => this.Position = p;

    public Position GetPosition() => this.Position;

    public bool IsColliding(IIntersectionable i)
    {
      bool flag = false;
      switch (i)
      {
        case Circular circle2:
          flag = Intersection.CircleInCircle(this, circle2);
          break;
        case Rectangular rectangle:
          flag = Intersection.CircleInRectangle(this, rectangle);
          break;
        case Point point:
          flag = Intersection.PointInCircle(point, this);
          break;
      }
      return flag;
    }

    public Position GetOffset() => throw new NotImplementedException();

    public Position GetClosestPoint(Position p) => throw new NotImplementedException();

    public Vector2 GetEdgeAtPoint(Position p) => throw new NotImplementedException();

    public Tuple<Position, Position> GetAreaPositions()
    {
      return new Tuple<Position, Position>(this.Position - this.Radius, this.Position + this.Radius);
    }

    public Position GetClosestEdge(Vector2 v) => throw new NotImplementedException();
  }
}
