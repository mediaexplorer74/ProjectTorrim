
// Type: BrawlerSource.Level
// Assembly: ProjectTorrim, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC8AE86A-8582-48C1-AE0D-A25C0B7D84A4
//

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

#nullable disable
namespace BrawlerSource
{
  public class Level
  {
    public Game1 Game;
    private GameStates myState;
    private GameStates myNextState;
    public double totalTime;
    private int? myLayerTotal;
    public List<Layer> Layers;
    private List<Layer> myNewLayers;
    private List<Layer> myRemovedLayers;

    public GameStates State
    {
      get => this.myState;
      set => this.myNextState = value;
    }

    public int LayerTotal
    {
      get
      {
        if (!this.myLayerTotal.HasValue)
          this.myLayerTotal = new int?(this.Layers.Count);
        return this.myLayerTotal.Value;
      }
    }

    public Layer GameLayer { get; protected set; }

    public Layer UILayer { get; protected set; }

    public Level(Game1 game)
    {
      this.Game = game;
      this.totalTime = 0.0;
      this.Layers = new List<Layer>();
      this.myNewLayers = new List<Layer>();
      this.myRemovedLayers = new List<Layer>();
      this.State = GameStates.Live;
    }

    public virtual void Initialize()
    {
      foreach (Layer layer in this.Layers)
        layer.Initilize();
    }

    public virtual void LoadContent()
    {
      foreach (Layer layer in this.Layers)
        layer.LoadContent();
    }

    public virtual void UnloadContent()
    {
      foreach (Layer layer in this.Layers)
        layer.UnloadContent();
    }

    public virtual void Update(GameTime gameTime)
    {
      this.myState = this.myNextState;
      this.myNewLayers = new List<Layer>();
      this.myRemovedLayers = new List<Layer>();
      this.totalTime += gameTime.ElapsedGameTime.TotalSeconds;
      foreach (Layer layer in this.Layers)
        layer.Update(gameTime);
    }

    public void AddLayer(Layer l) => this.myNewLayers.Add(l);

    public void RemoveLayer(Layer l) => this.myRemovedLayers.Add(l);

    public virtual void Draw(SpriteBatch spriteBatch)
    {
      foreach (Layer layer in this.Layers)
        layer.Draw(spriteBatch);
    }

    public virtual void Dispose()
    {
      foreach (Layer layer in this.Layers)
        layer.Dispose();
    }
  }
}
