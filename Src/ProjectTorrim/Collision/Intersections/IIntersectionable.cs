
// Type: BrawlerSource.Collision.Intersections.IIntersectionable
// Assembly: ProjectTorrim, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC8AE86A-8582-48C1-AE0D-A25C0B7D84A4
//

using Microsoft.Xna.Framework;
using System;

#nullable disable
namespace BrawlerSource.Collision.Intersections
{
  public interface IIntersectionable
  {
    void SetPosition(Position p);

    Position GetPosition();

    bool IsColliding(IIntersectionable i);

    Position GetOffset();

    Position GetClosestPoint(Position p);

    Position GetClosestEdge(Vector2 v);

    Vector2 GetEdgeAtPoint(Position p);

    Tuple<Position, Position> GetAreaPositions();
  }
}
