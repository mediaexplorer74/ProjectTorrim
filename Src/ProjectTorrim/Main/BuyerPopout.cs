
// Type: BrawlerSource.BuyerPopout
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
  public class BuyerPopout : DrawableGameObject
  {
    private Tower myTower;
    private Tower myNewTower;
    private Tower myNewTowerBuffer;
    private TimeSpan myLastSpawnTime;
    private TimeSpan mySpawnRate;
    public Button BuyButton;
    private Button myCloseButton;

    public BuyerPopout(GameObject parent, Position position, Tower tower)
      : base(parent)
    {
      this.Position = position + new Position(0.0f, -172f);
      if ((double) this.Position.X - 196.0 < -480.0)
        this.Position.X = -284f;
      if ((double) this.Position.X + 196.0 > 96.0)
        this.Position.X = -100f;
      this.myTower = tower;
      this.mySpawnRate = TimeSpan.FromMilliseconds(500.0);
      this.myLastSpawnTime = TimeSpan.FromSeconds(0.0);
      Sprite sprite1 = new Sprite((GameObject) this);
      sprite1.Sequence = new Sequence()
      {
        TexturePath = "Square_256",
        Width = 256,
        Height = 256
      };
      sprite1.Position = this.Position + new Position(0.0f, 24f);
      sprite1.Scale = Vector2.Divide(new Vector2(392f, 248f), 256f);
      sprite1.Depth = 0.0f;
      sprite1.Colour = new Color(Color.Black, 0.5f);
      Sprite sprite2 = new Sprite((GameObject) this);
      sprite2.Sequence = this.myTower.mySprite.Sequence;
      sprite2.Position = this.Position + new Position((float) sbyte.MinValue, 0.0f);
      sprite2.Scale = new Vector2(4f);
      sprite2.Depth = 1f;
      Text text1 = new Text((GameObject) this);
      text1.FontPath = "Font";
      text1.String = this.myTower.Name ?? "";
      text1.Colour = new Color(247, 209, 0);
      text1.Position = this.Position + new Position(0.0f, -64f);
      text1.Scale = new Vector2(1f);
      text1.Depth = 0.5f;
      Text text2 = new Text((GameObject) this);
      text2.FontPath = "Font";
      text2.String = this.myTower.Description ?? "";
      text2.Colour = Color.White;
      text2.Position = this.Position + new Position(64f, -32f);
      text2.Scale = new Vector2(0.5f);
      text2.Depth = 0.5f;
      this.AddStat("Range", this.myTower.Diameter, this.Position + new Position(32f, 0.0f));
      this.AddStat("Power", (float) this.myTower.ProjectileInfo.Damage, this.Position + new Position(32f, 32f));
      this.AddStat("Speed", (float) this.myTower.FireRate.TotalSeconds, this.Position + new Position(32f, 64f));
      this.BuyButton = new Button((GameObject) this, Align.Centre, 
          this.Position + new Position(8f, 112f), new Position(160f, 32f), 
          new MouseFunction(this.SpawnTower),
          new TouchFunction(this.SpawnTower));
      this.BuyButton.SetColour(Color.LimeGreen);
      this.BuyButton.SetTextScale(new Vector2(0.75f));
      this.BuyButton.SetText(string.Format("Buy {0}d", (object) this.myTower.Cost));
      this.myCloseButton = new Button((GameObject) this, Align.Centre, 
          this.Position + new Position(136f, 112f), new Position(80f, 32f), 
          new MouseFunction(((Buyer) this.Parent).HideInfo),
          new TouchFunction(((Buyer)this.Parent).HideInfo));
      this.myCloseButton.SetColour(Color.Firebrick);
      this.myCloseButton.SetTextScale(new Vector2(0.75f));
      this.myCloseButton.SetText("Close");
    }

    public void AddStat(string statName, float statValue, Position position)
    {
      Sprite sprite = new Sprite((GameObject) this);
      sprite.Sequence = new Sequence()
      {
        TexturePath = statName,
        Width = 32,
        Height = 32
      };
      sprite.Position = position + new Position(-64f, 0.0f);
      sprite.Depth = 0.5f;
      Text text = new Text((GameObject) this);
      text.FontPath = "Font";
      text.String = string.Format("{0} {1}", (object) statValue, (object) statName);
      text.Colour = Color.White;
      text.Position = position + new Position(32f, 0.0f);
      text.Scale = new Vector2(0.75f);
      text.Depth = 0.5f;
    }

    public override void Update(GameTime gameTime)
    {
      if (this.myNewTower != null)
      {
        this.myNewTower.myDragger.SetIsDragging(gameTime, true);
        this.myNewTower = (Tower) null;
      }
      this.myNewTower = this.myNewTowerBuffer;
      this.myNewTowerBuffer = (Tower) null;
      this.BuyButton.IsHidden = this.IsHidden;
      this.myCloseButton.IsHidden = this.IsHidden;
      base.Update(gameTime);
    }

    public void SpawnTower(object sender, BrawlerEventArgs e)
    {
      ((Buyer) this.Parent).HideInfo();
      if (!(e.GameTime.TotalGameTime - this.myLastSpawnTime > this.mySpawnRate))
        return;

      ((GameLevel) this.Level).Score -= this.myTower.Cost;

      this.myNewTowerBuffer = (Tower) Activator.CreateInstance(this.myTower.GetType(), 
          (object) this.Level.GameLayer, (object) this.Level.GameLayer.Cursor.Position);
      this.myNewTowerBuffer.LoadContent();
      this.myNewTowerBuffer.AddToDraw();
      this.myNewTowerBuffer.Select();
      this.myLastSpawnTime = e.GameTime.TotalGameTime;
    }
  }
}
