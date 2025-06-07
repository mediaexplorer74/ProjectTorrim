
// Type: BrawlerSource.Framework.LevelEditor.Grid
// Assembly: ProjectTorrim, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC8AE86A-8582-48C1-AE0D-A25C0B7D84A4
//

using BrawlerSource.Collision;
using BrawlerSource.Collision.Intersections;
using BrawlerSource.Graphics;
using BrawlerSource.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

#nullable disable
namespace BrawlerSource.Framework.LevelEditor
{
  public class Grid : DrawableGameObject
  {
    public List<DraggableDeletable> ObjectList;
    private Tile[,] myTiles;
    private Sequence myGridSequence;
    private TileInfo myDefaultInfo;
    private ClickableCollider myCollider;
    public int GridScale;
    private Position mySize;
    private int myIndex;
    private int myMaxIndex;
    private bool myIsTileMode;

    private float myDepthIndex => (float) this.myIndex / (float) this.myMaxIndex;

    public Grid(Layer layer, int index, int maxIndex, Position size, int scale, bool isTileMode)
      : base(layer)
    {
      this.Construct(index, maxIndex, size, scale, isTileMode);
    }

    public Grid(
      GameObject parent,
      int index,
      int maxIndex,
      Position size,
      int scale,
      bool isTileMode)
      : base(parent)
    {
      this.Construct(index, maxIndex, size, scale, isTileMode);
    }

    protected virtual void Construct(
      int index,
      int maxIndex,
      Position size,
      int scale,
      bool isTileMode)
    {
      this.myIsTileMode = isTileMode;
      this.myIndex = index;
      this.myMaxIndex = maxIndex;
      this.GridScale = scale;
      this.mySize = size.Round(0);
      this.ObjectList = new List<DraggableDeletable>();
      this.myCollider = new ClickableCollider((GameObject) this);
      this.myCollider.AddIntersection((IIntersectionable) new Rectangular()
      {
        Width = (float) ((int) this.mySize.X * this.GridScale),
        Height = (float) ((int) this.mySize.Y * this.GridScale)
      });
      this.myCollider.AddMouseInput(MouseButtons.Left, InputType.Pressed, new MouseFunction(this.PlaceOnGrid));
      this.myCollider.AddMouseInput(MouseButtons.Right, InputType.Pressed, new MouseFunction(this.RemoveSprite));
      this.myGridSequence = new Sequence()
      {
        TexturePath = "Square_32",
        Width = 32,
        Height = 32
      };
      this.New();
    }

    public void SetIsTileMode(bool isTileMode) => this.myIsTileMode = isTileMode;

    public void SetDisabled(bool isDisabled)
    {
      this.myCollider.IsDisabled = isDisabled;
      this.ObjectList.ForEach((Action<DraggableDeletable>) (o => o.SetDisabled(isDisabled)));
      for (int index1 = 0; index1 < this.myTiles.GetLength(0); ++index1)
      {
        for (int index2 = 0; index2 < this.myTiles.GetLength(1); ++index2)
        {
          if (this.myTiles[index1, index2].Sprite.Sequence == this.myGridSequence)
            this.myTiles[index1, index2].Sprite.IsHidden = isDisabled;
        }
      }
    }

    public Position GetGridPosition(Position pos)
    {
      return ((pos + (this.mySize * (float) (this.GridScale / 2) - (float) (this.GridScale / 2))) / (float) this.GridScale).Round(0);
    }

    public Tile GetGridSquare(Position pos)
    {
      Position gridPosition = this.GetGridPosition(pos);
      return this.myTiles[(int) gridPosition.X, (int) gridPosition.Y];
    }

    public void PlaceOnGrid(object sender, MouseEventArgs e)
    {
      TileSelector activeSelector = ((BrawlerSource.Framework.LevelEditor.LevelEditor) this.Level).ActiveSelector;
      if (activeSelector == null)
        return;
      if (this.myIsTileMode)
      {
        Tile gridSquare = this.GetGridSquare(e.Position);
        gridSquare.Sprite.Sequence = new Sequence()
        {
          TexturePath = activeSelector.TileInfo.SpriteName,
          Width = this.GridScale,
          Height = this.GridScale
        };
        gridSquare.Sprite.Scale = new Vector2(1f);
        gridSquare.Sprite.LoadContent();
        gridSquare.Info = activeSelector.TileInfo;
      }
      else
      {
        Position position = e.Position;
        Position dimensions = new Position((float) this.GridScale);
        Sequence sequence = new Sequence();
        sequence.TexturePath = activeSelector.TileInfo.SpriteName;
        sequence.Width = this.GridScale;
        sequence.Height = this.GridScale;
        double myDepthIndex = (double) this.myDepthIndex;
        DraggableDeletable draggableDeletable1 = new DraggableDeletable((GameObject) this, position, dimensions, sequence, (float) myDepthIndex);
        draggableDeletable1.IsResizeable = activeSelector.TileInfo.IsResizeable;
        draggableDeletable1.IsRepeating = activeSelector.TileInfo.IsRepeating;
        draggableDeletable1.SnapSize = this.GridScale;
        draggableDeletable1.Info = activeSelector.TileInfo;
        draggableDeletable1.Grid = this;
        DraggableDeletable draggableDeletable2 = draggableDeletable1;
        draggableDeletable2.LoadContent();
        this.ObjectList.Add(draggableDeletable2);
        ((BrawlerSource.Framework.LevelEditor.LevelEditor) this.Level).ActiveSelector.Button.SetColour(Color.White);
        ((BrawlerSource.Framework.LevelEditor.LevelEditor) this.Level).ActiveSelector = (TileSelector) null;
      }
    }

    public void RemoveSprite(object sender, MouseEventArgs e)
    {
      Tile gridSquare = this.GetGridSquare(e.Position);
      gridSquare.Sprite.Scale = new Vector2((float) this.GridScale / 32f);
      gridSquare.Sprite.Sequence = this.myGridSequence;
      gridSquare.Info = this.myDefaultInfo;
    }

    private void Clear()
    {
      if (this.myTiles == null)
        return;
      for (int index1 = 0; index1 < this.myTiles.GetLength(0); ++index1)
      {
        for (int index2 = 0; index2 < this.myTiles.GetLength(1); ++index2)
        {
          this.myTiles[index1, index2].Sprite.Dispose();
          this.SubGameObjects.Remove((GameObject) this.myTiles[index1, index2].Sprite);
          this.myTiles[index1, index2].Info = this.myDefaultInfo;
        }
      }
    }

    public void New()
    {
      this.Clear();
      this.myTiles = new Tile[(int) this.mySize.X, (int) this.mySize.Y];
      for (int index1 = 0; (double) index1 < (double) this.mySize.X; ++index1)
      {
        for (int index2 = 0; (double) index2 < (double) this.mySize.Y; ++index2)
        {
          Tile[,] tiles = this.myTiles;
          int index3 = index1;
          int index4 = index2;
          Tile tile = new Tile();
          Sprite sprite = new Sprite((GameObject) this);
          sprite.Sequence = this.myGridSequence;
          sprite.Position = new Position((float) (index1 * this.GridScale), (float) (index2 * this.GridScale)) - (this.mySize * (float) (this.GridScale / 2) - (float) (this.GridScale / 2));
          sprite.Depth = this.myDepthIndex;
          sprite.Scale = new Vector2((float) this.GridScale / 32f);
          tile.Sprite = sprite;
          tiles[index3, index4] = tile;
          this.myTiles[index1, index2].Sprite.LoadContent();
        }
      }
    }

    public GridProperties GetProperties()
    {
      List<DraggableDeletableProperties> myObjectProperties = (List<DraggableDeletableProperties>) null;
      if (this.ObjectList.Count > 0)
      {
        myObjectProperties = new List<DraggableDeletableProperties>();
        this.ObjectList.ForEach((Action<DraggableDeletable>) (o => myObjectProperties.Add(o.GetProperties())));
      }
      List<List<TileInfo>> tileInfoListList = new List<List<TileInfo>>();
      for (int index1 = 0; index1 < this.myTiles.GetLength(1); ++index1)
      {
        tileInfoListList.Add(new List<TileInfo>());
        for (int index2 = 0; index2 < this.myTiles.GetLength(0); ++index2)
          tileInfoListList[index1].Add(this.myTiles[index2, index1].Info);
      }
      return new GridProperties()
      {
        GridScale = this.GridScale,
        Objects = myObjectProperties,
        Tiles = tileInfoListList
      };
    }

    public void SetProperties(GridProperties properties)
    {
      this.Clear();
      this.GridScale = properties.GridScale;
      if (properties.Objects != null)
      {
        foreach (DraggableDeletableProperties properties1 in properties.Objects)
        {
          DraggableDeletable draggableDeletable1 = new DraggableDeletable((GameObject) this, new Position());
          draggableDeletable1.SnapSize = this.GridScale;
          draggableDeletable1.Grid = this;
          DraggableDeletable draggableDeletable2 = draggableDeletable1;
          this.ObjectList.Add(draggableDeletable2);
          draggableDeletable2.SetProperties(properties1);
          draggableDeletable2.LoadContent();
        }
      }
      this.myTiles = new Tile[properties.Tiles[0].Count, properties.Tiles.Count];
      for (int index1 = 0; index1 < properties.Tiles.Count; ++index1)
      {
        List<TileInfo> tile1 = properties.Tiles[index1];
        for (int index2 = 0; index2 < tile1.Count; ++index2)
        {
          TileInfo tileInfo = tile1[index2];
          Tile[,] tiles = this.myTiles;
          int index3 = index2;
          int index4 = index1;
          Tile tile2 = new Tile();
          tile2.Info = tileInfo;
          Sprite sprite = new Sprite((GameObject) this);
          Sequence sequence;
          if (tileInfo.SpriteName != null)
            sequence = new Sequence()
            {
              TexturePath = tileInfo.SpriteName,
              Width = this.GridScale,
              Height = this.GridScale
            };
          else
            sequence = this.myGridSequence;
          sprite.Sequence = sequence;
          sprite.Position = new Position((float) (index2 * this.GridScale), (float) (index1 * this.GridScale)) - (this.mySize * (float) (this.GridScale / 2) - (float) (this.GridScale / 2));
          sprite.Depth = this.myDepthIndex;
          sprite.Scale = new Vector2(tileInfo.SpriteName == null ? (float) this.GridScale / 32f : 1f);
          tile2.Sprite = sprite;
          tiles[index3, index4] = tile2;
          this.myTiles[index2, index1].Sprite.LoadContent();
        }
      }
    }
  }
}
