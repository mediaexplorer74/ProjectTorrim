
// Type: BrawlerSource.Mechanics.Towers.ChainPoint
// Assembly: ProjectTorrim, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC8AE86A-8582-48C1-AE0D-A25C0B7D84A4
//

using BrawlerSource.Audio;
using BrawlerSource.Mechanics.Projectiles;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

#nullable disable
namespace BrawlerSource.Mechanics.Towers
{
  public class ChainPoint : TowerBase
  {
    public Chain Chain;
    public float Speed;

    public ChainPoint(Layer layer, Position position)
      : base(layer, position, 250f)
    {
    }

    public ChainPoint(GameObject parent, Position position)
      : base(parent, position, 250f)
    {
    }

    protected override void Construct(Position position, float diameter)
    {
      base.Construct(position, diameter);
      this.ProjectileInfo.Type = typeof (Chain);
      this.ProjectileInfo.InitialSpeed = 600;
      this.ProjectileInfo.Damage = 75;
      this.ProjectileInfo.DamageType = DamageType.Piercing;
      this.ProjectileInfo.MaxHitCount = 3;
      this.FireRate = TimeSpan.FromDays(1.0);
      List<Sound> fireSounds1 = this.myFireSounds;
      Sound sound1 = new Sound((GameObject) this, this.Game.Effects);
      sound1.AudioPath = "Sound\\Spell_A1";
      fireSounds1.Add(sound1);
      List<Sound> fireSounds2 = this.myFireSounds;
      Sound sound2 = new Sound((GameObject) this, this.Game.Effects);
      sound2.AudioPath = "Sound\\Spell_A2";
      fireSounds2.Add(sound2);
    }

    public override void RemoveProjectile(Projectile p)
    {
    }

    public override void Shoot(GameTime gameTime)
    {
      if (this.Enemies.Count > 0)
      {
        Vector2 vector2 = (this.FindFunctions[4].Item1().Position - this.Position)
                    .ToVector2().NormalizeToZero();

        if (Vector2.Equals(vector2, Vector2.Zero))
          vector2 = Vector2.One;
        this.Chain.Start(gameTime, this.Position, Vector2.Multiply(vector2, 
            (float) this.ProjectileInfo.InitialSpeed), this.ProjectileInfo.Distance);

        this.myFireSounds[this.Random.Next(0, this.myFireSounds.Count)].Play();
      }
      else
        this.Chain.Kill();
      this.FireRate = TimeSpan.FromDays(1.0);
    }
  }
}
