
// Type: BrawlerSource.Mechanics.Towers.Exploder
// Assembly: ProjectTorrim, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC8AE86A-8582-48C1-AE0D-A25C0B7D84A4
//

using BrawlerSource.Audio;
using BrawlerSource.Graphics;
using BrawlerSource.Mechanics.Projectiles;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

#nullable disable
namespace BrawlerSource.Mechanics.Towers
{
  public class Exploder : Tower
  {
    public Exploder(Layer layer, Position position)
      : base(layer, position, 250f)
    {
    }

    public Exploder(GameObject parent, Position position)
      : base(parent, position, 250f)
    {
    }

    protected override void Construct(Position position, float diameter)
    {
      base.Construct(position, diameter);
      this.FireRate = TimeSpan.FromMilliseconds(1000.0);
      this.Cost = 48975;
      this.Description = "Fires an explosive projectile";
      this.Name = nameof (Exploder);
      this.ProjectileInfo.Type = typeof (Explosive);
      this.ProjectileInfo.Damage = 500;
      this.ProjectileInfo.DamageType = DamageType.Piercing;
      this.ProjectileInfo.Knockback = 4;
      this.ProjectileInfo.MaxHitCount = 10;
      List<Sound> fireSounds1 = this.myFireSounds;
      Sound sound1 = new Sound((GameObject) this, this.Game.Effects);
      sound1.AudioPath = "Sound\\Throw_2A";
      fireSounds1.Add(sound1);
      List<Sound> fireSounds2 = this.myFireSounds;
      Sound sound2 = new Sound((GameObject) this, this.Game.Effects);
      sound2.AudioPath = "Sound\\Throw_2B";
      fireSounds2.Add(sound2);
      List<Sound> fireSounds3 = this.myFireSounds;
      Sound sound3 = new Sound((GameObject) this, this.Game.Effects);
      sound3.AudioPath = "Sound\\Throw_2C";
      fireSounds3.Add(sound3);
      List<Sound> fireSounds4 = this.myFireSounds;
      Sound sound4 = new Sound((GameObject) this, this.Game.Effects);
      sound4.AudioPath = "Sound\\Throw_2D";
      fireSounds4.Add(sound4);
      this.mySequences.AddSequence(Directions.Front, new Sequence()
      {
        TexturePath = "Friar_2",
        Width = 32,
        Height = 32,
        InitialImageIndex = 0
      });
      this.mySequences.AddSequence(Directions.Back, new Sequence()
      {
        TexturePath = "Friar_2",
        Width = 32,
        Height = 32,
        InitialImageIndex = 1
      });
      this.mySequences.AddSequence(Directions.Right, new Sequence()
      {
        TexturePath = "Friar_2",
        Width = 32,
        Height = 32,
        InitialImageIndex = 2
      });
      this.mySequences.AddSequence(Directions.Left, new Sequence()
      {
        TexturePath = "Friar_2",
        Width = 32,
        Height = 32,
        InitialImageIndex = 2,
        Effect = (SpriteEffects) 1
      });
      this.mySequences.SetSprite(this.mySprite);
    }
  }
}
