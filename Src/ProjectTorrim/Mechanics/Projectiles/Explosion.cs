
// Type: BrawlerSource.Mechanics.Projectiles.Explosion
// Assembly: ProjectTorrim, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC8AE86A-8582-48C1-AE0D-A25C0B7D84A4
//

using BrawlerSource.Audio;
using BrawlerSource.Graphics;
using BrawlerSource.Particles;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

#nullable disable
namespace BrawlerSource.Mechanics.Projectiles
{
  public class Explosion : Projectile
  {
    protected List<Sound> myExplosionSounds;
    private Emitter myFireParticles;

    public Explosion(GameObject parent, ProjectileInfo info)
      : base(parent, info)
    {
    }

    public Explosion(Layer layer, ProjectileInfo info)
      : base(layer, info)
    {
    }

    protected override void Construct(ProjectileInfo info)
    {
      base.Construct(info);
      this.mySprite.Sequence = new Sequence()
      {
        TexturePath = nameof (Explosion),
        Width = 256,
        Height = 256,
        ImageTotal = 8,
        FrameSpeed = 16,
        Looping = true,
        SequenceEnd = new EndFunction(this.Finish)
      };
      this.mySprite.Scale = new Vector2(1f);
      this.mySprite.Position = new Position(0.0f, 0.0f);
      this.mySprite.Depth = 1f;
      this.mySprite.ImageSpeed = 0.0;
      this.mySprite.Colour = Color.White;
      this.myEmitter.QueueDispose();
      GameObject parent = this.Parent;
      Position position = this.Position + new Position(0.0f, 8f);
      List<string> texturePaths = new List<string>();
      texturePaths.Add("Square_256");
      ParticleDefinition definition = new ParticleDefinition()
      {
        VelocityMin = new Position(-256f, -256f),
        VelocityMax = new Position(256f, 256f),
        RotationMin = 0.0f,
        ScaleMin = Vector2.Divide(new Vector2(4f), 256f),
        ScaleVelocityMin = new Vector2(0.0f),
        LifespanMin = TimeSpan.FromMilliseconds(400.0),
        Colour = new Color(0.25f, 0.25f, 0.25f, 0.5f),
        ColourChange = new Color(0.0f, 0.0f, 0.0f, 0.5f)
      };
      TimeSpan? lifespan = new TimeSpan?(TimeSpan.FromMilliseconds(200.0));
      Emitter emitter = new Emitter(parent, position, texturePaths, definition, 50, lifespan);
      emitter.Depth = 0.9f;
      this.myFireParticles = emitter;
      this.myPoint.myCollider.IsDisabled = true;
    }

    public override void LoadContent()
    {
      base.LoadContent();
      this.myFireParticles.LoadContent();
    }

    public override void Start(
      GameTime gameTime,
      Position position,
      Vector2 velocity,
      float distance,
      Enemy target = null)
    {
      base.Start(gameTime, position, velocity, distance, target);
      this.myFireParticles.Position = position;
      this.mySprite.ImageSpeed = 1.0;
    }

    public void Finish()
    {
      this.Parent.SubGameObjects.Remove((GameObject) this);
      this.RemoveFromDraw();
      this.myPoint.myCollider.IsDisabled = true;
      this.myDamagedEnemies.Clear();
      this.Target = (Enemy) null;
      this.mySprite.ImageSpeed = 0.0;
      this.mySprite.ResetImageIndex();
    }

    public override void ApplyDamage(Enemy e, GameTime gameTime)
    {
      this.ApplyDamage((float) (1.0 - this.Position.DistanceTo(e.Position) / (double) (this.Info.Diameter / 2)), e, gameTime);
    }
  }
}
