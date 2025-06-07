
// Type: BrawlerSource.FormatUtils
// Assembly: ProjectTorrim, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC8AE86A-8582-48C1-AE0D-A25C0B7D84A4
//

using System.Numerics;

#nullable disable
namespace BrawlerSource
{
  public static class FormatUtils
  {
    public static string GetNumericFormat(BigInteger i)
    {
      return i.ToString(i < 1000000L ? "#,###" : "0.0#e+00");
    }
  }
}
