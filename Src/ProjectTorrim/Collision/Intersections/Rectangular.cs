
// Type: BrawlerSource.Collision.Intersections.Rectangular
// Assembly: ProjectTorrim, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC8AE86A-8582-48C1-AE0D-A25C0B7D84A4
//

using Microsoft.Xna.Framework;
using System;

#nullable disable
namespace BrawlerSource.Collision.Intersections
{
  public class Rectangular : IIntersectionable
  {
    private Position myPosition;

    public Position Position
    {
      get
      {
        return !(this.myPosition != (Position) null) ? this.myPosition : this.myPosition - this.CentreOffset;
      }
      set => this.myPosition = value;
    }

    public float Width
    {
      get => this.Dimensions.X;
      set
      {
        if (this.Dimensions == (Position) null)
          this.Dimensions = new Position();
        this.Dimensions.X = value;
      }
    }

    public float Height
    {
      get => this.Dimensions.Y;
      set
      {
        if (this.Dimensions == (Position) null)
          this.Dimensions = new Position();
        this.Dimensions.Y = value;
      }
    }

    public Position Dimensions { get; set; }

    public Position Centre => this.myPosition;

    public Position CentreOffset => new Position(this.Width / 2f, this.Height / 2f);

    public float Left => this.Position.X;

    public float Right => this.Position.X + this.Width;

    public float Top => this.Position.Y;

    public float Bottom => this.Position.Y + this.Height;

    public Position TopLeft => this.Position;

    public Position TopRight => new Position(this.Right, this.Top);

    public Position BottomLeft => new Position(this.Left, this.Bottom);

    public Position BottomRight => new Position(this.Right, this.Bottom);

    public Vector2 LeftSide => (this.BottomLeft - this.TopLeft).ToVector2();

    public Vector2 RightSide => (this.BottomRight - this.TopRight).ToVector2();

    public Vector2 TopSide => (this.TopRight - this.TopLeft).ToVector2();

    public Vector2 BottomSide => (this.BottomRight - this.BottomLeft).ToVector2();

    public Vector2 LeftNormal => Vector2.Normalize(new Vector2(-this.LeftSide.Y, this.LeftSide.X));

    public Vector2 RightNormal
    {
      get => Vector2.Normalize(new Vector2(this.RightSide.Y, -this.RightSide.X));
    }

    public Vector2 TopNormal => Vector2.Normalize(new Vector2(-this.TopSide.Y, this.TopSide.X));

    public Vector2 BottomNormal
    {
      get => Vector2.Normalize(new Vector2(this.BottomSide.Y, -this.BottomSide.X));
    }

    public void SetPosition(Position p) => this.Position = p;

    public Position GetPosition() => this.Position;

    public bool IsColliding(IIntersectionable i)
    {
      bool flag = false;
      switch (i)
      {
        case Circular circle:
          flag = Intersection.CircleInRectangle(circle, this);
          break;
        case Rectangular rectangle2:
          flag = Intersection.RectangleInRectangle(this, rectangle2);
          break;
        case Point point:
          flag = Intersection.PointInRectangle(point, this);
          break;
      }
      return flag;
    }

    public Position GetOffset() => this.CentreOffset;

    public Position GetClosestPoint(Position p) => p.Clamp(this.TopLeft, this.BottomRight);

    public Vector2 GetEdgeAtPoint(Position p)
    {
      Vector2 vector2_1 = new Vector2();
      if ((double) p.X == (double) this.Left)
        vector2_1 = this.LeftSide;
      else if ((double) p.X == (double) this.Right)
        vector2_1 = this.RightSide;
      Vector2 vector2_2 = new Vector2();
      if ((double) p.Y == (double) this.Top)
        vector2_2 = this.TopSide;
      else if ((double) p.Y == (double) this.Bottom)
        vector2_2 = this.BottomSide;
      return Vector2.Add(vector2_1, vector2_2);
    }

    public Tuple<Position, Position> GetAreaPositions()
    {
      return new Tuple<Position, Position>(this.TopLeft, this.BottomRight);
    }

    public Position GetClosestEdge(Vector2 v)
    {
      return new Position((double) v.X > 0.0 ? this.Left : ((double) v.X < 0.0 ? this.Right : 0.0f), (double) v.Y > 0.0 ? this.Top : ((double) v.Y < 0.0 ? this.Bottom : 0.0f));
    }
  }
}
