
// Type: BrawlerSource.Enemy
// Assembly: ProjectTorrim, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC8AE86A-8582-48C1-AE0D-A25C0B7D84A4
//

using BrawlerSource.Audio;
using BrawlerSource.Collision;
using BrawlerSource.Collision.Intersections;
using BrawlerSource.Graphics;
using BrawlerSource.Mechanics.Towers;
using BrawlerSource.PathFinding;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

#nullable disable
namespace BrawlerSource
{
  public class Enemy : DrawableGameObject
  {
    protected List<Sound> myHitSounds;
    public EventHandler OnDisposal;
    public HashSet<DamageType> DamageWeakness;
    public HashSet<DamageType> DamageResistence;
    public Sprite mySprite;
    public SequenceManager<Directions> mySequences;
    public Navigator myNavigator;
    private Collider myCollider;
    private Rectangular myRectangle;
    public int Health;
    public int Reward;
    public HashSet<Freezer> Freezers;
    public TimeSpan LastKnockbackTime;
    public TimeSpan KnockbackRate;
    private int mySpeed;

    public int InitialSpeed { get; private set; }

    public int Speed
    {
      get => this.mySpeed;
      set
      {
        this.mySpeed = value;
        this.myNavigator.Speed = (float) this.mySpeed;
      }
    }

    public Enemy(Layer layer, Position position, int health, int speed, Queue<BrawlerSource.PathFinding.Node> path)
      : base(layer)
    {
      this.Construct(position, health, speed, path);
    }

    public Enemy(GameObject parent, Position position, int health, int speed, Queue<BrawlerSource.PathFinding.Node> path)
      : base(parent)
    {
      this.Construct(position, health, speed, path);
    }

    protected virtual void Construct(Position position, int health, int speed, 
        Queue<BrawlerSource.PathFinding.Node> path)
    {
      this.Position = position;
      this.Health = health;
      this.Reward = (int) ((double) this.Health * 0.5);
      this.KnockbackRate = TimeSpan.FromMilliseconds(500.0);
      this.LastKnockbackTime = TimeSpan.Zero;
      this.Depth = 0.7f;
      this.DamageResistence = new HashSet<DamageType>();
      this.DamageWeakness = new HashSet<DamageType>();
      this.myHitSounds = new List<Sound>();
      this.myCollider = new Collider((GameObject) this)
      {
        Position = this.Position
      };
      this.myRectangle = new Rectangular()
      {
        Dimensions = new Position(32f)
      };
      this.myCollider.AddIntersection((IIntersectionable) this.myRectangle);
      this.myCollider.AddEvent(typeof (Projectile), TouchType.TouchStart, new CollisionFunc(this.Damage));
      this.myNavigator = new Navigator((GameObject) this, this.Position, (IIntersectionable) new Rectangular()
      {
        Dimensions = new Position(32f)
      })
      {
        Path = path
      };
      this.Freezers = new HashSet<Freezer>();
      this.Speed = speed;
      this.InitialSpeed = this.Speed;
      this.mySequences = new SequenceManager<Directions>((GameObject) this);
    }

    public override void FirstUpdate(GameTime gameTime)
    {
      this.myRectangle.Dimensions = new Position((float) this.mySprite.Sequence.Width, (float) this.mySprite.Sequence.Height);
      base.FirstUpdate(gameTime);
    }

    public void Damage(object sender, CollisionEventArgs e)
    {
      ((Projectile) e.Trigger).ApplyDamage(this, e.GameTime);
      this.myHitSounds[this.Random.Next(0, this.myHitSounds.Count)].Play();
    }

    public override void Update(GameTime gameTime)
    {
      this.Speed = this.Freezers.Count > 0 ? (int) ((double) this.InitialSpeed * 0.5)
                : (int) (double) this.InitialSpeed;
      this.Position = this.myNavigator.Position - new Position(0.0f,
          (float) (this.mySprite.Sequence.Height / 2));
      this.mySprite.Position = this.Position;
      this.myCollider.Position = this.Position;
      Vector2 velocity = this.myNavigator.Velocity;
      this.mySequences.SetSequence((double) Math.Abs(velocity.Y) > (double) Math.Abs(velocity.X)
          ? ((double) velocity.Y > 0.0 
          ? Directions.Front : Directions.Back)
          : ((double) velocity.X > 0.0 ? Directions.Right : Directions.Left));
      this.mySprite.ImageSpeed = (double) velocity.LengthSquared() > 0.0 ? 1.0 : 0.0;
      base.Update(gameTime);
    }

    public override void QueueDispose()
    {
      EventHandler onDisposal = this.OnDisposal;
      if (onDisposal != null)
        onDisposal((object) this, new EventArgs());
      base.QueueDispose();
    }
  }
}
