
// Type: BrawlerSource.Mechanics.Towers.Chainer
// Assembly: ProjectTorrim, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC8AE86A-8582-48C1-AE0D-A25C0B7D84A4
//

using BrawlerSource.Audio;
using BrawlerSource.Graphics;
using BrawlerSource.Mechanics.Projectiles;
using System;
using System.Collections.Generic;

#nullable disable
namespace BrawlerSource.Mechanics.Towers
{
  public class Chainer : Tower
  {
    public Chainer(Layer layer, Position position)
      : base(layer, position, 250f)
    {
    }

    public Chainer(GameObject parent, Position position)
      : base(parent, position, 250f)
    {
    }

    protected override void Construct(Position position, float diameter)
    {
      base.Construct(position, diameter);
      this.FireRate = TimeSpan.FromMilliseconds(500.0);
      this.Cost = 445875;
      this.Description = "Fires a chain projectile";
      this.Name = nameof (Chainer);
      this.ProjectileInfo.Type = typeof (Chain);
      this.ProjectileInfo.InitialSpeed = 600;
      this.ProjectileInfo.Damage = 750;
      this.ProjectileInfo.DamageType = DamageType.Piercing;
      this.ProjectileInfo.MaxHitCount = 3;
      List<Sound> fireSounds1 = this.myFireSounds;
      Sound sound1 = new Sound((GameObject) this, this.Game.Effects);
      sound1.AudioPath = "Sound\\Spell_A1";
      fireSounds1.Add(sound1);
      List<Sound> fireSounds2 = this.myFireSounds;
      Sound sound2 = new Sound((GameObject) this, this.Game.Effects);
      sound2.AudioPath = "Sound\\Spell_A2";
      fireSounds2.Add(sound2);
      this.mySequences.AddSequence(Directions.Front, new Sequence()
      {
        TexturePath = "Monk_2",
        Width = 32,
        Height = 32,
        InitialImageIndex = 0
      });
      this.mySequences.AddSequence(Directions.Back, new Sequence()
      {
        TexturePath = "Monk_2",
        Width = 32,
        Height = 32,
        InitialImageIndex = 1
      });
      this.mySequences.AddSequence(Directions.Right, new Sequence()
      {
        TexturePath = "Monk_2",
        Width = 32,
        Height = 32,
        InitialImageIndex = 2
      });
      this.mySequences.AddSequence(Directions.Left, new Sequence()
      {
        TexturePath = "Monk_2",
        Width = 32,
        Height = 32,
        InitialImageIndex = 3
      });
      this.mySequences.SetSprite(this.mySprite);
    }
  }
}
