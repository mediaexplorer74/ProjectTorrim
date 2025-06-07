
// Type: Vector2Extensions
// Assembly: ProjectTorrim, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC8AE86A-8582-48C1-AE0D-A25C0B7D84A4
//

using Microsoft.Xna.Framework;

#nullable disable
public static class Vector2Extensions
{
  public static Vector2 Average(this Vector2 value, Vector2 value2)
  {
    return Vector2.Divide(Vector2.Add(value, value2), 2f);
  }

  public static Vector2 Pow(this Vector2 x, int y)
  {
    Vector2 vector2 = x;
    for (int index = 0; index < y; ++index)
      vector2 = Vector2.Multiply(vector2, vector2);
    return vector2;
  }

  public static bool IsNaN(this Vector2 value) => float.IsNaN(value.X) && float.IsNaN(value.Y);

  public static Vector2 ClampToAbsoluteMaximum(this Vector2 value, Vector2 maximum)
  {
    return Vector2.Clamp(value, Vector2.Min(Vector2.Zero, maximum), Vector2.Max(Vector2.Zero, maximum));
  }

  public static Vector2 NormalizeToZero(this Vector2 value)
  {
    Vector2 zero = value;
    zero.Normalize();
    if (float.IsNaN(zero.X))
      zero.X = 0.0f;
    if (float.IsNaN(zero.Y))
      zero.Y = 0.0f;
    return zero;
  }
}
