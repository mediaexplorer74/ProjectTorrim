
// Type: BrawlerSource.Layer
// Assembly: ProjectTorrim, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC8AE86A-8582-48C1-AE0D-A25C0B7D84A4
//

using BrawlerSource.Collision;
using BrawlerSource.Framework.LevelEditor;
using BrawlerSource.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
//using System.Text.Json;
using Newtonsoft.Json;

#nullable disable
namespace BrawlerSource
{
  public class Layer
  {
    public CollisionMap CollisionMap;
    public ViewCamera ViewCamera;
    public CursorCollider Cursor;
    public Level Level;
    public BrawlerGame Game;
    public int Index;
    public TimeSpan TotalTime;
    public bool IsNew;
    public bool IsLoaded;
    private bool myIsEnabled;
    public GameStates UpdateStates = GameStates.Live;
    public GameStates DrawStates = GameStates.Live;
    public GameObject ActiveDragger;

    public GameObjectCollection GameObjects { get; }

    public GameObjectCollection DrawableGameObjects { get; }

    public Random Random { get; private set; }

    public bool IsEnabled
    {
      get => this.myIsEnabled;
      set
      {
        this.myIsEnabled = value;
        if (this.Cursor == null)
          return;
        this.Cursor.myCollider.IsDisabled = !this.myIsEnabled;
      }
    }

    public Layer(Level level, int index)
    {
      this.IsEnabled = true;
      this.IsNew = true;
      this.Index = index;
      this.Level = level;
      this.Game = level.Game;
      this.GameObjects = new GameObjectCollection();
      this.DrawableGameObjects = new GameObjectCollection();
      this.Random = new Random((int) DateTime.Now.Ticks);
    }

    public virtual void Initilize()
    {
      this.TotalTime = TimeSpan.Zero;
      this.CollisionMap = new CollisionMap(this, new Position(0.0f, 0.0f), 
          new Position(9600f, 5400f), 4, 8);
      this.ViewCamera = new ViewCamera(this, new Vector2(960f, 540f));
      this.Cursor = new CursorCollider(this);
    }

    public virtual void LoadContent()
    {
      this.GameObjects.LoadContent();
      this.IsLoaded = true;
    }

    public virtual void UnloadContent()
    {
      this.GameObjects.UnloadContent();
      this.IsLoaded = false;
    }

    public virtual void FirstUpdate(GameTime gameTime)
    {
      this.IsNew = false;
      this.Game.Graphics.ScreenResize();
    }

    public virtual void Update(GameTime gameTime)
    {
      if (!(this.IsEnabled & this.UpdateStates.HasFlag((Enum) this.Level.State)))
        return;
      if (this.IsNew)
        this.FirstUpdate(gameTime);
      this.TotalTime += gameTime.ElapsedGameTime;
      this.GameObjects.Update(gameTime);
    }

    public virtual void Draw(SpriteBatch spriteBatch)
    {
      if (!this.IsEnabled || !this.DrawStates.HasFlag((Enum) this.Level.State))
        return;
      spriteBatch.Begin((SpriteSortMode) 4, (BlendState) null, SamplerState.PointWrap, 
          (DepthStencilState) null, (RasterizerState) null, (Effect) null, new Matrix?());
      this.DrawableGameObjects.Draw(spriteBatch);
      spriteBatch.End();
    }

    public virtual void Dispose() => this.GameObjects.RemoveAll();

    public void Load(string filename)
    {
      MultiGridProperties multiGridProperties = JsonConvert.DeserializeObject<MultiGridProperties>(
          File.ReadAllText("Content/" + filename));
      for (int index1 = 0; index1 < multiGridProperties.Grids.Count; ++index1)
      {
        GridProperties grid = multiGridProperties.Grids[index1];
        float depth = (float) index1 / (float) multiGridProperties.Grids.Count;
        if (grid.Objects != null)
        {
          foreach (DraggableDeletableProperties deletableProperties in grid.Objects)
            this.LoadTile(depth, grid.GridScale, deletableProperties.Info, 
                deletableProperties.Position, deletableProperties.Dimensions);
        }
        for (int index2 = 0; index2 < grid.Tiles.Count; ++index2)
        {
          List<TileInfo> tile = grid.Tiles[index2];
          for (int index3 = 0; index3 < tile.Count; ++index3)
          {
            this.LoadTile(depth, grid.GridScale, tile[index3],
                new Position((float)(index3 * grid.GridScale),
                (float)(index2 * grid.GridScale)) - new Position(960f, 540f)
                + (float)(grid.GridScale / 2));
          }
        }
      }
    }

    private void LoadTile(float depth, int gridScale, TileInfo tile, Position position)
    {
      this.LoadTile(depth, gridScale, tile, position, new Position((float) gridScale));
    }

    private void LoadTile(
      float depth,
      int gridScale,
      TileInfo tile,
      Position position,
      Position dimensions)
    {
      if (tile.SpriteName == null)
        return;
      if (tile.ClassName == null || tile.ClassName == "BrawlerSource.Graphics.Sprite")
      {
        Sprite sprite1 = new Sprite(this);
        sprite1.Sequence = new Sequence()
        {
          TexturePath = tile.SpriteName,
          Width = tile.IsRepeating ? (int) dimensions.X : gridScale,
          Height = tile.IsRepeating ? (int) dimensions.Y : gridScale
        };
        sprite1.Scale = tile.IsRepeating ? Vector2.One : (dimensions / (float) gridScale).ToVector2();
        sprite1.Position = position;
        sprite1.Depth = depth;
        Sprite sprite2 = sprite1;
        if (this.IsLoaded)
          sprite2.LoadContent();
        sprite2.AddToDraw();
      }
      else
      {
        if (!((GameObject) Activator.CreateInstance(Type.GetType(tile.ClassName), (object) this, 
            (object) position, (object) dimensions, (object) depth) is DrawableGameObject instance))
          return;
        if (this.IsLoaded)
          instance.LoadContent();
        instance.AddToDraw();
      }
    }
  }
}
