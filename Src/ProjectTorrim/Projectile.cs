
// Type: BrawlerSource.Projectile
// Assembly: ProjectTorrim, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC8AE86A-8582-48C1-AE0D-A25C0B7D84A4
//

using BrawlerSource.Collision.Intersections;
using BrawlerSource.GameInfo;
using BrawlerSource.Graphics;
using BrawlerSource.Particles;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

#nullable disable
namespace BrawlerSource
{
  public class Projectile : DrawableGameObject
  {
    public Sprite mySprite;
    public BrawlerSource.Physics.Point myPoint;
    protected HashSet<Enemy> myDamagedEnemies;
    protected TimeSpan myEndTime;
    protected TimeSpan myEndSpan;
    protected Enemy Target;
    protected Emitter myEmitter;
    private Dictionary<DamageType, Color> myDamageColours = new Dictionary<DamageType, Color>()
    {
      {
        DamageType.Piercing,
        Color.LightPink
      },
      {
        DamageType.Slashing,
        Color.PaleGreen
      },
      {
        DamageType.Blunt,
        Color.PaleTurquoise
      }
    };
    public ProjectileInfo Info;

    public Projectile(GameObject parent, ProjectileInfo info)
      : base(parent)
    {
      this.Construct(info);
    }

    public Projectile(Layer layer, ProjectileInfo info)
      : base(layer)
    {
      this.Construct(info);
    }

    protected virtual void Construct(ProjectileInfo info)
    {
      this.Position = new Position(0.0f);
      this.Info = info;
      this.myDamagedEnemies = new HashSet<Enemy>();
      this.myPoint = new BrawlerSource.Physics.Point((GameObject) this, this.Position, (IIntersectionable) new Circular()
      {
        Radius = (float) (this.Info.Diameter / 2)
      });
      Sprite sprite = new Sprite((GameObject) this);
      sprite.Sequence = new Sequence()
      {
        TexturePath = "Circle_256",
        Width = 256,
        Height = 256
      };
      sprite.Position = this.Position;
      sprite.Scale = Vector2.Divide(new Vector2((float) this.Info.Diameter), 256f);
      sprite.Depth = 1f;
      sprite.Colour = this.myDamageColours[this.Info.DamageType];
      this.mySprite = sprite;
      Position position = this.Position;
      List<string> texturePaths = new List<string>();
      texturePaths.Add("Circle_32");
      ParticleDefinition definition = new ParticleDefinition()
      {
        VelocityMin = new Position(0.0f),
        RotationMin = 0.0f,
        ScaleMin = Vector2.Divide(new Vector2((float) this.Info.Diameter), 32f),
        ScaleVelocityMin = new Vector2((float) -((double) this.Info.Diameter / 10.0)),
        LifespanMin = TimeSpan.FromMilliseconds(100.0),
        Colour = this.myDamageColours[this.Info.DamageType],
        ColourChange = new Color(Color.Gray, 0.05f)
      };
      TimeSpan? lifespan = new TimeSpan?();
      Emitter emitter = new Emitter((GameObject) this, position, texturePaths, definition, 200, lifespan);
      emitter.Depth = 0.9f;
      this.myEmitter = emitter;
    }

    public virtual void Start(
      GameTime gameTime,
      Position position,
      Vector2 velocity,
      float distance,
      Enemy target = null)
    {
      this.myPoint.SetCurrent(position, velocity);
      this.myPoint.myCollider.IsDisabled = false;
      this.Position = position;
      if ((double) this.Info.Distance != (double) distance || this.myEndSpan == TimeSpan.Zero)
      {
        this.Info.Distance = distance;
        this.myEndSpan = TimeSpan.FromMilliseconds(1.0 + (double) distance / 
            (double) velocity.Length() * 1000.0);
      }
      this.myEndTime = gameTime.TotalGameTime + this.myEndSpan;
      this.Target = target;
    }

    public override void Update(GameTime gameTime)
    {
      if (gameTime.TotalGameTime > this.myEndTime)
        this.DistanceReached();
      base.Update(gameTime);
      this.Position = this.myPoint.Position;
      this.mySprite.Position = this.Position;
      this.myEmitter.Position = this.Position;
    }

    public virtual void DistanceReached() => this.Kill();

    public virtual void Kill()
    {
      TowerBase parent = (TowerBase) this.Parent;
      parent.RemoveProjectile(this);
      this.myPoint.myCollider.IsDisabled = true;
      this.myDamagedEnemies.Clear();
      this.Target = (Enemy) null;
      this.myEmitter.Position = parent.Position;
    }

    public virtual void ApplyDamage(Enemy e, GameTime gameTime)
    {
      this.ApplyDamage(1f, e, gameTime);
    }

    protected void ApplyDamage(float damageMultiplier, Enemy e, GameTime gameTime)
    {
      if (this.myDamagedEnemies.Contains(e))
        return;
      float num = e.DamageResistence.Contains(this.Info.DamageType) ? 0.5f : (e.DamageWeakness.Contains(this.Info.DamageType) ? 2f : 1f);
      e.Health -= (int) ((double) this.Info.Damage * (double) num * (double) damageMultiplier);
      if (e.Health <= 0)
      {
        ((GameLevel) this.Level).Score += e.Reward;
        e.QueueDispose();
      }
      if (gameTime.TotalGameTime - e.LastKnockbackTime > e.KnockbackRate)
      {
        Vector2 vector2 = -e.myNavigator.Velocity.NormalizeToZero();
       e.myNavigator.Position = e.myNavigator.Position + Vector2.Multiply(vector2, (float) this.Info.Knockback);
        e.LastKnockbackTime = gameTime.TotalGameTime;
      }
      this.myDamagedEnemies.Add(e);
      if (this.myDamagedEnemies.Count < this.Info.MaxHitCount)
        return;
      this.Kill();
    }
  }
}
