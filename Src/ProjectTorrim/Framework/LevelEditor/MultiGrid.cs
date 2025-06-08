
// Type: BrawlerSource.Framework.LevelEditor.MultiGrid
// Assembly: ProjectTorrim, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC8AE86A-8582-48C1-AE0D-A25C0B7D84A4
//

using BrawlerSource.Input;
using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using Windows.Storage;
//using System.Text.Json;

#nullable disable
namespace BrawlerSource.Framework.LevelEditor
{
  public class MultiGrid : DrawableGameObject
  {
    private List<Grid> myGrids;
    public int Index;
    private int myMaxIndex;
    public int GridScale;
    private Position mySize;
    private InputEvents myInput;

    public bool IsTileMode { get; private set; }

    public MultiGrid(Layer layer, Position size)
      : base(layer)
    {
      this.myMaxIndex = 8;
      this.GridScale = 32;
      this.mySize = size;
      this.myGrids = new List<Grid>();
      this.Index = 0;
      this.myInput = new InputEvents((GameObject) this);
      this.myInput.AddKey((Keys) 83, InputType.Pressed, new KeyFunction(this.Save));
      this.myInput.AddKey((Keys) 76, InputType.Pressed, new KeyFunction(this.Load));
      this.myInput.AddKey((Keys) 78, InputType.Pressed, new KeyFunction(this.New));
    }

    public void ToggleMode() => this.SetIsTileMode(!this.IsTileMode);

    private void SetIsTileMode(bool isTileMode)
    {
      this.IsTileMode = isTileMode;
      foreach (Grid grid in this.myGrids)
        grid.SetIsTileMode(isTileMode);
    }

    public void AddGrid()
    {
      this.myGrids.Add(new Grid((GameObject) this, this.Index, this.myMaxIndex, 
          this.mySize / (float) this.GridScale, this.GridScale, this.IsTileMode));
    }

    public void IncreaseGridScale(object sender, MouseEventArgs e) => this.GridScale *= 2;

    public void DecreaseGridScale(object sender, MouseEventArgs e)
    {
      if (this.GridScale <= 1)
        return;
      this.GridScale /= 2;
    }

    public void IncreaseGridLayer(object sender, MouseEventArgs e) => this.IncreaseGridLayer();

    public void IncreaseGridLayer()
    {
      if (this.Index >= this.myMaxIndex)
        return;
      if (this.myGrids.Count > 0)
        this.myGrids[this.Index].SetDisabled(true);
      ++this.Index;
      if (this.Index == this.myGrids.Count)
        this.AddGrid();
      this.myGrids[this.Index].SetDisabled(false);
      this.GridScale = this.myGrids[this.Index].GridScale;
    }

    public void DecreaseGridLayer(object sender, MouseEventArgs e)
    {
      this.myGrids[this.Index].SetDisabled(true);
      if (this.Index > 0)
        --this.Index;
      this.myGrids[this.Index].SetDisabled(false);
      this.GridScale = this.myGrids[this.Index].GridScale;
    }

    public void ResetActive(object sender, MouseEventArgs e) => this.ResetActive();

    public void ResetActive()
    {
      this.myGrids[this.Index].Dispose();
      this.myGrids[this.Index] = new Grid((GameObject) this, this.Index, this.myMaxIndex,
          this.mySize / (float) this.GridScale, this.GridScale, this.IsTileMode);
    }

    private void ClearAll()
    {
      foreach (GameObject grid in this.myGrids)
        grid.Dispose();
      this.myGrids = new List<Grid>();
      this.Index = 0;
    }

    public void New(object sender, KeyEventArgs e) => this.New();

    public void New(object sender, MouseEventArgs e) => this.New();

    public void New()
    {
      this.ClearAll();
      this.AddGrid();
    }

    public void Save(object sender, KeyEventArgs e) => this.Save();

    public void Save(object sender, MouseEventArgs e) => this.Save();

    public void Save()
    {
        StorageFolder appdataFolder = ApplicationData.Current.LocalFolder;
        using (FileStream fileStream = new FileStream(
            System.IO.Path.Combine(appdataFolder.Path, "save.json"), 
            FileMode.Create, 
            FileAccess.Write))
        using (StreamWriter streamWriter = new StreamWriter(fileStream))
        {
            string str = JsonConvert.SerializeObject(this.GetProperties(), Formatting.Indented);
            streamWriter.WriteLine(str);
        }
    }

    public void Load(object sender, KeyEventArgs e) => this.Load();

    public void Load(object sender, MouseEventArgs e) => this.Load();

    public void Load()
    {
      StorageFolder appdataFolder = ApplicationData.Current.LocalFolder;
      this.ClearAll();
      --this.Index;
      this.SetProperties(JsonConvert.DeserializeObject<MultiGridProperties>(
          File.ReadAllText(
             System.IO.Path.Combine(appdataFolder.Path, "save.json"))));
    }

    public MultiGridProperties GetProperties()
    {
      List<GridProperties> grids = new List<GridProperties>();
      this.myGrids.ForEach((Action<Grid>) (g => grids.Add(g.GetProperties())));
      return new MultiGridProperties() { Grids = grids };
    }

    public void SetProperties(MultiGridProperties properties)
    {
      foreach (GridProperties grid in properties.Grids)
      {
        this.IncreaseGridLayer();
        this.GridScale = grid.GridScale;
        this.ResetActive();
        this.myGrids[this.Index].SetProperties(grid);
      }
    }
  }
}
