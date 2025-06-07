
// Type: BrawlerSource.Tower
// Assembly: ProjectTorrim, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC8AE86A-8582-48C1-AE0D-A25C0B7D84A4
//

using BrawlerSource.Collision;
using BrawlerSource.Collision.Intersections;
using BrawlerSource.GameInfo;
using BrawlerSource.Graphics;
using BrawlerSource.Input;
using BrawlerSource.UI;
using Microsoft.Xna.Framework;
using System;

#nullable disable
namespace BrawlerSource
{
  public class Tower : TowerBase
  {
    public Sprite mySprite;
    public Sprite myDistanceSprite;
    public SequenceManager<Directions> mySequences;
    public UniqueDraggable myDragger;
    private bool RequestDeselect;
    private ClickableCollider myClickCollider;
    public int SpeedUpgrade;
    public int MaxSpeedUpgrade = 1;
    public int DiameterUpgrade;
    public int MaxDiameterUpgrade = 3;
    public int ProjectileUpgrade;
    public int MaxProjectileUpgrade = 2;
    public int Cost;
    public string Description;
    public string Name;

    public Tower(GameObject parent, Position position, float diameter)
      : base(parent, position, diameter)
    {
    }

    public Tower(Layer layer, Position position, float diameter)
      : base(layer, position, diameter)
    {
    }

    protected override void Construct(Position position, float diameter)
    {
      base.Construct(position, diameter);
      this.SpeedUpgrade = 0;
      this.DiameterUpgrade = 0;
      this.OnDiameterChanged += new EventHandler(this.Diameter_OnValueChanged);
      UniqueDraggable uniqueDraggable = new UniqueDraggable((GameObject) this, this.Position, new Position(32f), (Sequence) null, 0.0f);
      uniqueDraggable.MinPosition = new Position(-960f, -570f);
      uniqueDraggable.MaxPosition = new Position(960f, 370f);
      uniqueDraggable.SnapSize = 8;
      this.myDragger = uniqueDraggable;
      this.myDragger.AddInvalidCollisionType(typeof (Path));
      this.myDragger.AddInvalidCollisionType(typeof (Bases));
      ClickableCollider clickableCollider = new ClickableCollider((GameObject) this);
      clickableCollider.Position = this.Position;
      this.myClickCollider = clickableCollider;
      this.myClickCollider.AddIntersection((IIntersectionable) new Circular()
      {
        Radius = 16f
      });
      this.myClickCollider.AddMouseInput(MouseButtons.Left, InputType.Pressed, new MouseFunction(this.Select));
      this.mySequences = new SequenceManager<Directions>((GameObject) this);
      Sprite sprite1 = new Sprite((GameObject) this);
      sprite1.Position = this.Position;
      sprite1.Depth = 0.9f;
      this.mySprite = sprite1;
      Sprite sprite2 = new Sprite((GameObject) this);
      sprite2.Sequence = new Sequence()
      {
        TexturePath = "Circle_256",
        Width = 256,
        Height = 256
      };
      sprite2.Position = this.Position;
      sprite2.Scale = Vector2.Divide(new Vector2(this.Diameter), 256f);
      sprite2.Colour = new Color(Color.Goldenrod, 0.03f);
      sprite2.Depth = 0.6f;
      sprite2.IsHidden = true;
      this.myDistanceSprite = sprite2;
      this.OnShooting += new EventHandler(this.Projectile_OnShooting);
    }

    private void Diameter_OnValueChanged(object sender, EventArgs e)
    {
      if (this.myDistanceSprite == null)
        return;
      this.myDistanceSprite.Scale = Vector2.Divide(new Vector2(((ValueEventArgs) e).Value), 256f);
    }

    public void Select(object sender, BrawlerEventArgs e) => this.Select();

    public void Select()
    {
      Tower activeTower = ((GameLayer) this.Layer).ActiveTower;
      if (activeTower != this)
      {
        activeTower?.Deselect();
        ((GameLayer) this.Layer).ActiveTower = this;
        ((UILayer) this.Level.UILayer).ActiveTowerManager.ShowInfo();
      }
      else
        this.RequestDeselect = true;
    }

    public void Deselect() => ((UILayer) this.Level.UILayer).ActiveTowerManager.HideInfo();

    private void Projectile_OnShooting(object sender, EventArgs e)
    {
      this.SetDirectionalSequence(((ShootEventArgs) e).Direction);
    }

    public void SetDirectionalSequence(Vector2 direction)
    {
      this.mySequences.SetSequence((double) Math.Abs(direction.Y) > (double) Math.Abs(direction.X) ? ((double) direction.Y > 0.0 ? Directions.Front : Directions.Back) : ((double) direction.X > 0.0 ? Directions.Right : Directions.Left));
    }

    public override void Update(GameTime gameTime)
    {
      if (this.myDragger.IsDragging || this.myDragger.WasDragging)
      {
        this.myDistanceSprite.Colour = this.myDragger.IsValidPosition() ? new Color(Color.OliveDrab, 0.03f) : new Color(Color.Firebrick, 0.15f);
        this.Position = this.myDragger.Position;
        this.myClickCollider.Position = this.Position;
        this.mySprite.Position = this.Position;
        this.myDistanceSprite.Position = this.Position + this.myWatchOffset;
      }
      if (this.CanShoot = !this.myDragger.IsDragging)
        this.myDistanceSprite.Colour = new Color(Color.Goldenrod, 0.03f);
      if (this.RequestDeselect && !this.myDragger.IsDragging && !this.myDragger.WasDragging)
      {
        this.RequestDeselect = false;
        if (this.myDragger.DragTime.HasValue && this.myDragger.DragTime.Value < TimeSpan.FromMilliseconds(200.0))
          this.Deselect();
      }
      base.Update(gameTime);
    }

    public TowerProperties GetProperties()
    {
      return new TowerProperties()
      {
        Type = this.GetType().ToString(),
        Position = this.Position,
        SpeedUpgrade = this.SpeedUpgrade,
        DiameterUpgrade = this.DiameterUpgrade,
        ProjectileUpgrade = this.ProjectileUpgrade,
        FindIndex = this.FindIndex
      };
    }

    public void SetProperties(TowerProperties properties)
    {
      this.SpeedUpgrade = properties.SpeedUpgrade;
      this.FireRate = TimeSpan.FromMilliseconds(this.FireRate.TotalMilliseconds * Math.Pow(0.8, (double) this.SpeedUpgrade));
      this.DiameterUpgrade = properties.DiameterUpgrade;
      this.Diameter *= (float) Math.Pow(1.2, (double) this.DiameterUpgrade);
      this.ProjectileUpgrade = properties.ProjectileUpgrade;
      this.ProjectileInfo.Damage *= (int) Math.Pow(2.0, (double) this.ProjectileUpgrade);
      this.ProjectileInfo.MaxHitCount += this.ProjectileUpgrade;
      this.FindIndex = properties.FindIndex;
    }
  }
}
