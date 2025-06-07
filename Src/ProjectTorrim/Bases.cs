
// Type: BrawlerSource.Bases
// Assembly: ProjectTorrim, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC8AE86A-8582-48C1-AE0D-A25C0B7D84A4
//

using BrawlerSource.Audio;
using BrawlerSource.Collision;
using BrawlerSource.Collision.Intersections;
using BrawlerSource.GameInfo;
using BrawlerSource.Graphics;
using BrawlerSource.Particles;
using BrawlerSource.UI;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

#nullable disable
namespace BrawlerSource
{
  public class Bases : DrawableGameObject
  {
    protected List<Sound> myDamageSounds;
    protected Emitter myFireParticles;
    protected List<SequenceManager<BaseSpriteSequence>> myBaseSequences;
    protected Queue<int> myFallingSequences;
    protected int myLastBaseIndex;
    protected PercentageBar myHealthbar;
    protected int myHealth;
    protected int myMaxHealth = 1000;
    protected Collider myCollider;
    public int ID;

    public Bases(Layer layer, Position position, Position dimensions, float depth)
      : base(layer)
    {
      if (this.Layer is GameLayer)
        this.ID = ((GameLayer) this.Layer).basesID++;
      this.Position = position;
      this.myHealth = this.myMaxHealth;
      this.Depth = depth;
      PercentageBar percentageBar = new PercentageBar((GameObject) this, this.Position + new Position(0.0f, -80f), new Position(80f, 8f), this.myMaxHealth, Align.Left);
      percentageBar.Depth = this.Depth;
      this.myHealthbar = percentageBar;
      this.myHealthbar.Value = this.myMaxHealth;
      this.myCollider = new Collider((GameObject) this)
      {
        Position = this.Position
      };
      this.myCollider.AddIntersection((IIntersectionable) new Rectangular()
      {
        Dimensions = new Position(32f, 32f)
      });
      this.myCollider.AddEvent(typeof (Enemy), TouchType.TouchStart, new CollisionFunc(this.Damage));
      this.myDamageSounds = new List<Sound>();
      List<Sound> damageSounds1 = this.myDamageSounds;
      Sound sound1 = new Sound((GameObject) this, this.Game.Effects);
      sound1.AudioPath = "Sound\\Tent_A";
      damageSounds1.Add(sound1);
      List<Sound> damageSounds2 = this.myDamageSounds;
      Sound sound2 = new Sound((GameObject) this, this.Game.Effects);
      sound2.AudioPath = "Sound\\Tent_B";
      damageSounds2.Add(sound2);
      List<Sound> damageSounds3 = this.myDamageSounds;
      Sound sound3 = new Sound((GameObject) this, this.Game.Effects);
      sound3.AudioPath = "Sound\\Tent_C";
      damageSounds3.Add(sound3);
      this.myFallingSequences = new Queue<int>();
      this.myBaseSequences = new List<SequenceManager<BaseSpriteSequence>>();
      SequenceManager<BaseSpriteSequence> sequenceManager1 = new SequenceManager<BaseSpriteSequence>((GameObject) this);
      sequenceManager1.AddSequence(BaseSpriteSequence.Standing, new Sequence()
      {
        TexturePath = "Campfire",
        Width = 32,
        Height = 32,
        ImageTotal = 5,
        FrameSpeed = 10,
        Looping = true
      });
      sequenceManager1.AddSequence(BaseSpriteSequence.Falling, new Sequence()
      {
        TexturePath = "Campfire",
        Width = 32,
        Height = 32,
        ImageTotal = 5,
        FrameSpeed = 10,
        SequenceEnd = new EndFunction(this.ExtinguishFire)
      });
      sequenceManager1.AddSequence(BaseSpriteSequence.Down, new Sequence()
      {
        TexturePath = "Campfire",
        Width = 32,
        Height = 32,
        InitialImageIndex = 5
      });
      SequenceManager<BaseSpriteSequence> sequenceManager2 = sequenceManager1;
      Sprite sprite = new Sprite((GameObject) this);
      sprite.Position = this.Position;
      sprite.Depth = this.Depth;
      sequenceManager2.SetSprite(sprite);
      this.myBaseSequences.Add(sequenceManager1);
      Position position1 = this.Position + new Position(0.0f, 8f);
      List<string> texturePaths = new List<string>();
      texturePaths.Add("Square_256");
      ParticleDefinition definition = new ParticleDefinition()
      {
        VelocityMin = new Position(-12f, -32f),
        VelocityMax = new Position(12f, 0.0f),
        RotationMin = 0.0f,
        ScaleMin = Vector2.Divide(new Vector2(4f), 256f),
        ScaleVelocityMin = new Vector2(0.0f),
        LifespanMin = TimeSpan.FromMilliseconds(1000.0),
        Colour = new Color(0.25f, 0.25f, 0.25f, 0.5f),
        ColourChange = new Color(0.0f, 0.0f, 0.0f, 0.5f)
      };
      TimeSpan? lifespan = new TimeSpan?();
      Emitter emitter = new Emitter((GameObject) this, position1, texturePaths, definition, 50, lifespan);
      emitter.Depth = this.Depth - 0.1f;
      this.myFireParticles = emitter;
    }

    public void HideHealthBar() => this.myHealthbar.IsHidden = true;

    public void Fall()
    {
      this.myHealth = 0;
      this.ApplyDamage();
    }

    protected virtual void Damage(object sender, CollisionEventArgs e)
    {
      Enemy trigger = (Enemy) e.Trigger;
      this.myHealth -= trigger.Health;
      this.myHealthbar.Value = this.myHealth;
      this.myDamageSounds[this.Random.Next(0, this.myDamageSounds.Count)].Play();
      trigger.QueueDispose();
      this.ApplyDamage();
    }

    private void ApplyDamage()
    {
      if (this.myHealth <= 0)
      {
        this.myCollider.QueueDispose();
        this.myBaseSequences[0].PlaySequence(BaseSpriteSequence.Falling);
        this.myFallingSequences.Enqueue(0);
      }
      int num = MathHelper.Clamp(this.myHealth / (this.myMaxHealth / this.myBaseSequences.Count), 0, this.myBaseSequences.Count);
      if (this.myLastBaseIndex != num && this.myLastBaseIndex >= 0)
      {
        for (int index = num + 1; index <= this.myLastBaseIndex; ++index)
        {
          this.myBaseSequences[index].PlaySequence(BaseSpriteSequence.Falling);
          this.myFallingSequences.Enqueue(index);
        }
      }
      this.myLastBaseIndex = num < this.myBaseSequences.Count ? num : this.myBaseSequences.Count - 1;
    }

    public void ExtinguishFire()
    {
      this.FallFinish();
      this.myFireParticles.Lifespan = new TimeSpan?(TimeSpan.FromSeconds(0.0));
    }

    public void FallFinish()
    {
      this.myBaseSequences[this.myFallingSequences.Dequeue()].SetSequence(BaseSpriteSequence.Down);
    }

    public BasesProperties GetProperties()
    {
      return new BasesProperties()
      {
        Health = this.myHealth,
        ID = this.ID
      };
    }

    public void SetProperties(BasesProperties properties)
    {
      this.myHealth = properties.Health;
      this.myHealthbar.Value = this.myHealth;
      this.ApplyDamage();
    }
  }
}
