
// Type: BrawlerSource.GameInfo.GameLevel
// Assembly: ProjectTorrim, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC8AE86A-8582-48C1-AE0D-A25C0B7D84A4
//

#nullable disable
namespace BrawlerSource.GameInfo
{
  public class GameLevel : Level
  {
    public int Score = 250;
    private string myProfile;

    public GameLevel(BrawlerGame game, string profile)
      : base(game)
    {
      this.myProfile = profile;
    }

    public override void Initialize()
    {
      this.GameLayer = (Layer) new BrawlerSource.GameInfo.GameLayer((Level) this, 0, this.myProfile);
      this.Layers.Add(this.GameLayer);
      this.UILayer = (Layer) new BrawlerSource.GameInfo.UILayer((Level) this, 1, (BrawlerSource.GameInfo.GameLayer) this.GameLayer);
      this.Layers.Add(this.UILayer);
      this.Layers.Add((Layer) new OverlayLayer((Level) this, 2));
      base.Initialize();
    }
  }
}
