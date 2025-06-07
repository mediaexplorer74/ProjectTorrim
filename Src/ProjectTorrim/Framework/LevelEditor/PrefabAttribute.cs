
// Type: BrawlerSource.Framework.LevelEditor.PrefabAttribute
// Assembly: ProjectTorrim, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC8AE86A-8582-48C1-AE0D-A25C0B7D84A4
//

using System;

#nullable disable
namespace BrawlerSource.Framework.LevelEditor
{
  public class PrefabAttribute : Attribute
  {
    public TileInfo Info;

    public PrefabAttribute(Type prefab, string spriteName, bool isRepeating = true, bool isResizeable = true)
    {
      this.Info = new TileInfo()
      {
        SpriteName = spriteName,
        ClassName = prefab.FullName,
        IsRepeating = isRepeating,
        IsResizeable = isResizeable
      };
    }
  }
}
