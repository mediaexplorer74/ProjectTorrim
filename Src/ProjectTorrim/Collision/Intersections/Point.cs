
// Type: BrawlerSource.Collision.Intersections.Point
// Assembly: ProjectTorrim, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC8AE86A-8582-48C1-AE0D-A25C0B7D84A4
//

using Microsoft.Xna.Framework;
using System;

#nullable disable
namespace BrawlerSource.Collision.Intersections
{
  public class Point : IIntersectionable
  {
    public Position Position { get; set; }

    public Point()
    {
    }

    public Point(Position position) => this.Position = position;

    public void SetPosition(Position p) => this.Position = p;

    public Position GetPosition() => this.Position;

    public bool IsColliding(IIntersectionable i)
    {
      bool flag = false;
      switch (i)
      {
        case Circular circle:
          flag = Intersection.PointInCircle(this, circle);
          break;
        case Rectangular rectangle:
          flag = Intersection.PointInRectangle(this, rectangle);
          break;
        case Point point2:
          flag = Intersection.PointInPoint(this, point2);
          break;
      }
      return flag;
    }

    public Position GetOffset() => throw new NotImplementedException();

    public Position GetClosestPoint(Position p) => throw new NotImplementedException();

    public Vector2 GetEdgeAtPoint(Position p) => throw new NotImplementedException();

    public Tuple<Position, Position> GetAreaPositions()
    {
      return new Tuple<Position, Position>(this.Position, this.Position);
    }

    public Position GetClosestEdge(Vector2 v) => throw new NotImplementedException();
  }
}
