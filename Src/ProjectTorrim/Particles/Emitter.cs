
// Type: BrawlerSource.Particles.Emitter
// Assembly: ProjectTorrim, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC8AE86A-8582-48C1-AE0D-A25C0B7D84A4
//

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

#nullable disable
namespace BrawlerSource.Particles
{
  public class Emitter : DrawableGameObject
  {
    private List<string> myTexturePaths;
    private List<Texture2D> myTextures;
    public int? Density;
    private int mySpawnRate;
    private float mySpawnSpeed;
    private double mySpawnRatePassed;
    public TimeSpan? Lifespan;
    private TimeSpan SpawnTime;
    private ParticleDefinition Definition;
    private Queue<Particle> myParticlePool;

    public Emitter(
      GameObject parent,
      Position position,
      List<string> texturePaths,
      ParticleDefinition definition,
      int spawnRate,
      TimeSpan? lifespan = null)
      : base(parent)
    {
      this.Construct(position, texturePaths, definition, spawnRate, lifespan);
    }

    public Emitter(
      Layer layer,
      Position position,
      List<string> texturePaths,
      ParticleDefinition definition,
      int spawnRate,
      TimeSpan? lifespan = null)
      : base(layer)
    {
      this.Construct(position, texturePaths, definition, spawnRate, lifespan);
    }

    private void Construct(
      Position position,
      List<string> texturePaths,
      ParticleDefinition definition,
      int spawnRate,
      TimeSpan? lifespan = null)
    {
      this.Position = position;
      this.myTexturePaths = texturePaths;
      this.Definition = definition;
      this.mySpawnRate = spawnRate;
      this.mySpawnRatePassed = 1.0;
      this.Lifespan = lifespan;
      this.myTextures = new List<Texture2D>();
      this.myParticlePool = new Queue<Particle>();
    }

    public void RemoveParticle(Particle p)
    {
      this.SubGameObjects.Remove((GameObject) p);
      this.myParticlePool.Enqueue(p);
    }

    public override void LoadContent()
    {
      base.LoadContent();
      foreach (string texturePath in this.myTexturePaths)
        this.myTextures.Add(this.Game.Content.Load<Texture2D>(texturePath));
    }

    public override void FirstUpdate(GameTime gameTime)
    {
      base.FirstUpdate(gameTime);
      this.SpawnTime = gameTime.TotalGameTime;
      this.mySpawnSpeed = 1f / (float) this.mySpawnRate;
    }

    public override void Update(GameTime gameTime)
    {
      base.Update(gameTime);
      if (this.Lifespan.HasValue)
      {
        TimeSpan timeSpan = gameTime.TotalGameTime - this.SpawnTime;
        TimeSpan? lifespan = this.Lifespan;
        if ((lifespan.HasValue ? (timeSpan > lifespan.GetValueOrDefault() ? 1 : 0) : 0) != 0)
        {
          if (this.SubGameObjects.Count != 0)
            return;
          this.QueueDispose();
          return;
        }
      }
      this.mySpawnRatePassed += gameTime.ElapsedGameTime.TotalSeconds;
      if (this.mySpawnRatePassed <= (double) this.mySpawnSpeed)
        return;
      int num1 = (int) Math.Floor(this.mySpawnRatePassed / (double) this.mySpawnSpeed);
      if (this.Density.HasValue)
      {
        int? density = this.Density;
        int num2 = this.SubGameObjects.Count + num1;
        if (density.GetValueOrDefault() < num2 & density.HasValue)
          num1 = this.Density.Value - this.SubGameObjects.Count;
      }
      for (int index = 0; index < num1; ++index)
        this.SpawnNewParticle(gameTime);
      this.mySpawnRatePassed %= (double) this.mySpawnSpeed;
    }

    private Particle SpawnNewParticle(GameTime gameTime)
    {
      int totalMilliseconds = this.Definition.LifespanMax.HasValue ? (int) (this.Definition.LifespanMax.Value - this.Definition.LifespanMin).TotalMilliseconds : 0;
      Position position = this.Definition.VelocityMax != (Position) null ? this.Definition.VelocityMax - this.Definition.VelocityMin : new Position(0.0f);
      float num1 = this.Definition.RotationMax.HasValue ? this.Definition.RotationMax.Value - this.Definition.RotationMin : 0.0f;
      Vector2 vector2_1 = this.Definition.ScaleMax.HasValue ? Vector2.Subtract(this.Definition.ScaleMax.Value, this.Definition.ScaleMin) : Vector2.Zero;
      float num2 = this.Definition.RotationVelocityMax.HasValue ? this.Definition.RotationVelocityMax.Value - this.Definition.RotationVelocityMin : 0.0f;
      Vector2 vector2_2 = this.Definition.ScaleVelocityMax.HasValue ? Vector2.Subtract(this.Definition.ScaleVelocityMax.Value, this.Definition.ScaleVelocityMin) : Vector2.Zero;
      Particle particle;
      if (this.myParticlePool.Count > 0)
      {
        particle = this.myParticlePool.Dequeue();
        this.SubGameObjects.Add((GameObject) particle);
      }
      else
        particle = new Particle((GameObject) this);
      particle.Lifespan = this.Definition.LifespanMin + (totalMilliseconds > 0 ? TimeSpan.FromMilliseconds((double) this.Random.Next(totalMilliseconds)) : TimeSpan.Zero);
      particle.myTexture = this.myTextures.Count > 1 ? this.myTextures[this.Random.Next(this.myTextures.Count)] : this.myTextures[0];
      particle.Position = this.Position;
      particle.Depth = this.Depth;
      particle.Colour = this.Definition.Colour;
      particle.ColourChange = this.Definition.ColourChange;
      particle.Velocity = this.Definition.VelocityMin + new Position((double) position.X == 0.0 ? 0.0f : (float) this.Random.Next((int) position.X), (double) position.Y == 0.0 ? 0.0f : (float) this.Random.Next((int) position.Y));
      particle.Rotation = this.Definition.RotationMin + ((double) num1 == 0.0 ? 0.0f : (float) this.Random.NextDouble() % num1);
      particle.Scale = Vector2.Add(this.Definition.ScaleMin, new Vector2((double) vector2_1.X == 0.0 ? 0.0f : (float) this.Random.Next((int) vector2_1.X), (double) vector2_1.Y == 0.0 ? 0.0f : (float) this.Random.Next((int) vector2_1.Y)));
      particle.RotationVelocity = this.Definition.RotationVelocityMin + ((double) num2 == 0.0 ? 0.0f : (float) this.Random.NextDouble() % num2);
      particle.ScaleVelocity = Vector2.Add(this.Definition.ScaleVelocityMin, new Vector2((double) vector2_2.X == 0.0 ? 0.0f : (float) this.Random.Next((int) vector2_2.X), (double) vector2_2.Y == 0.0 ? 0.0f : (float) this.Random.Next((int) vector2_2.Y)));
      particle.Start(gameTime);
      return particle;
    }
  }
}
