
// Type: BrawlerSource.Mechanics.Towers.Ballista
// Assembly: ProjectTorrim, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC8AE86A-8582-48C1-AE0D-A25C0B7D84A4
//

using BrawlerSource.Audio;
using BrawlerSource.Collision;
using BrawlerSource.Collision.Intersections;
using BrawlerSource.Graphics;
using BrawlerSource.Mechanics.Projectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

#nullable disable
namespace BrawlerSource.Mechanics.Towers
{
  public class Ballista : Tower
  {
    protected List<Sound> myReloadSounds;
    private Rectangular myWatchIntersection2;
    private Vector2 myDirection;

    public Ballista(Layer layer, Position position)
      : base(layer, position, 160f)
    {
    }

    public Ballista(GameObject parent, Position position)
      : base(parent, position, 160f)
    {
    }

    protected override void Construct(Position position, float diameter)
    {
      base.Construct(position, diameter);
      this.FireRate = TimeSpan.FromMilliseconds(1500.0);
      this.Cost = 1200;
      this.Description = "Fires a boomerang projectile\nin a straight line";
      this.Name = nameof (Ballista);
      this.ProjectileInfo.Type = typeof (Boomerang);
      this.ProjectileInfo.Damage = 25;
      this.ProjectileInfo.MaxHitCount = 5;
      this.ProjectileInfo.DamageType = DamageType.Slashing;
      this.myReloadSounds = new List<Sound>();
      List<Sound> reloadSounds1 = this.myReloadSounds;
      Sound sound1 = new Sound((GameObject) this, this.Game.Effects);
      sound1.AudioPath = "Sound\\Ratchet_2A";
      reloadSounds1.Add(sound1);
      List<Sound> reloadSounds2 = this.myReloadSounds;
      Sound sound2 = new Sound((GameObject) this, this.Game.Effects);
      sound2.AudioPath = "Sound\\Ratchet_2B";
      reloadSounds2.Add(sound2);
      List<Sound> fireSounds1 = this.myFireSounds;
      Sound sound3 = new Sound((GameObject) this, this.Game.Effects);
      sound3.AudioPath = "Sound\\Throw_1A";
      fireSounds1.Add(sound3);
      List<Sound> fireSounds2 = this.myFireSounds;
      Sound sound4 = new Sound((GameObject) this, this.Game.Effects);
      sound4.AudioPath = "Sound\\Throw_1B";
      fireSounds2.Add(sound4);
      List<Sound> fireSounds3 = this.myFireSounds;
      Sound sound5 = new Sound((GameObject) this, this.Game.Effects);
      sound5.AudioPath = "Sound\\Throw_1C";
      fireSounds3.Add(sound5);
      List<Sound> fireSounds4 = this.myFireSounds;
      Sound sound6 = new Sound((GameObject) this, this.Game.Effects);
      sound6.AudioPath = "Sound\\Throw_1D";
      fireSounds4.Add(sound6);
      this.FindFunctions = new List<Tuple<FindEnemy, string>>()
      {
        new Tuple<FindEnemy, string>((FindEnemy) null, "Down"),
        new Tuple<FindEnemy, string>((FindEnemy) null, "Left"),
        new Tuple<FindEnemy, string>((FindEnemy) null, "Up"),
        new Tuple<FindEnemy, string>((FindEnemy) null, "Right")
      };
      this.myWatchCollider.RemoveIntersection((IIntersectionable) this.myWatchIntersection);
      Collider watchCollider = this.myWatchCollider;
      Rectangular rectangular1 = new Rectangular();
      rectangular1.Width = (float) (int) this.Diameter;
      rectangular1.Height = 32f;
      Rectangular rectangular2 = rectangular1;
      this.myWatchIntersection2 = rectangular1;
      Rectangular i = rectangular2;
      watchCollider.AddIntersection((IIntersectionable) i);
      this.myDistanceSprite.Dispose();
      Sprite sprite = new Sprite((GameObject) this);
      sprite.Sequence = new Sequence()
      {
        TexturePath = "Square_256",
        Width = 256,
        Height = 256
      };
      sprite.Position = this.Position;
      sprite.Scale = Vector2.Divide(new Vector2(this.Diameter, 32f), 256f);
      sprite.Colour = new Color(Color.Yellow, 0.5f);
      sprite.Depth = 0.6f;
      sprite.IsHidden = true;
      this.myDistanceSprite = sprite;
      this.mySequences.AddSequence(Directions.Front, new Sequence()
      {
        TexturePath = nameof (Ballista),
        Width = 32,
        Height = 32,
        InitialImageIndex = 0
      });
      this.mySequences.AddSequence(Directions.Back, new Sequence()
      {
        TexturePath = nameof (Ballista),
        Width = 32,
        Height = 32,
        InitialImageIndex = 1
      });
      this.mySequences.AddSequence(Directions.Right, new Sequence()
      {
        TexturePath = nameof (Ballista),
        Width = 32,
        Height = 32,
        InitialImageIndex = 2
      });
      this.mySequences.AddSequence(Directions.Left, new Sequence()
      {
        TexturePath = nameof (Ballista),
        Width = 32,
        Height = 32,
        InitialImageIndex = 2,
        Effect = (SpriteEffects) 1
      });
      this.mySequences.SetSprite(this.mySprite);
      this.OnFindChanged += new EventHandler(this.Ballista_OnFindChanged);
      this.OnDiameterChanged += new EventHandler(this.Ballista_OnDiameterChanged);
      this.ResetDirection();
    }

    private void Ballista_OnDiameterChanged(object sender, EventArgs e) => this.ResetDirection();

    private void Ballista_OnFindChanged(object sender, EventArgs e) => this.ResetDirection();

    private void ResetDirection()
    {
      this.SetDirection();
      this.SetDirectionalSequence(this.myDirection);
    }

    public override void Shoot(GameTime gameTime)
    {
      if (this.Enemies.Count <= 0)
        return;
      Projectile projectile;
      if (this.myProjectilePool.Count > 0)
      {
        projectile = this.myProjectilePool.Dequeue();
        this.SubGameObjects.Add((GameObject) projectile);
      }
      else
      {
        Type type = this.ProjectileInfo.Type;
        if ((object) type == null)
          type = typeof (Projectile);
        projectile = (Projectile) Activator.CreateInstance(type, (object) this, (object) this.ProjectileInfo);
        projectile.LoadContent();
      }
      projectile.AddToDraw();
      projectile.Start(gameTime, this.Position, Vector2.Multiply(this.myDirection, (float) this.ProjectileInfo.InitialSpeed), this.ProjectileInfo.Distance * 2f);
      this.myFireSounds[this.Random.Next(0, this.myFireSounds.Count)].Play();
      this.myReloadSounds[this.Random.Next(0, this.myReloadSounds.Count)].Play();
    }

    public void SetDirection()
    {
      this.myDirection = !(this.FindFunctions[this.FindIndex].Item2 == "Up")
                ? (!(this.FindFunctions[this.FindIndex].Item2 == "Down") ? (!(this.FindFunctions[this.FindIndex].Item2 == "Left") ? new Vector2(1f, 0.0f) : new Vector2(-1f, 0.0f)) : new Vector2(0.0f, 1f)) : new Vector2(0.0f, -1f);
      Vector2 v = new Vector2((double) this.myDirection.X == 0.0 
          ? 32f : this.Diameter, (double) this.myDirection.Y == 0.0 ? 32f : this.Diameter);
      this.myDistanceSprite.Scale = Vector2.Divide(v, 256f);
      this.myWatchIntersection2.Dimensions = new Position(v);
      this.myWatchOffset = new Position(Vector2.Multiply(this.myDirection, this.Diameter / 2f));
      this.myDistanceSprite.Position = this.Position + this.myWatchOffset;
      this.myWatchCollider.Position = this.Position + this.myWatchOffset;
    }
  }
}
