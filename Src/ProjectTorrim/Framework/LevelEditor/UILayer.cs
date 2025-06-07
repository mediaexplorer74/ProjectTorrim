
// Type: BrawlerSource.Framework.LevelEditor.UILayer
// Assembly: ProjectTorrim, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC8AE86A-8582-48C1-AE0D-A25C0B7D84A4
//

using BrawlerSource.Input;
using BrawlerSource.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.IO;
//using System.Text.Json;
using Newtonsoft.Json;

#nullable disable
namespace BrawlerSource.Framework.LevelEditor
{
  public class UILayer(Level level, int index) : Layer(level, index)
  {
    public GridLayer gl;
    private BrawlerSource.Graphics.Text gridLayerText;
    private BrawlerSource.Graphics.Text gridScaleText;
    private Button btnMode;

    public override void Initilize()
    {
      base.Initilize();
      new Panel((Layer) this, new Vector2(200f, 10f), Align.Top, new Position(0.0f)).AddToDraw();
      Button button1 = new Button((Layer) this, Align.Top | Align.Left, new Position(0.0f, 0.0f), new Position(48f, 32f), new MouseFunction(this.gl.Grids.New));
      button1.SetText("New");
      button1.AddToDraw();
      Button button2 = new Button((Layer) this, Align.Top | Align.Left, new Position(48f, 0.0f), new Position(48f, 32f), new MouseFunction(this.gl.Grids.Save));
      button2.SetText("Save");
      button2.AddToDraw();
      Button button3 = new Button((Layer) this, Align.Top | Align.Left, new Position(96f, 0.0f), new Position(48f, 32f), new MouseFunction(this.gl.Grids.Load));
      button3.SetText("Load");
      button3.AddToDraw();
      this.btnMode = new Button((Layer) this, Align.Top, new Position(-160f, 0.0f), new Position(96f, 32f), new MouseFunction(this.ToggleMode));
      this.btnMode.SetText("Object Mode");
      this.btnMode.AddToDraw();
      BrawlerSource.Graphics.Text text1 = new BrawlerSource.Graphics.Text((Layer) this);
      text1.FontPath = "Font";
      text1.Colour = Color.Black;
      text1.Depth = 1f;
      text1.Scale = new Vector2(1f);
      text1.Position = new Position(304f, -250f);
      text1.String = "Layer";
      BrawlerSource.Graphics.Text text2 = text1;
      this.GameObjects.Add((GameObject) text2);
      text2.AddToDraw();
      Button button4 = new Button((Layer) this, Align.Top | Align.Right, new Position(0.0f, 0.0f), new Position(32f), new MouseFunction(this.gl.Grids.IncreaseGridLayer));
      button4.SetColour(Color.OldLace);
      button4.SetText("+");
      button4.AddToDraw();
      BrawlerSource.Graphics.Text text3 = new BrawlerSource.Graphics.Text((Layer) this);
      text3.FontPath = "Font";
      text3.Colour = Color.Black;
      text3.Depth = 1f;
      text3.Scale = new Vector2(1f);
      text3.Position = new Position(416f, -250f);
      this.gridLayerText = text3;
      this.GameObjects.Add((GameObject) this.gridLayerText);
      this.gridLayerText.AddToDraw();
      Button button5 = new Button((Layer) this, Align.Top | Align.Right, new Position(-96f, 0.0f), new Position(32f), new MouseFunction(this.gl.Grids.DecreaseGridLayer));
      button5.SetColour(Color.OldLace);
      button5.SetText("-");
      button5.AddToDraw();
      BrawlerSource.Graphics.Text text4 = new BrawlerSource.Graphics.Text((Layer) this);
      text4.FontPath = "Font";
      text4.Colour = Color.Black;
      text4.Depth = 1f;
      text4.Scale = new Vector2(1f);
      text4.Position = new Position(-32f, -250f);
      text4.String = "Size";
      BrawlerSource.Graphics.Text text5 = text4;
      this.GameObjects.Add((GameObject) text5);
      text5.AddToDraw();
      Button button6 = new Button((Layer) this, Align.Top | Align.Right, new Position(-336f, 0.0f), new Position(32f), new MouseFunction(this.gl.Grids.IncreaseGridScale));
      button6.SetColour(Color.OldLace);
      button6.SetText("+");
      button6.AddToDraw();
      BrawlerSource.Graphics.Text text6 = new BrawlerSource.Graphics.Text((Layer) this);
      text6.FontPath = "Font";
      text6.Colour = Color.Black;
      text6.Depth = 1f;
      text6.Scale = new Vector2(1f);
      text6.Position = new Position(80f, -250f);
      this.gridScaleText = text6;
      this.GameObjects.Add((GameObject) this.gridScaleText);
      this.gridScaleText.AddToDraw();
      Button button7 = new Button((Layer) this, Align.Top | Align.Right, new Position(-432f, 0.0f), new Position(32f), new MouseFunction(this.gl.Grids.DecreaseGridScale));
      button7.SetColour(Color.OldLace);
      button7.SetText("-");
      button7.AddToDraw();
      Button button8 = new Button((Layer) this, Align.Top | Align.Right, new Position(-256f, 0.0f), new Position(48f, 32f), new MouseFunction(this.gl.Grids.ResetActive));
      button8.SetColour(Color.Green);
      button8.SetText("Apply");
      button8.AddToDraw();
      this.AddTileSelectors();
      new InputEvents((Layer) this).AddKey((Keys) 27, InputType.Pressed, new KeyFunction(this.Exit));
    }

    public override void Update(GameTime gameTime)
    {
      this.gridLayerText.String = string.Format("{0}", (object) this.gl.Grids.Index);
      this.gridScaleText.String = string.Format("{0}", (object) this.gl.Grids.GridScale);
      base.Update(gameTime);
    }

    public void ToggleMode(object sender, MouseEventArgs e)
    {
      this.gl.Grids.ToggleMode();
      this.btnMode.SetText(this.gl.Grids.IsTileMode ? "Tile Mode" : "Object Mode");
    }

        public void AddTileSelectors()
        {
            Panel scrollArea = new Panel((Layer)this, new Vector2(100f, 10f), Align.Bottom, new Position(0.0f));
            this.GameObjects.Add((GameObject)scrollArea);
            scrollArea.AddToDraw();
            ScrollBar scrollBar = new ScrollBar((Layer)this, new Vector2(5f), Align.Bottom);
            this.GameObjects.Add((GameObject)scrollBar);
            scrollBar.AddToDraw();
            scrollBar.AddScrollItem((DrawableGameObject)scrollArea);

            List<TileInfo> tileInfoList = JsonConvert.DeserializeObject<List<TileInfo>>(File.ReadAllText(
                this.Game.Content.RootDirectory + "\\Prefabs.json"));

            for (int index = 0; index < tileInfoList.Count; ++index)
            {
                TileSelector tileSelector = new TileSelector((Layer)this, tileInfoList[index], new Position((float)(index * 48), -20f), scrollArea);
                this.GameObjects.Add((GameObject)tileSelector);
                tileSelector.AddToDraw();
            }
        }

  
    public void Exit(object sender, BrawlerEventArgs e) => this.Game.Exit();
  }
}
