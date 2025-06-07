
// Type: BrawlerSource.Mechanics.Projectiles.Chain
// Assembly: ProjectTorrim, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC8AE86A-8582-48C1-AE0D-A25C0B7D84A4
//

using BrawlerSource.Audio;
using BrawlerSource.Mechanics.Towers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

#nullable disable
namespace BrawlerSource.Mechanics.Projectiles
{
  public class Chain : Projectile
  {
    protected List<Sound> myChainSounds;
    private ChainPoint myChainPoint;

    public Chain(GameObject parent, ProjectileInfo info)
      : base(parent, info)
    {
    }

    public Chain(Layer layer, ProjectileInfo info)
      : base(layer, info)
    {
    }

    protected override void Construct(ProjectileInfo info)
    {
      base.Construct(info);
      this.myChainSounds = new List<Sound>();
      List<Sound> chainSounds1 = this.myChainSounds;
      Sound sound1 = new Sound((GameObject) this, this.Game.Effects);
      sound1.AudioPath = "Sound\\Spell_B1";
      chainSounds1.Add(sound1);
      List<Sound> chainSounds2 = this.myChainSounds;
      Sound sound2 = new Sound((GameObject) this, this.Game.Effects);
      sound2.AudioPath = "Sound\\Spell_B2";
      chainSounds2.Add(sound2);
      this.myChainPoint = new ChainPoint((GameObject) this, this.Position)
      {
        Chain = this
      };
    }

    public override void ApplyDamage(Enemy e, GameTime gameTime)
    {
      base.ApplyDamage(e, gameTime);
      if (this.myChainPoint.Enemies.Contains(e))
        this.myChainPoint.Enemies.Remove(e);
      this.myPoint.Velocity = new Vector2(0.0f);
      this.myChainPoint.Position = new Position(e.Position.X, e.Position.Y);
      this.myChainPoint.FireRate = TimeSpan.FromMilliseconds(50.0);
      this.myChainPoint.myLastFireTime = gameTime.TotalGameTime;
      this.myChainPoint.Diameter = this.Info.Distance;
    }
  }
}
