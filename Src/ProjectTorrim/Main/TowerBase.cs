
// Type: BrawlerSource.TowerBase
// Assembly: ProjectTorrim, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC8AE86A-8582-48C1-AE0D-A25C0B7D84A4
//

using BrawlerSource.Audio;
using BrawlerSource.Collision;
using BrawlerSource.Collision.Intersections;
using BrawlerSource.GameInfo;
using BrawlerSource.UI;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

#nullable disable
namespace BrawlerSource
{
  public class TowerBase : DrawableGameObject
  {
    protected List<Sound> myFireSounds;
    protected Position myWatchOffset;
    protected Collider myWatchCollider;
    protected Circular myWatchIntersection;
    public List<Enemy> Enemies;
    private int myFindIndex;
    public TimeSpan FireRate;
    public TimeSpan myLastFireTime;
    protected bool CanShoot;
    protected Queue<Projectile> myProjectilePool;
    public ProjectileInfo ProjectileInfo;
    private float myDiameter;

    public event EventHandler OnFindChanged;

    public int FindIndex
    {
      get => this.myFindIndex;
      set
      {
        this.myFindIndex = value % this.FindFunctions.Count;
        EventHandler onFindChanged = this.OnFindChanged;
        if (onFindChanged == null)
          return;
        onFindChanged((object) this, new EventArgs());
      }
    }

    public List<Tuple<FindEnemy, string>> FindFunctions { get; protected set; }

    public event EventHandler OnShooting;

    public float Diameter
    {
      get => this.myDiameter;
      set
      {
        if ((double) this.myDiameter == (double) value)
          return;
        this.myDiameter = value;
        EventHandler onDiameterChanged = this.OnDiameterChanged;
        if (onDiameterChanged == null)
          return;
        onDiameterChanged((object) this, (EventArgs) new ValueEventArgs()
        {
          Value = value
        });
      }
    }

    public event EventHandler OnDiameterChanged;

    public TowerBase(GameObject parent, Position position, float diameter)
      : base(parent)
    {
      this.Construct(position, diameter);
    }

    public TowerBase(Layer layer, Position position, float diameter)
      : base(layer)
    {
      this.Construct(position, diameter);
    }

    protected virtual void Construct(Position position, float diameter)
    {
      this.CanShoot = true;
      this.Position = position;
      this.Diameter = diameter;
      this.myProjectilePool = new Queue<Projectile>();
      this.ProjectileInfo = new ProjectileInfo()
      {
        Type = typeof (Projectile),
        InitialSpeed = 500,
        Damage = 10,
        DamageType = DamageType.Blunt,
        Knockback = 0,
        Distance = this.Diameter / 2f,
        MaxHitCount = 1,
        Diameter = 8
      };
      this.OnDiameterChanged += new EventHandler(this.Diameter_OnValueChanged);
      this.FindFunctions = new List<Tuple<FindEnemy, string>>()
      {
        new Tuple<FindEnemy, string>(new FindEnemy(this.FindKnownEnemy), "Known"),
        new Tuple<FindEnemy, string>(new FindEnemy(this.FindUnknownEnemy), "Unknown"),
        new Tuple<FindEnemy, string>(new FindEnemy(this.FindNearestEnemy), "Nearest"),
        new Tuple<FindEnemy, string>(new FindEnemy(this.FindFurthestEnemy), "Furthest"),
        new Tuple<FindEnemy, string>(new FindEnemy(this.FindRandomEnemy), "Random")
      };
      this.FindIndex = 0;
      this.Enemies = new List<Enemy>();
      this.myWatchOffset = new Position(0.0f);
      this.myWatchCollider = new Collider((GameObject) this)
      {
        Position = this.Position
      };
      Collider watchCollider = this.myWatchCollider;
      Circular circular1 = new Circular();
      circular1.Radius = this.Diameter / 2f;
      Circular circular2 = circular1;
      this.myWatchIntersection = circular1;
      Circular i = circular2;
      watchCollider.AddIntersection((IIntersectionable) i);
      this.myWatchCollider.AddEvent(typeof (Enemy), TouchType.TouchStart, new CollisionFunc(this.RegisterEnemy));
      this.myWatchCollider.AddEvent(typeof (Enemy), TouchType.TouchEnd, new CollisionFunc(this.UnregisterEnemy));
      this.myFireSounds = new List<Sound>();
      if (!(this.Layer is GameLayer))
        return;
      ((GameLayer) this.Layer).Spawner.OnWaveEnd += new EventHandler(this.Spawner_OnWaveEnd);
    }

    private void Spawner_OnWaveEnd(object sender, EventArgs e) => this.Enemies.Clear();

    private void Diameter_OnValueChanged(object sender, EventArgs e)
    {
      if (this.myWatchIntersection != null)
        this.myWatchIntersection.Radius = this.myDiameter / 2f;
      this.ProjectileInfo.Distance = this.myDiameter / 2f;
    }

    public virtual void RegisterEnemy(object sender, CollisionEventArgs e)
    {
      Enemy trigger = (Enemy) e.Trigger;
      trigger.OnDisposal += new EventHandler(this.OnEnemyDisposed);
      this.Enemies.Add(trigger);
    }

    private void OnEnemyDisposed(object sender, EventArgs e) => this.Enemies.Remove((Enemy) sender);

    public virtual void UnregisterEnemy(object sender, CollisionEventArgs e)
    {
      Enemy trigger = (Enemy) e.Trigger;
      trigger.OnDisposal -= new EventHandler(this.OnEnemyDisposed);
      this.Enemies.Remove(trigger);
    }

    public virtual void RemoveProjectile(Projectile p)
    {
      this.SubGameObjects.Remove((GameObject) p);
      this.myProjectilePool.Enqueue(p);
      p.RemoveFromDraw();
    }

    public virtual void Shoot(GameTime gameTime)
    {
      if (this.Enemies.Count <= 0)
        return;
      Enemy target = this.FindFunctions[this.FindIndex].Item1();
      if (target.Health < 0)
      {
        this.Enemies.Remove(target);
      }
      else
      {
        Vector2 zero = (target.myNavigator.NextPosition - this.Position).ToVector2().NormalizeToZero();
        Projectile projectile;
        if (this.myProjectilePool.Count > 0)
        {
          projectile = this.myProjectilePool.Dequeue();
          this.SubGameObjects.Add((GameObject) projectile);
        }
        else
        {
          Type type = this.ProjectileInfo.Type;
          if ((object) type == null)
            type = typeof (Projectile);
          projectile = (Projectile) Activator.CreateInstance(type, (object) this, (object) this.ProjectileInfo);
          projectile.LoadContent();
        }
        projectile.AddToDraw();
        projectile.Start(gameTime, this.Position, Vector2.Multiply(zero, (float) this.ProjectileInfo.InitialSpeed), this.ProjectileInfo.Distance, target);
        this.myFireSounds[this.Random.Next(0, this.myFireSounds.Count)].Play();
        EventHandler onShooting = this.OnShooting;
        if (onShooting == null)
          return;
        onShooting((object) this, (EventArgs) new ShootEventArgs()
        {
          Target = target,
          Direction = zero
        });
      }
    }

    public Enemy FindRandomEnemy() => this.Enemies[this.Random.Next(0, this.Enemies.Count)];

    public Enemy FindKnownEnemy() => this.Enemies[0];

    public Enemy FindUnknownEnemy() => this.Enemies[this.Enemies.Count - 1];

    public Enemy FindNearestEnemy() => this.FindBestEnemy(true);

    public Enemy FindFurthestEnemy() => this.FindBestEnemy(false);

    private Enemy FindBestEnemy(bool isNearest)
    {
      Enemy bestEnemy = this.Enemies[0];
      double num = bestEnemy.Position.DistanceToSquared(this.Position);
      for (int index = 1; index < this.Enemies.Count; ++index)
      {
        Enemy enemy = this.Enemies[index];
        double squared = enemy.Position.DistanceToSquared(this.Position);
        if (isNearest && squared < num || !isNearest && squared > num)
        {
          bestEnemy = enemy;
          num = squared;
        }
      }
      return bestEnemy;
    }

    public override void Update(GameTime gameTime)
    {
      this.myWatchCollider.Position = this.Position + this.myWatchOffset;
      if (this.CanShoot && gameTime.TotalGameTime - this.myLastFireTime > this.FireRate)
      {
        this.myLastFireTime = gameTime.TotalGameTime;
        this.Shoot(gameTime);
      }
      base.Update(gameTime);
    }
  }
}
