
// Type: BrawlerSource.Position
// Assembly: ProjectTorrim, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC8AE86A-8582-48C1-AE0D-A25C0B7D84A4
//

using Microsoft.Xna.Framework;
using System;

#nullable disable
namespace BrawlerSource
{
  public class Position
  {
    public float X { get; set; }

    public float Y { get; set; }

    public Position()
    {
      this.X = 0.0f;
      this.Y = 0.0f;
    }

    public Position(float x)
    {
      this.X = x;
      this.Y = x;
    }

    public Position(float x, float y)
    {
      this.X = x;
      this.Y = y;
    }

    public Position(Vector2 v)
    {
      this.X = v.X;
      this.Y = v.Y;
    }

    public Position(Position p)
    {
      this.X = p.X;
      this.Y = p.Y;
    }

    public Position(Point p)
    {
      this.X = (float) p.X;
      this.Y = (float) p.Y;
    }

    public Vector2 GetVector2() => new Vector2(this.X, this.Y);

    public override bool Equals(object obj) => obj as Position == this;

    public override int GetHashCode() => new Tuple<float, float>(this.X, this.Y).GetHashCode();

    public Position Clamp(Position min, Position max)
    {
      return new Position(this.X.Clamp(min.X, max.X), this.Y.Clamp(min.Y, max.Y));
    }

    public Position Abs() => new Position(Math.Abs(this.X), Math.Abs(this.Y));

    public Position Round(int digits) => this.Round(digits, 1.0);

    public Position Round(int digits, double nearest)
    {
      return new Position((float) Math.Round((double) this.X / nearest, digits) * (float) nearest, (float) Math.Round((double) this.Y / nearest, digits) * (float) nearest);
    }

    public Position Floor() => this.Floor(1.0);

    public Position Floor(double nearest)
    {
      return new Position((float) Math.Floor((double) this.X / nearest) * (float) nearest, (float) Math.Floor((double) this.Y / nearest) * (float) nearest);
    }

    public static bool operator ==(Position x, Position y)
    {
      if ((object) x == null && (object) y == null)
        return true;
      return (object) x != null && (object) y != null && (double) x.X == (double) y.X && (double) x.Y == (double) y.Y;
    }

    public static bool operator !=(Position x, Position y) => !(x == y);

    public static Position operator +(Position a) => a;

    public static Position operator +(Position a, Position b) => new Position(a.X + b.X, a.Y + b.Y);

    public static Position operator +(Position a, float b) => new Position(a.X + b, a.Y + b);

    public static Position operator -(Position a) => new Position(-a.X, -a.Y);

    public static Position operator -(Position a, Position b) => new Position(a.X - b.X, a.Y - b.Y);

    public static Position operator -(Position a, float b) => new Position(a.X - b, a.Y - b);

    public static Position operator /(Position a, Position b) => new Position(a.X / b.X, a.Y / b.Y);

    public static Position operator /(float a, Position b) => new Position(a / b.X, a / b.Y);

    public static Position operator /(double a, Position b)
    {
      return new Position((float) a / b.X, (float) a / b.Y);
    }

    public static Position operator /(Position a, float b) => new Position(a.X / b, a.Y / b);

    public static Position operator /(Position a, double b)
    {
      return new Position(a.X / (float) b, a.Y / (float) b);
    }

    public static Position operator %(Position a, Position b) => new Position(a.X % b.X, a.Y % b.Y);

    public static Position operator %(float a, Position b) => new Position(a % b.X, a % b.Y);

    public static Position operator %(double a, Position b)
    {
      return new Position((float) a % b.X, (float) a % b.Y);
    }

    public static Position operator %(Position a, float b) => new Position(a.X % b, a.Y % b);

    public static Position operator %(Position a, double b)
    {
      return new Position(a.X % (float) b, a.Y % (float) b);
    }

    public static Position operator *(Position a, Position b) => new Position(a.X * b.X, a.Y * b.Y);

    public static Position operator *(Position a, float b) => new Position(a.X * b, a.Y * b);

    public double DistanceToSquared(Position pos)
    {
      double num1 = (double) this.X - (double) pos.X;
      float num2 = this.Y - pos.Y;
      return num1 * num1 + (double) num2 * (double) num2;
    }

    public double DistanceTo(Position pos) => Math.Sqrt(this.DistanceToSquared(pos));

    public static Position operator +(Position a, Vector2 b) => new Position(a.X + b.X, a.Y + b.Y);

    public static Position operator -(Position a, Vector2 b) => new Position(a.X - b.X, a.Y - b.Y);

    public static Position Intersects(Position aPos, Vector2 aDir, Position bPos, Vector2 bDir)
    {
      float num1 = (float) ((double) aDir.Y * (double) aPos.X + (double) aDir.X * (double) aPos.Y);
      float num2 = (float) ((double) bDir.Y * (double) bPos.X + (double) bDir.X * (double) bPos.Y);
      Position position = (Position) null;
      float num3 = (float) ((double) aDir.Y * (double) bDir.X - (double) bDir.Y * (double) aDir.X);
      if ((double) num3 != 0.0)
        position = new Position((float) ((double) bDir.X * (double) num1 - (double) aDir.X * (double) num2) / num3, (float) ((double) aDir.Y * (double) num2 - (double) bDir.Y * (double) num1) / num3);
      return position;
    }

    public Vector2 VectorTo(Position a) => new Vector2(this.X - a.X, this.Y - a.Y);

    public Vector2 ToVector2() => new Vector2(this.X, this.Y);

    public override string ToString()
    {
      return string.Format("Position(x: {0}, y: {1})", (object) this.X, (object) this.Y);
    }
  }
}
