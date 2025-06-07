
// Type: BrawlerSource.Settings.SettingsMenu
// Assembly: ProjectTorrim, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC8AE86A-8582-48C1-AE0D-A25C0B7D84A4
//

using BrawlerSource.Audio;
using BrawlerSource.Graphics;
using BrawlerSource.Input;
using BrawlerSource.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

#nullable disable
namespace BrawlerSource.Settings
{
  public class SettingsMenu : DrawableGameObject
  {
    private Dictionary<string, Tuple<Text, BrawlerSource.UI.Slider>> mySliders;

    public SettingsMenu(Layer layer, bool includeQuit)
      : base(layer)
    {
      this.mySliders = new Dictionary<string, Tuple<Text, BrawlerSource.UI.Slider>>();
      new InputEvents((GameObject) this).AddKey((Keys) 27, InputType.Pressed, new KeyFunction(this.Back));
      this.Position = new Position(0.0f);
      Sprite sprite = new Sprite((GameObject) this);
      sprite.Sequence = new Sequence()
      {
        TexturePath = "Menu_Background",
        Width = 256,
        Height = 256
      };
      sprite.Position = this.Position;
      sprite.Depth = 0.0f;
      sprite.Scale = Vector2.Divide(new Vector2(384f, 416f), 256f);
      sprite.Colour = new Color(Color.White, 0.5f);
      Text text = new Text((GameObject) this);
      text.FontPath = "Font";
      text.Colour = new Color(247, 209, 0);
      text.Depth = 1f;
      text.Scale = new Vector2(1f);
      text.Position = new Position(0.0f, -176f);
      text.String = "Settings";
      this.AddVolumeSlider("Master", this.Position + new Position(0.0f, -96f), this.Game.Master);
      this.AddVolumeSlider("Music", this.Position + new Position(0.0f, -32f), this.Game.Music);
      this.AddVolumeSlider("Effects", new Position(0.0f, 32f), this.Game.Effects);
      new Button((GameObject) this, Align.Centre, new Position(-100f, 80f), new Position(80f, 48f), new MouseFunction(this.Game.Graphics.SetFullScreen)).SetText("Fullscreen");
      new Button((GameObject) this, Align.Centre, new Position(0.0f, 80f), new Position(80f, 48f), new MouseFunction(this.Game.Graphics.SetFullScreenWindowed)).SetText("Fullscreen\nWindowed");
      new Button((GameObject) this, Align.Centre, new Position(100f, 80f), new Position(80f, 48f), new MouseFunction(this.Game.Graphics.SetWindowed)).SetText("Windowed");
    }

    public void Exit(object sender, BrawlerEventArgs e)
    {
      this.Back();
      this.Game.LevelManager.SetLevel(Levels.MainMenu);
    }

    public void Back(object sender, BrawlerEventArgs e) => this.Back();

    public void Back() => this.Level.State = GameStates.Live;

    public void AddVolumeSlider(string name, Position position, Channel channel)
    {
      Text text1 = new Text((GameObject) this);
      text1.FontPath = "Font";
      text1.Colour = Color.White;
      text1.Depth = 1f;
      text1.Scale = new Vector2(1f);
      text1.Position = position + new Position(0.0f, -24f);
      Text text2 = text1;
      BrawlerSource.UI.Slider slider = (BrawlerSource.UI.Slider) new BrawlerSource.Audio.Slider(this.Layer, new Position(200f, 8f), position, channel);
      slider.AddToDraw();
      this.mySliders.Add(name, new Tuple<Text, BrawlerSource.UI.Slider>(text2, slider));
    }

    public override void Update(GameTime gameTime)
    {
      foreach (string key in this.mySliders.Keys)
      {
        Tuple<Text, BrawlerSource.UI.Slider> slider = this.mySliders[key];
        slider.Item1.String = string.Format("{0} Volume: {1}", (object) key, (object) (Math.Round((double) slider.Item2.Value, 2) * 100.0));
      }
      base.Update(gameTime);
    }
  }
}
