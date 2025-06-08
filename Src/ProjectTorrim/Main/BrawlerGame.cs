
// Type: BrawlerSource.BrawlerGame
// Assembly: ProjectTorrim, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC8AE86A-8582-48C1-AE0D-A25C0B7D84A4
//

using BrawlerSource.Audio;
using BrawlerSource.Graphics;
using BrawlerSource.Input;
using BrawlerSource.Menu;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#nullable disable
namespace BrawlerSource
{
  public class BrawlerGame : Game
  {
    public GraphicsManager Graphics;
    public SpriteFont Font;
    public Channel Master;
    public Channel Music;
    public Channel Effects;
    public BrawlerSource.LevelManager<Levels> LevelManager;
    public string arg;

    public SpriteBatch spriteBatch { get; set; }

    public BrawlerGame(/*string[] args = null*/)
    {
      //if (args != null && args.Length != 0)
      //  this.arg = args[0];
      this.Graphics = new GraphicsManager(this);
      this.Content.RootDirectory = "Content";
    }

    protected override void Initialize()
    {
      this.Master = new Channel();
      this.Music = new Channel();
      this.Effects = new Channel();
      this.LevelManager = new BrawlerSource.LevelManager<Levels>();
      this.LevelManager.AddLevel(Levels.MainMenu, (Level) new LevelMenu(this));
      this.LevelManager.SetLevel(Levels.MainMenu);
      this.Graphics.SetWindowed(this.GraphicsDevice.DisplayMode.Width / 2, this.GraphicsDevice.DisplayMode.Height / 2);
      this.LevelManager.Current.Initialize();
      this.IsMouseVisible = true;
      base.Initialize();
    }

    protected override void LoadContent()
    {
      this.spriteBatch = new SpriteBatch(this.GraphicsDevice);
      this.Font = this.Content.Load<SpriteFont>("Font");
      this.LevelManager.Current.LoadContent();
    }

    protected override void UnloadContent()
    {
        this.LevelManager.Current.UnloadContent();
    }

    protected override void Update(GameTime gameTime)
    {
      KeyboardInput.Update();
      MouseInput.Update();
      MobileInput.Update();
      this.LevelManager.Current.Update(gameTime);
      if (this.LevelManager.Last != null)
        this.LevelManager.DisposeLast();
      base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
      this.GraphicsDevice.Clear(new Color(61, 64, 35));
      this.LevelManager.Current.Draw(this.spriteBatch);
    }

    public void Reset()
    {
      base.Initialize();
      base.LoadContent();
    }
  }
}
