
// Type: BrawlerSource.Mechanics.Towers.Freezer
// Assembly: ProjectTorrim, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC8AE86A-8582-48C1-AE0D-A25C0B7D84A4
//

using BrawlerSource.Audio;
using BrawlerSource.Collision;
using BrawlerSource.Graphics;
using BrawlerSource.Particles;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

#nullable disable
namespace BrawlerSource.Mechanics.Towers
{
  public class Freezer : Tower
  {
    private Sound myIceSound;
    private Emitter myEmitter;

    public Freezer(Layer layer, Position position)
      : base(layer, position, 200f)
    {
    }

    public Freezer(GameObject parent, Position position)
      : base(parent, position, 200f)
    {
    }

    protected override void Construct(Position position, float diameter)
    {
      base.Construct(position, diameter);
      this.FireRate = TimeSpan.FromMilliseconds(0.0);
      this.Cost = 4875;
      this.Description = "Slows enemies within range";
      this.Name = nameof (Freezer);
      this.ProjectileInfo.Damage = 0;
      this.MaxSpeedUpgrade = 0;
      this.MaxDiameterUpgrade = 3;
      this.MaxProjectileUpgrade = 0;
      Sound sound = new Sound((GameObject) this, this.Game.Effects);
      sound.AudioPath = "Sound\\Ice_A";
      this.myIceSound = sound;
      this.mySequences.AddSequence(Directions.Front, new Sequence()
      {
        TexturePath = "Ice Totem",
        Width = 32,
        Height = 32,
        InitialImageIndex = 0
      });
      this.mySequences.SetSprite(this.mySprite);
      Position position1 = this.Position;
      List<string> texturePaths = new List<string>();
      texturePaths.Add("Circle_32");
      ParticleDefinition definition = new ParticleDefinition()
      {
        VelocityMin = new Position((float) (-(double) this.Diameter / 2.0)),
        VelocityMax = new Position(this.Diameter / 2f),
        RotationMin = 0.0f,
        ScaleMin = new Vector2(0.25f),
        ScaleVelocityMin = new Vector2(-0.25f),
        LifespanMin = TimeSpan.FromMilliseconds(1000.0),
        Colour = Color.DarkCyan
      };
      TimeSpan? lifespan = new TimeSpan?();
      Emitter emitter = new Emitter((GameObject) this, position1, texturePaths, definition, 50, lifespan);
      emitter.Depth = 0.8f;
      this.myEmitter = emitter;
    }

    public override void FirstUpdate(GameTime gameTime)
    {
      base.FirstUpdate(gameTime);
      this.myIceSound.Play();
    }

    public override void Update(GameTime gameTime)
    {
      base.Update(gameTime);
      this.myEmitter.Position = this.Position;
    }

    public override void Shoot(GameTime gameTime)
    {
    }

    public override void RegisterEnemy(object sender, CollisionEventArgs e)
    {
      base.RegisterEnemy(sender, e);
      ((Enemy) e.Trigger).Freezers.Add(this);
    }

    public override void UnregisterEnemy(object sender, CollisionEventArgs e)
    {
      base.UnregisterEnemy(sender, e);
      ((Enemy) e.Trigger).Freezers.Remove(this);
    }
  }
}
