
// Type: MathsExtensions
// Assembly: ProjectTorrim, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC8AE86A-8582-48C1-AE0D-A25C0B7D84A4
//

using System.Numerics;

#nullable disable
public static class MathsExtensions
{
  public static float Clamp(this float value, float min, float max)
  {
    if ((double) value < (double) min)
      return min;
    return (double) value <= (double) max ? value : max;
  }

  public static double Clamp(this double value, double min, double max)
  {
    if (value < min)
      return min;
    return value <= max ? value : max;
  }

  public static BigInteger Pow(this BigInteger value, BigInteger exponent)
  {
    BigInteger bigInteger = value;
    while (exponent-- > 1L)
      value *= bigInteger;
    return value;
  }
}
