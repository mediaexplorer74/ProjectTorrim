
// Type: BrawlerSource.Menu.MenuLayer
// Assembly: ProjectTorrim, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC8AE86A-8582-48C1-AE0D-A25C0B7D84A4
//

using BrawlerSource.Graphics;
using BrawlerSource.Input;
using BrawlerSource.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

#nullable disable
namespace BrawlerSource.Menu
{
  public class MenuLayer : Layer
  {
    public MenuLayer(Level level, int index)
      : base(level, index)
    {
      this.DrawStates = GameStates.Pause | GameStates.Live;
    }

    public override void Initilize()
    {
      base.Initilize();
      Sprite sprite1 = new Sprite((Layer) this);
      sprite1.Sequence = new Sequence()
      {
        TexturePath = "Menu_Background",
        Width = 256,
        Height = 256
      };
      sprite1.Position = new Position(0.0f, 0.0f);
      sprite1.Depth = 0.0f;
      sprite1.Scale = Vector2.Divide(new Vector2(384f, 416f), 256f);
      sprite1.Colour = new Color(Color.White, 0.5f);
      sprite1.AddToDraw();
      Sprite sprite2 = new Sprite((Layer) this);
      sprite2.Sequence = new Sequence()
      {
        TexturePath = "Splash Title",
        Width = 374,
        Height = 232
      };
      sprite2.Position = new Position(0.0f, -144f);
      sprite2.Scale = new Vector2(0.5f);
      sprite2.AddToDraw();
      Button button1 = new Button((Layer) this, Align.Centre, new Position(0.0f, -50f), new Position(256f, 40f), new MouseFunction(this.New));
      button1.SetText("New");
      button1.SetTextScale(new Vector2(1f));
      button1.AddToDraw();
      Button button2 = new Button((Layer) this, Align.Centre, new Position(0.0f, 0.0f), new Position(256f, 40f), new MouseFunction(this.Load));
      button2.SetText("Load");
      button2.SetTextScale(new Vector2(1f));
      button2.AddToDraw();
      Button button3 = new Button((Layer) this, Align.Centre, new Position(0.0f, 50f), new Position(256f, 40f), new MouseFunction(this.ShowInstructions));
      button3.SetText("Instructions");
      button3.SetTextScale(new Vector2(1f));
      button3.AddToDraw();
      Button button4 = new Button((Layer) this, Align.Centre, new Position(0.0f, 100f), new Position(256f, 40f), new MouseFunction(this.ShowSettings));
      button4.SetText("Settings");
      button4.SetTextScale(new Vector2(1f));
      button4.AddToDraw();
      new InputEvents((Layer) this).AddKey((Keys) 27, InputType.Pressed, new KeyFunction(this.Exit));
      Button button5 = new Button((Layer) this, Align.Centre, new Position(0.0f, 150f), new Position(256f, 40f), new MouseFunction(this.Exit));
      button5.SetText("Exit");
      button5.SetTextScale(new Vector2(1f));
      button5.AddToDraw();
      Sprite sprite3 = new Sprite((Layer) this);
      sprite3.Sequence = new Sequence()
      {
        TexturePath = "Menu_Background",
        Width = 256,
        Height = 256
      };
      sprite3.Position = new Position(288f, 150f);
      sprite3.Depth = 0.0f;
      sprite3.Scale = Vector2.Divide(new Vector2(176f, 80f), 256f);
      sprite3.Colour = new Color(Color.White, 0.5f);
      sprite3.AddToDraw();
      Sprite sprite4 = new Sprite((Layer) this);
      sprite4.Sequence = new Sequence()
      {
        TexturePath = "Skeffles",
        Width = 128,
        Height = 128
      };
      sprite4.Position = new Position(336f, 150f);
      sprite4.Scale = new Vector2(0.5f);
      sprite4.Depth = 1f;
      sprite4.AddToDraw();
      Text text = new Text((Layer) this);
      text.String = "A Game By\nSkeffles";
      text.FontPath = "Font";
      text.Colour = Color.White;
      text.Depth = 1f;
      text.Scale = new Vector2(0.5f);
      text.Position = new Position(256f, 150f);
      text.AddToDraw();
    }

    public void New(object sender, BrawlerEventArgs e) => this.ShowProfileMenu(true);

    public void Load(object sender, BrawlerEventArgs e) => this.ShowProfileMenu(false);

    private void ShowProfileMenu(bool warnOverride)
    {
      ((LevelMenu) this.Level).ProfilePicker.IsEnabled = true;
      ((LevelMenu) this.Level).ProfilePicker.IsNewMode = warnOverride;
      this.IsEnabled = false;
    }

    public void ShowInstructions(object sender, BrawlerEventArgs e)
    {
      ((LevelMenu) this.Level).Instructions.IsEnabled = true;
      this.IsEnabled = false;
    }

    public void ShowSettings(object sender, BrawlerEventArgs e)
    {
      ((LevelMenu) this.Level).Settings.IsEnabled = true;
      this.IsEnabled = false;
    }

    public void Exit(object sender, BrawlerEventArgs e) => this.Game.Exit();
  }
}
