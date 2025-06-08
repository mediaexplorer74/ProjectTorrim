
// Type: BrawlerSource.GameInfo.EndLevel
// Assembly: ProjectTorrim, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC8AE86A-8582-48C1-AE0D-A25C0B7D84A4
//

using BrawlerSource.Menu;

#nullable disable
namespace BrawlerSource.GameInfo
{
  public class EndLevel : Level
  {
    private bool myHasWon;
    private GameProperties myProperties;

    public EndLevel(BrawlerGame game, bool hasWon, GameProperties properties)
      : base(game)
    {
      this.myHasWon = hasWon;
      this.myProperties = properties;
    }

    public override void Initialize()
    {
      this.Layers.Add((Layer) new BackgroundLayer((Level) this, 0, !this.myHasWon));
      this.Layers.Add((Layer) new EndLayer((Level) this, 1, this.myHasWon, this.myProperties));
      base.Initialize();
    }
  }
}
