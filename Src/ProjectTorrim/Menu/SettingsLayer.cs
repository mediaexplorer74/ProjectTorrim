
// Type: BrawlerSource.Menu.SettingsLayer
// Assembly: ProjectTorrim, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC8AE86A-8582-48C1-AE0D-A25C0B7D84A4
//

using BrawlerSource.Input;
using BrawlerSource.Settings;
using BrawlerSource.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

#nullable disable
namespace BrawlerSource.Menu
{
  public class SettingsLayer : Layer
  {
    public SettingsLayer(Level level, int index)
      : base(level, index)
    {
      this.DrawStates = GameStates.Pause | GameStates.Live;
    }

    public override void Initilize()
    {
      base.Initilize();
      Button button1 = new Button((Layer) this, Align.Top | Align.Right, new Position(), new Position(32f, 32f), new MouseFunction(this.Back));
      button1.SetColour(new Color(Color.White, 0.5f));
      button1.SetSprite("Cog", new Vector2?(new Vector2(32f, 32f)));
      button1.AddToDraw();
      InputEvents inputEvents = new InputEvents((Layer) this);
      inputEvents.AddKey((Keys) 80, InputType.Pressed, new KeyFunction(this.Back));
      inputEvents.AddKey((Keys) 27, InputType.Pressed, new KeyFunction(this.Back));
      new SettingsMenu((Layer) this, false).AddToDraw();
      Button button2 = new Button((Layer) this, Align.Centre, new Position(0.0f, 154f), new Position(256f, 40f), new MouseFunction(this.Back));
      button2.SetText("Back");
      button2.SetTextScale(new Vector2(1f));
      button2.AddToDraw();
    }

    public void Back(object sender, BrawlerEventArgs e) => this.Back();

    public void Back()
    {
      ((LevelMenu) this.Level).MainMenu.IsEnabled = true;
      this.IsEnabled = false;
    }
  }
}
