
// Type: BrawlerSource.Graphics.GraphicsManager
// Assembly: ProjectTorrim, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC8AE86A-8582-48C1-AE0D-A25C0B7D84A4
//

using Microsoft.Xna.Framework;
using System;

#nullable disable
namespace BrawlerSource.Graphics
{
  public class GraphicsManager : GraphicsDeviceManager
  {
    public Game1 Game;

    public event EventHandler OnScreenResize;

    public GraphicsManager(Game1 game)
      : base((Microsoft.Xna.Framework.Game) game)
    {
      this.Game = game;
      this.Game.Window.ClientSizeChanged += new EventHandler<EventArgs>(this.ScreenResize);
    }

    public void ScreenResize(object sender, EventArgs e) => this.ScreenResize();

    public void ScreenResize()
    {
      EventHandler onScreenResize = this.OnScreenResize;
      if (onScreenResize == null)
        return;
      onScreenResize((object) this, (EventArgs) new ScreenResizeEventArgs()
      {
        Size = new Vector2((float) this.PreferredBackBufferWidth, (float) this.PreferredBackBufferHeight)
      });
    }

    public void SetFullScreen(object sender, EventArgs e) => this.SetFullScreen();

    public void SetFullScreen()
    {
      this.PreferredBackBufferWidth = this.GraphicsDevice.DisplayMode.Width;
      this.PreferredBackBufferHeight = this.GraphicsDevice.DisplayMode.Height;
      this.IsFullScreen = true;
      this.ApplyChanges();
      this.Game.Window.AllowUserResizing = false;
      this.Game.Window.IsBorderless = false;
      this.ScreenResize();
    }

    public void SetFullScreenWindowed(object sender, EventArgs e) => this.SetFullScreenWindowed();

    public void SetFullScreenWindowed()
    {
      this.PreferredBackBufferWidth = this.GraphicsDevice.DisplayMode.Width;
      this.PreferredBackBufferHeight = this.GraphicsDevice.DisplayMode.Height;
      this.IsFullScreen = false;
      this.ApplyChanges();
      this.Game.Window.AllowUserResizing = false;
      this.Game.Window.IsBorderless = true;
      //this.Game.Window.Position = new Point(0, 0);
      this.ScreenResize();
    }

    public void SetWindowed(object sender, EventArgs e) => this.SetWindowed();

    public void SetWindowed()
    {
      this.SetWindowed(this.GraphicsDevice.DisplayMode.Width, this.GraphicsDevice.DisplayMode.Height);
    }

    public void SetWindowed(int width, int height)
    {
      this.PreferredBackBufferWidth = width;
      this.PreferredBackBufferHeight = height;
      this.IsFullScreen = false;
      this.ApplyChanges();
      this.Game.Window.AllowUserResizing = true;
      this.Game.Window.IsBorderless = false;
      this.ScreenResize();
    }
  }
}
