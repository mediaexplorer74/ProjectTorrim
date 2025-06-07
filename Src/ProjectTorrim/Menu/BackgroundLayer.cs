
// Type: BrawlerSource.Menu.BackgroundLayer
// Assembly: ProjectTorrim, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC8AE86A-8582-48C1-AE0D-A25C0B7D84A4
//

using BrawlerSource.Graphics;

#nullable disable
namespace BrawlerSource.Menu
{
  public class BackgroundLayer : Layer
  {
    private bool myFortIsFallen;

    public BackgroundLayer(Level level, int index, bool fortIsFallen)
      : base(level, index)
    {
      this.DrawStates = GameStates.Pause | GameStates.Live;
      this.myFortIsFallen = fortIsFallen;
    }

    public override void Initilize()
    {
      base.Initilize();
      this.ViewCamera.RelativeScale = 1.5f;
      Sprite sprite = new Sprite((Layer) this);
      sprite.Sequence = new Sequence()
      {
        TexturePath = "Sprites\\Swamp_Ground_Full",
        Width = 960,
        Height = 256
      };
      sprite.Position = new Position(0.0f, 0.0f);
      sprite.Depth = 0.0f;
      sprite.AddToDraw();
      Fortress fortress = new Fortress((Layer) this, new Position(-192f, 0.0f), new Position(100f, 100f), 0.5f);
      fortress.HideHealthBar();
      if (this.myFortIsFallen)
        fortress.Fall();
      fortress.AddToDraw();
    }
  }
}
