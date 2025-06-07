
// Type: BrawlerSource.Framework.LevelEditor.GridProperties
// Assembly: ProjectTorrim, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC8AE86A-8582-48C1-AE0D-A25C0B7D84A4
//

using System.Collections.Generic;

#nullable disable
namespace BrawlerSource.Framework.LevelEditor
{
  public class GridProperties : GameObjectProperties
  {
    public int GridScale { get; set; }

    public List<DraggableDeletableProperties> Objects { get; set; }

    public List<List<TileInfo>> Tiles { get; set; }
  }
}
