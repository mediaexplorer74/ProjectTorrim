
// Type: BrawlerSource.GameInfo.GameProperties
// Assembly: ProjectTorrim, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC8AE86A-8582-48C1-AE0D-A25C0B7D84A4
//

using System.Collections.Generic;

#nullable disable
namespace BrawlerSource.GameInfo
{
  public class GameProperties
  {
    public List<TowerProperties> Towers { get; set; }

    public List<BasesProperties> Bases { get; set; }

    public int Waves { get; set; }

    public int Score { get; set; }
  }
}
