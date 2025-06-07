
// Type: BrawlerSource.Mechanics.Enemies.Goat
// Assembly: ProjectTorrim, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC8AE86A-8582-48C1-AE0D-A25C0B7D84A4
//

using BrawlerSource.Audio;
using BrawlerSource.Graphics;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

#nullable disable
namespace BrawlerSource.Mechanics.Enemies
{
  public class Goat : Enemy
  {
    public Goat(Layer layer, Position position, Queue<BrawlerSource.PathFinding.Node> path)
      : base(layer, position, 75, 90, path)
    {
    }

    public Goat(GameObject parent, Position position, Queue<BrawlerSource.PathFinding.Node> path)
      : base(parent, position, 75, 90, path)
    {
    }

    protected override void Construct(Position position, int health, int speed, Queue<BrawlerSource.PathFinding.Node> path)
    {
      base.Construct(position, health, speed, path);
      this.DamageResistence.Add(DamageType.Blunt);
      this.DamageWeakness.Add(DamageType.Slashing);
      List<Sound> hitSounds1 = this.myHitSounds;
      Sound sound1 = new Sound((GameObject) this, this.Game.Effects);
      sound1.AudioPath = "Sound\\Enemies\\Goat_Hit_A";
      sound1.Volume = 0.5f;
      hitSounds1.Add(sound1);
      List<Sound> hitSounds2 = this.myHitSounds;
      Sound sound2 = new Sound((GameObject) this, this.Game.Effects);
      sound2.AudioPath = "Sound\\Enemies\\Goat_Hit_B";
      sound2.Volume = 0.5f;
      hitSounds2.Add(sound2);
      List<Sound> hitSounds3 = this.myHitSounds;
      Sound sound3 = new Sound((GameObject) this, this.Game.Effects);
      sound3.AudioPath = "Sound\\Enemies\\Goat_Hit_C";
      sound3.Volume = 0.5f;
      hitSounds3.Add(sound3);
      this.mySequences.AddSequence(Directions.Front, new Sequence()
      {
        TexturePath = "Enemies\\Goat",
        Width = 32,
        Height = 32,
        Looping = true,
        InitialImageIndex = 0,
        ImageTotal = 4,
        FrameSpeed = 4
      });
      this.mySequences.AddSequence(Directions.Back, new Sequence()
      {
        TexturePath = "Enemies\\Goat",
        Width = 32,
        Height = 32,
        Looping = true,
        InitialImageIndex = 4,
        ImageTotal = 4,
        FrameSpeed = 4
      });
      this.mySequences.AddSequence(Directions.Right, new Sequence()
      {
        TexturePath = "Enemies\\Goat",
        Width = 32,
        Height = 32,
        Looping = true,
        InitialImageIndex = 8,
        ImageTotal = 4,
        FrameSpeed = 4
      });
      this.mySequences.AddSequence(Directions.Left, new Sequence()
      {
        TexturePath = "Enemies\\Goat",
        Width = 32,
        Height = 32,
        Looping = true,
        InitialImageIndex = 8,
        ImageTotal = 4,
        FrameSpeed = 4,
        Effect = (SpriteEffects) 1
      });
      this.Depth = 0.71f;
      Sprite sprite = new Sprite((GameObject) this);
      sprite.Position = this.Position;
      sprite.Depth = this.Depth;
      this.mySprite = sprite;
      this.mySequences.SetSprite(this.mySprite);
    }
  }
}
