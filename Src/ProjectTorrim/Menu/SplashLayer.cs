
// Type: BrawlerSource.Menu.SplashLayer
// Assembly: ProjectTorrim, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC8AE86A-8582-48C1-AE0D-A25C0B7D84A4
//

using BrawlerSource.Audio;
using BrawlerSource.Graphics;
using BrawlerSource.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

#nullable disable
namespace BrawlerSource.Menu
{
  public class SplashLayer : Layer
  {
    private MediaSong myMusic;
    private TimeSpan mySplashTime;

    public SplashLayer(Level level, int index, TimeSpan splashTime)
      : base(level, index)
    {
      this.mySplashTime = splashTime;
    }

    public override void Initilize()
    {
      base.Initilize();
      InputEvents inputEvents = new InputEvents((Layer) this);
      inputEvents.AddKey((Keys) 27, InputType.Released, new KeyFunction(this.Hide));
      inputEvents.AddKey((Keys) 13, InputType.Released, new KeyFunction(this.Hide));
      inputEvents.AddKey((Keys) 32, InputType.Released, new KeyFunction(this.Hide));
      inputEvents.AddMouseButton(MouseButtons.Left, InputType.Released, new MouseFunction(this.Hide));
      Sprite sprite1 = new Sprite((Layer) this);
      sprite1.Sequence = new Sequence()
      {
        TexturePath = "Splash Title",
        Width = 374,
        Height = 232
      };
      sprite1.Position = new Position(0.0f, -96f);
      sprite1.AddToDraw();
      Sprite sprite2 = new Sprite((Layer) this);
      sprite2.Sequence = new Sequence()
      {
        TexturePath = "Menu_Background",
        Width = 256,
        Height = 256
      };
      sprite2.Position = new Position(0.0f, 112f);
      sprite2.Depth = 0.0f;
      sprite2.Scale = Vector2.Divide(new Vector2(352f, 160f), 256f);
      sprite2.Colour = new Color(Color.White, 0.5f);
      sprite2.AddToDraw();
      Sprite sprite3 = new Sprite((Layer) this);
      sprite3.Sequence = new Sequence()
      {
        TexturePath = "Skeffles",
        Width = 128,
        Height = 128
      };
      sprite3.Position = new Position(96f, 112f);
      sprite3.Depth = 1f;
      sprite3.AddToDraw();
      Text text = new Text((Layer) this);
      text.String = "A Game By\nSkeffles";
      text.FontPath = "Font";
      text.Colour = Color.White;
      text.Depth = 1f;
      text.Scale = new Vector2(1f);
      text.Position = new Position(-64f, 112f);
      text.AddToDraw();
      MediaSong mediaSong = new MediaSong((Layer) this, this.Game.Music);
      mediaSong.AudioPath = "Sound\\Deep Towers";
      mediaSong.Volume = 0.1f;
      this.myMusic = mediaSong;
    }

    public override void FirstUpdate(GameTime gameTime)
    {
      base.FirstUpdate(gameTime);
      this.myMusic.Play();
    }

    public void Hide(object sender, BrawlerEventArgs e) => this.Hide();

    public void Hide()
    {
      ((LevelMenu) this.Level).MainMenu.IsEnabled = true;
      this.IsEnabled = false;
    }

    public override void Update(GameTime gameTime)
    {
      if (this.IsEnabled && gameTime.TotalGameTime > this.mySplashTime)
        this.Hide();
      base.Update(gameTime);
    }
  }
}
