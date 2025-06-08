
// Type: BrawlerSource.Collision.Intersections.Intersection
// Assembly: ProjectTorrim, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC8AE86A-8582-48C1-AE0D-A25C0B7D84A4
//

using System;

#nullable disable
namespace BrawlerSource.Collision.Intersections
{
  public static class Intersection
  {
    public static bool PointInPoint(Point point1, Point point2) => point1 == point2;

    public static bool PointInCircle(Point point, Circular circle)
    {
      return point.Position.DistanceToSquared(circle.Position) < Math.Pow((double) circle.Radius, 2.0);
    }

    public static bool PointInRectangle(Point point, Rectangular rectangle)
    {
      return (double) point.Position.X > (double) rectangle.Left 
                && (double) point.Position.X < (double) rectangle.Right 
                && (double) point.Position.Y > (double) rectangle.Top && (double) point.Position.Y < (double) rectangle.Bottom;
    }

    public static bool CircleInCircle(Circular circle1, Circular circle2)
    {
      return circle1.Position.DistanceToSquared(circle2.Position) 
                < Math.Pow((double) circle1.Radius + (double) circle2.Radius, 2.0);
    }

    public static bool RectangleInRectangle(Rectangular rectangle1, Rectangular rectangle2)
    {
      return (double) rectangle1.Right > (double) rectangle2.Left 
                && (double) rectangle1.Left < (double) rectangle2.Right && (double) rectangle1.Bottom > (double) rectangle2.Top && (double) rectangle1.Top < (double) rectangle2.Bottom;
    }

    public static bool CircleInRectangle(Circular circle, Rectangular rectangle)
    {
      return circle.Position.Clamp(rectangle.TopLeft, rectangle.BottomRight).DistanceToSquared(circle.Position) < Math.Pow((double) circle.Radius, 2.0);
    }
  }
}
