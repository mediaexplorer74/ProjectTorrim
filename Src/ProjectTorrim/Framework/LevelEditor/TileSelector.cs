
// Type: BrawlerSource.Framework.LevelEditor.TileSelector
// Assembly: ProjectTorrim, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC8AE86A-8582-48C1-AE0D-A25C0B7D84A4
//

using BrawlerSource.Input;
using BrawlerSource.UI;
using Microsoft.Xna.Framework;

#nullable disable
namespace BrawlerSource.Framework.LevelEditor
{
  public class TileSelector : DrawableGameObject
  {
    public Button Button;
    public TileInfo TileInfo;

    public TileSelector(Layer layer, TileInfo tileInfo, Position position, Panel scrollArea)
      : base(layer)
    {
      this.TileInfo = tileInfo;
      this.Button = new Button((GameObject) this, Align.Bottom | Align.Left, position, new Position(32f), 
          new MouseFunction(this.ActivateButton), new TouchFunction(this.ActivateButton));
      this.Button.Anchor = (Element) scrollArea;
      this.Button.SetSprite(this.TileInfo.SpriteName);
    }

    public void ActivateButton(object sender, MouseEventArgs e)
    {
      TileSelector activeSelector = ((BrawlerSource.Framework.LevelEditor.LevelEditor) this.Level).ActiveSelector;
      activeSelector?.Button.SetColour(Color.White);
      if (activeSelector == this)
      {
        ((BrawlerSource.Framework.LevelEditor.LevelEditor) this.Level).ActiveSelector = (TileSelector) null;
      }
      else
      {
        ((BrawlerSource.Framework.LevelEditor.LevelEditor) this.Level).ActiveSelector = this;
        this.Button.SetColour(Color.Gray);
      }
    }


    public void ActivateButton(object sender, TouchEventArgs e)
    {
        TileSelector activeSelector = ((BrawlerSource.Framework.LevelEditor.LevelEditor)this.Level).ActiveSelector;
        activeSelector?.Button.SetColour(Color.White);
        if (activeSelector == this)
        {
            ((BrawlerSource.Framework.LevelEditor.LevelEditor)this.Level).ActiveSelector = (TileSelector)null;
        }
        else
        {
            ((BrawlerSource.Framework.LevelEditor.LevelEditor)this.Level).ActiveSelector = this;
            this.Button.SetColour(Color.Gray);
        }
    }


    }
}
