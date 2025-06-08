
// Type: BrawlerSource.GameInfo.OverlayLayer
// Assembly: ProjectTorrim, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC8AE86A-8582-48C1-AE0D-A25C0B7D84A4
//

using BrawlerSource.Input;
using BrawlerSource.Settings;
using BrawlerSource.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

#nullable disable
namespace BrawlerSource.GameInfo
{
  public class OverlayLayer : Layer
  {
    public OverlayLayer(Level level, int index)
      : base(level, index)
    {
      this.UpdateStates = GameStates.Pause;
      this.DrawStates = GameStates.Pause;
    }

    public override void Initilize()
    {
      base.Initilize();
      Button button1 = new Button((Layer) this, Align.Top | Align.Right, new Position(), 
          new Position(32f, 32f), new MouseFunction(this.Back), new TouchFunction(this.Back));
      button1.SetColour(new Color(Color.White, 0.5f));
      button1.SetSprite("Cog", new Vector2?(new Vector2(32f, 32f)));
      button1.AddToDraw();
      new InputEvents((Layer) this).AddKey((Keys) 80, InputType.Pressed, new KeyFunction(this.Back));
      new SettingsMenu((Layer) this, true).AddToDraw();
      Button button2 = new Button((Layer) this, Align.Centre, new Position(-64f, 154f), 
          new Position(128f, 40f), new MouseFunction(this.Exit), new TouchFunction(this.Exit));
      button2.SetText("Quit");
      button2.SetTextScale(new Vector2(1f));
      button2.AddToDraw();
      Button button3 = new Button((Layer) this, Align.Centre, new Position(64f, 154f), 
          new Position(128f, 40f), new MouseFunction(this.Back), new TouchFunction(this.Back));
      button3.SetText("Back");
      button3.SetTextScale(new Vector2(1f));
      button3.AddToDraw();
    }

    public void Exit(object sender, BrawlerEventArgs e)
    {
      this.Back();
      this.Game.LevelManager.SetLevel(Levels.MainMenu, true);
    }

    public void Back(object sender, BrawlerEventArgs e) => this.Back();

    public void Back() => this.Level.State = GameStates.Live;
  }
}
