
// Type: BrawlerSource.LevelManager`1
// Assembly: ProjectTorrim, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC8AE86A-8582-48C1-AE0D-A25C0B7D84A4
//

using System.Collections.Generic;

#nullable disable
namespace BrawlerSource
{
  public class LevelManager<T>
  {
    private Dictionary<T, Level> myLevels;

    public Level Last { get; private set; }

    public Level Current { get; private set; }

    public LevelManager() => this.myLevels = new Dictionary<T, Level>();

    public void RemoveLevel(T id) => this.myLevels.Remove(id);

    public void AddLevel(T id, Level level)
    {
      if (this.myLevels.ContainsKey(id))
        return;
      this.myLevels.Add(id, level);
    }

    public void SetLevel(T id) => this.SetLevel(id, false);

    public void SetLevel(T id, bool disposeOld)
    {
      if (disposeOld)
        this.Last = this.Current;
      this.Current = this.myLevels[id];
    }

    public void DisposeLast()
    {
      this.Last.Dispose();
      this.Last = (Level) null;
    }
  }
}
