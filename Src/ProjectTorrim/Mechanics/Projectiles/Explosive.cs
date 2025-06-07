
// Type: BrawlerSource.Mechanics.Projectiles.Explosive
// Assembly: ProjectTorrim, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC8AE86A-8582-48C1-AE0D-A25C0B7D84A4
//

using BrawlerSource.Audio;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

#nullable disable
namespace BrawlerSource.Mechanics.Projectiles
{
  public class Explosive : Projectile
  {
    protected Explosion myExplosion;
    protected List<Sound> myExplosionSounds;

    public Explosive(GameObject parent, ProjectileInfo info)
      : base(parent, info)
    {
    }

    public Explosive(Layer layer, ProjectileInfo info)
      : base(layer, info)
    {
    }

    protected override void Construct(ProjectileInfo info)
    {
      base.Construct(info);
      this.myExplosionSounds = new List<Sound>();
      List<Sound> explosionSounds1 = this.myExplosionSounds;
      Sound sound1 = new Sound((GameObject) this, this.Game.Effects);
      sound1.AudioPath = "Sound\\Explosion_A";
      explosionSounds1.Add(sound1);
      List<Sound> explosionSounds2 = this.myExplosionSounds;
      Sound sound2 = new Sound((GameObject) this, this.Game.Effects);
      sound2.AudioPath = "Sound\\Explosion_B";
      explosionSounds2.Add(sound2);
      this.myExplosion = new Explosion(this.Parent, new ProjectileInfo()
      {
        Damage = this.Info.Damage,
        Distance = 4f,
        MaxHitCount = 100,
        Diameter = 256,
        InitialSpeed = 0
      });
      this.Parent.SubGameObjects.Remove((GameObject) this.myExplosion);
    }

    public override void LoadContent()
    {
      base.LoadContent();
      this.myExplosion.LoadContent();
    }

    public override void ApplyDamage(Enemy e, GameTime gameTime)
    {
      if (this.myExplosion.myPoint.myCollider.IsDisabled)
      {
        this.Parent.SubGameObjects.Add((GameObject) this.myExplosion);
        this.myExplosion.AddToDraw();
        this.myExplosion.Start(gameTime, e.Position, Vector2.One, 100f, (Enemy) null);
        this.myExplosionSounds[this.Random.Next(0, this.myExplosionSounds.Count)].Play();
      }
      this.Kill();
    }
  }
}
