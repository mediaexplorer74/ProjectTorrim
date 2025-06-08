
// Type: BrawlerSource.Buyer
// Assembly: ProjectTorrim, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC8AE86A-8582-48C1-AE0D-A25C0B7D84A4
//

using BrawlerSource.GameInfo;
using BrawlerSource.Graphics;
using BrawlerSource.Input;
using BrawlerSource.UI;
using Microsoft.Xna.Framework;
using System;

#nullable disable
namespace BrawlerSource
{
  public class Buyer : DrawableGameObject
  {
    private Tower myTower;
    private Button myButton;
    private BuyerPopout myTowerInfo;
    private Sprite myTowerSprite;
    private Text myCostText;

    public Buyer(Layer layer, Position position, Type towerType)
      : base(layer)
    {
      this.myTower = (Tower) Activator.CreateInstance(towerType, (object) this, (object) new Position());
      this.SubGameObjects.Remove((GameObject) this.myTower);
      this.myButton = new Button((GameObject) this, Align.Bottom | Align.Left, position, 
          new Position(80f, 32f), new MouseFunction(this.ShowInfo), new TouchFunction(this.ShowInfo));
      this.myButton.OnRealign += new EventHandler(this.MyButton_OnRealign);
      this.Position = this.myButton.Position;
      Sprite sprite = new Sprite((GameObject) this);
      sprite.Sequence = new Sequence()
      {
        TexturePath = this.myTower.mySprite.Sequence.TexturePath,
        Width = this.myTower.mySprite.Sequence.Width,
        Height = this.myTower.mySprite.Sequence.Height
      };
      sprite.Scale = new Vector2(1f);
      sprite.Depth = 1f;
      sprite.Position = this.Position + new Position(-24f, 0.0f);
      sprite.Colour = Color.White;
      this.myTowerSprite = sprite;
      Text text = new Text((GameObject) this);
      text.FontPath = "Font";
      text.Colour = Color.Black;
      text.Depth = 1f;
      text.Scale = new Vector2(0.5f);
      text.Position = this.Position + new Position(8f, 0.0f);
      text.String = string.Format("{0}d", (object) this.myTower.Cost);
      this.myCostText = text;
      this.myTowerInfo = new BuyerPopout((GameObject) this, this.Position, this.myTower);
      this.myTowerInfo.IsHidden = true;
    }

    private void MyButton_OnRealign(object sender, EventArgs e)
    {
      this.Position = this.myButton.Position;
      this.myTowerSprite.Position = this.Position + new Position(-24f, 0.0f);
      this.myCostText.Position = this.Position + new Position(8f, 0.0f);
    }

    public override void Update(GameTime gameTime)
    {
      bool flag = ((GameLevel) this.Level).Score < this.myTower.Cost;
      this.myButton.SpriteMain.Colour = flag ? Color.DarkGray : Color.White;
      this.myTowerInfo.BuyButton.IsDisabled = flag;
      base.Update(gameTime);
    }

    public void ShowInfo(object sender, BrawlerEventArgs e) => this.ShowInfo();

    public void ShowInfo()
    {
      UILayer layer = (UILayer) this.Layer;
      int num = layer.ActiveBuyer == this ? 1 : 0;
      if (layer.ActiveBuyer != null)
        layer.ActiveBuyer.HideInfo();
      if (num != 0)
        return;
      layer.ActiveBuyer = this;
      this.myTowerInfo.IsHidden = this.IsHidden;
    }

    public void HideInfo(object sender, BrawlerEventArgs e) => this.HideInfo();

    public void HideInfo()
    {
      this.myTowerInfo.IsHidden = true;
      ((UILayer) this.Layer).ActiveBuyer = (Buyer) null;
    }
  }
}
