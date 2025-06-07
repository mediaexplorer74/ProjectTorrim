
// Type: BrawlerSource.TowerManager
// Assembly: ProjectTorrim, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC8AE86A-8582-48C1-AE0D-A25C0B7D84A4
//

using BrawlerSource.GameInfo;
using BrawlerSource.Graphics;
using BrawlerSource.Input;
using BrawlerSource.UI;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

#nullable disable
namespace BrawlerSource
{
  public class TowerManager : DrawableGameObject
  {
    private Panel myPanel;
    private Button myRangeButton;
    private Button mySpeedButton;
    private Button myPowerButton;
    private Button myTargetButton;
    private Button mySellButton;
    private Button myCloseButton;
    private Dictionary<string, Sprite> myTowerSprites;
    private string myActiveTowerName;
    private Text myTowerText;
    private Text myTowerDescriptionText;
    private Text myTargetText;
    private Dictionary<string, Tuple<Text, Button>> myStats;
    private float mySellModifier = 0.8f;
    private float myRangeUpdatePriceModifier = 0.5f;
    private float myDamageUpdatePriceModifier = 1f;
    private float mySpeedUpdatePriceModifier = 1.5f;
    private float myRangeModifier = 1.2f;
    private float mySpeedModifier = 0.8f;
    private float myDamageMofidier = 1.25f;

    public TowerManager(Layer layer)
      : base(layer)
    {
      this.Construct();
    }

    public TowerManager(GameObject parent)
      : base(parent)
    {
      this.Construct();
    }

    protected virtual void Construct()
    {
      this.IsHidden = true;
      this.myPanel = new Panel((GameObject) this, new Vector2(40f, 50f), Align.Right, new Position(0.0f, 92f));
      this.myPanel.mySprite.Colour = new Color(Color.Black, 0.5f);
      this.Position = this.myPanel.Position + new Position(0.0f, -24f);
      this.myTowerSprites = new Dictionary<string, Sprite>();
      Text text1 = new Text((GameObject) this);
      text1.FontPath = "Font";
      text1.String = "Tower";
      text1.Colour = new Color(247, 209, 0);
      text1.Position = this.Position + new Position(0.0f, -80f);
      text1.Scale = new Vector2(1f);
      text1.Depth = 0.5f;
      this.myTowerText = text1;
      Text text2 = new Text((GameObject) this);
      text2.FontPath = "Font";
      text2.String = "Tower";
      text2.Colour = Color.White;
      text2.Position = this.Position + new Position(64f, -48f);
      text2.Scale = new Vector2(0.5f);
      text2.Depth = 0.5f;
      this.myTowerDescriptionText = text2;
      this.myRangeButton = new Button((GameObject) this, Align.Centre, this.Position + new Position(144f, -16f), new Position(64f, 32f), new MouseFunction(this.UpgradeRange));
      this.myRangeButton.SetColour(Color.LimeGreen);
      this.myPowerButton = new Button((GameObject) this, Align.Centre, this.Position + new Position(144f, 16f), new Position(64f, 32f), new MouseFunction(this.UpgradePower));
      this.myPowerButton.SetColour(Color.LimeGreen);
      this.myPowerButton.SetText("Power");
      this.mySpeedButton = new Button((GameObject) this, Align.Centre, this.Position + new Position(144f, 48f), new Position(64f, 32f), new MouseFunction(this.UpgradeSpeed));
      this.mySpeedButton.SetColour(Color.LimeGreen);
      this.mySpeedButton.SetText("Speed");
      this.myStats = new Dictionary<string, Tuple<Text, Button>>();
      this.AddStat("Range", this.Position + new Position(32f, -16f), this.myRangeButton);
      this.AddStat("Power", this.Position + new Position(32f, 16f), this.myPowerButton);
      this.AddStat("Speed", this.Position + new Position(32f, 48f), this.mySpeedButton);
      Sprite sprite = new Sprite((GameObject) this);
      sprite.Sequence = new Sequence()
      {
        TexturePath = "Target",
        Width = 32,
        Height = 32
      };
      sprite.Position = this.Position + new Position(-32f, 80f);
      sprite.Depth = 0.5f;
      Text text3 = new Text((GameObject) this);
      text3.FontPath = "Font";
      text3.String = "";
      text3.Colour = Color.White;
      text3.Position = this.Position + new Position(48f, 80f);
      text3.Scale = new Vector2(0.75f);
      text3.Depth = 0.5f;
      this.myTargetText = text3;
      this.myTargetButton = new Button((GameObject) this, Align.Centre, this.Position + new Position(144f, 80f), new Position(64f, 32f), new MouseFunction(this.ChangeTarget));
      this.myTargetButton.SetColour(Color.MonoGameOrange);
      this.myTargetButton.SetText("Mode");
      this.mySellButton = new Button((GameObject) this, Align.Centre, this.Position + new Position(8f, 128f), new Position(160f, 32f), new MouseFunction(this.SellTower));
      this.mySellButton.SetColour(Color.LimeGreen);
      this.mySellButton.SetTextScale(new Vector2(0.75f));
      this.mySellButton.SetText("Sell");
      this.myCloseButton = new Button((GameObject) this, Align.Centre, this.Position + new Position(136f, 128f), new Position(80f, 32f), new MouseFunction(this.HideInfo));
      this.myCloseButton.SetColour(Color.Firebrick);
      this.myCloseButton.SetTextScale(new Vector2(0.75f));
      this.myCloseButton.SetText("Close");
    }

    public void AddStat(string statName, Position position, Button upgradeButton)
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
      Text text1 = new Text((GameObject) this);
      text1.FontPath = "Font";
      text1.String = "";
      text1.Colour = Color.White;
      text1.Position = position + new Position(0.0f, 0.0f);
      text1.Scale = new Vector2(0.75f);
      text1.Depth = 0.5f;
      Text text2 = text1;
      this.myStats.Add(statName, new Tuple<Text, Button>(text2, upgradeButton));
    }

    public void UpdateStat(string statName, float statValue, float cost, int level, int maxLevel)
    {
      this.myStats[statName].Item1.String = string.Format("{0}", (object) statValue);
      this.myStats[statName].Item2.SetText(level < maxLevel ? string.Format("Upgrade\n{0}d", (object) cost) : "MAX");
    }

    public override void Update(GameTime gameTime)
    {
      Tower activeTower = ((GameLayer) this.Level.GameLayer).ActiveTower;
      GameLevel level = (GameLevel) this.Level;
      if (activeTower != null)
      {
        this.myRangeButton.SetIsDisabled(activeTower.DiameterUpgrade >= activeTower.MaxDiameterUpgrade || (double) level.Score < (double) activeTower.Cost * (double) this.myRangeUpdatePriceModifier);
        this.mySpeedButton.SetIsDisabled(activeTower.SpeedUpgrade >= activeTower.MaxSpeedUpgrade || (double) level.Score < (double) activeTower.Cost * (double) this.mySpeedUpdatePriceModifier);
        this.myPowerButton.SetIsDisabled(activeTower.ProjectileUpgrade >= activeTower.MaxProjectileUpgrade || (double) level.Score < (double) activeTower.Cost * (double) this.myDamageUpdatePriceModifier);
      }
      this.myRangeButton.IsHidden = this.IsHidden;
      this.mySpeedButton.IsHidden = this.IsHidden;
      this.myPowerButton.IsHidden = this.IsHidden;
      this.myTargetButton.IsHidden = this.IsHidden;
      this.mySellButton.IsHidden = this.IsHidden;
      this.myCloseButton.IsHidden = this.IsHidden;
      base.Update(gameTime);
    }

    public void ShowInfo(object sender, BrawlerEventArgs e) => this.ShowInfo();

    public void ShowInfo()
    {
      if (!(((GameLayer) this.Level.GameLayer).ActiveTower.Layer is GameLayer layer))
        return;
      Tower activeTower = layer.ActiveTower;
      this.IsHidden = !this.IsHidden && activeTower.myDragger.IsDisabled;
      if (!this.IsHidden)
        this.SetTowerGraphics(activeTower);
      if (activeTower.myDistanceSprite == null)
        return;
      activeTower.myDistanceSprite.IsHidden = this.IsHidden;
    }

    private void SetTowerGraphics(Tower activeTower)
    {
      if (!string.IsNullOrEmpty(this.myActiveTowerName))
        this.myTowerSprites[this.myActiveTowerName].IsHidden = true;
      this.myActiveTowerName = activeTower.Name;
      if (!this.myTowerSprites.ContainsKey(activeTower.Name))
      {
        Sprite sprite1 = new Sprite((GameObject) this);
        sprite1.Sequence = activeTower.mySprite.Sequence;
        sprite1.Position = this.Position + new Position((float) sbyte.MinValue, 0.0f);
        sprite1.Scale = new Vector2(4f);
        sprite1.Depth = 1f;
        Sprite sprite2 = sprite1;
        sprite2.LoadContent();
        this.myTowerSprites.Add(this.myActiveTowerName, sprite2);
      }
      this.myTowerText.String = this.myActiveTowerName;
      this.myTowerDescriptionText.String = activeTower.Description;
      this.myTowerSprites[this.myActiveTowerName].IsHidden = false;
      this.myTargetText.String = activeTower.FindFunctions[activeTower.FindIndex].Item2;
      this.mySellButton.SetText(string.Format("Sell {0}d", (object) (float) ((double) activeTower.Cost * (double) this.mySellModifier)));
      this.UpdateStat("Range", activeTower.Diameter, (float) activeTower.Cost * this.myRangeUpdatePriceModifier, activeTower.DiameterUpgrade, activeTower.MaxDiameterUpgrade);
      this.UpdateStat("Speed", (float) activeTower.FireRate.TotalSeconds, (float) activeTower.Cost * this.mySpeedUpdatePriceModifier, activeTower.SpeedUpgrade, activeTower.MaxSpeedUpgrade);
      this.UpdateStat("Power", (float) activeTower.ProjectileInfo.Damage, (float) activeTower.Cost * this.myDamageUpdatePriceModifier, activeTower.ProjectileUpgrade, activeTower.MaxProjectileUpgrade);
    }

    public void HideInfo(object sender, BrawlerEventArgs e) => this.HideInfo();

    public void HideInfo()
    {
      Tower activeTower = ((GameLayer) this.Level.GameLayer).ActiveTower;
      this.IsHidden = activeTower == null || !activeTower.myDragger.IsDragging;
      if (activeTower == null || !(activeTower.Layer is GameLayer) || !this.IsHidden)
        return;
      if (activeTower.myDistanceSprite != null)
        activeTower.myDistanceSprite.IsHidden = this.IsHidden;
      ((GameLayer) this.Level.GameLayer).ActiveTower = (Tower) null;
    }

    public void SellTower(object sender, BrawlerEventArgs e) => this.SellTower();

    public void SellTower()
    {
      Tower activeTower = ((GameLayer) this.Level.GameLayer).ActiveTower;
      if (activeTower != null)
      {
        ((GameLevel) this.Level).Score += (int) ((double) activeTower.Cost * (double) this.mySellModifier);
        activeTower.QueueDispose();
      }
      this.HideInfo();
    }

    public void ChangeTarget(object sender, BrawlerEventArgs e) => this.ChangeTarget();

    public void ChangeTarget()
    {
      Tower activeTower = ((GameLayer) this.Level.GameLayer).ActiveTower;
      ++activeTower.FindIndex;
      this.myTargetText.String = activeTower.FindFunctions[activeTower.FindIndex].Item2;
    }

    public void UpgradeRange(object sender, BrawlerEventArgs e) => this.UpgradeRange();

    public void UpgradeRange()
    {
      Tower activeTower = ((GameLayer) this.Level.GameLayer).ActiveTower;
      GameLevel level = (GameLevel) this.Level;
      if (activeTower.DiameterUpgrade >= activeTower.MaxDiameterUpgrade || (double) level.Score < (double) activeTower.Cost * (double) this.myRangeUpdatePriceModifier)
        return;
      activeTower.Diameter *= this.myRangeModifier;
      ++activeTower.DiameterUpgrade;
      level.Score -= (int) ((double) activeTower.Cost * (double) this.myRangeUpdatePriceModifier);
      this.UpdateStat("Range", activeTower.Diameter, (float) activeTower.Cost * this.myRangeUpdatePriceModifier, activeTower.DiameterUpgrade, activeTower.MaxDiameterUpgrade);
    }

    public void UpgradeSpeed(object sender, BrawlerEventArgs e) => this.UpgradeSpeed();

    public void UpgradeSpeed()
    {
      Tower activeTower = ((GameLayer) this.Level.GameLayer).ActiveTower;
      GameLevel level = (GameLevel) this.Level;
      if (activeTower.SpeedUpgrade >= activeTower.MaxSpeedUpgrade || (double) level.Score < (double) activeTower.Cost * (double) this.mySpeedUpdatePriceModifier)
        return;
      activeTower.FireRate = TimeSpan.FromMilliseconds(activeTower.FireRate.TotalMilliseconds * (double) this.mySpeedModifier);
      ++activeTower.SpeedUpgrade;
      level.Score -= (int) ((double) activeTower.Cost * (double) this.mySpeedUpdatePriceModifier);
      this.UpdateStat("Speed", (float) activeTower.FireRate.TotalSeconds, (float) activeTower.Cost * this.mySpeedUpdatePriceModifier, activeTower.SpeedUpgrade, activeTower.MaxSpeedUpgrade);
    }

    public void UpgradePower(object sender, BrawlerEventArgs e) => this.UpgradePower();

    public void UpgradePower()
    {
      Tower activeTower = ((GameLayer) this.Level.GameLayer).ActiveTower;
      GameLevel level = (GameLevel) this.Level;
      if (activeTower.ProjectileUpgrade >= activeTower.MaxProjectileUpgrade || (double) level.Score < (double) activeTower.Cost * (double) this.myDamageUpdatePriceModifier)
        return;
      activeTower.ProjectileInfo.Damage = (int) ((double) activeTower.ProjectileInfo.Damage * (double) this.myDamageMofidier);
      ++activeTower.ProjectileInfo.MaxHitCount;
      ++activeTower.ProjectileUpgrade;
      level.Score -= (int) ((double) activeTower.Cost * (double) this.myDamageUpdatePriceModifier);
      this.UpdateStat("Power", (float) activeTower.ProjectileInfo.Damage, (float) activeTower.Cost * this.myDamageUpdatePriceModifier, activeTower.ProjectileUpgrade, activeTower.MaxProjectileUpgrade);
    }
  }
}
