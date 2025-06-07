
// Type: BrawlerSource.Particles.Particle
// Assembly: ProjectTorrim, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC8AE86A-8582-48C1-AE0D-A25C0B7D84A4
//

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

#nullable disable
namespace BrawlerSource.Particles
{
  public class Particle(GameObject parent) : DrawableGameObject(parent)
  {
    public Texture2D myTexture;
    public Position Velocity;
    public float RotationVelocity;
    public Vector2 ScaleVelocity;
    public Color ColourChange;
    private Vector4 ColourVelocity = new Vector4(0.0f, 0.0f, 0.0f, 0.0f);
    public TimeSpan Lifespan;
    private TimeSpan SpawnTime;

    public void Start(GameTime gameTime)
    {
      this.SpawnTime = gameTime.TotalGameTime;
      Color colourChange = this.ColourChange;
      this.ColourVelocity = new Vector4((float) ((int) this.ColourChange.R - this.Colour.R) 
          / (float) this.Lifespan.TotalSeconds, (float) ((int) this.ColourChange.G - 
          (int) this.Colour.G) / (float) this.Lifespan.TotalSeconds,
          (float) ((int) this.ColourChange.B - (int) this.Colour.B) 
          / (float) this.Lifespan.TotalSeconds, 
             (float) ( (int) this.ColourChange.A 
          - (int) this.Colour.A ) / (float) this.Lifespan.TotalSeconds);
    }

    public override void Update(GameTime gameTime)
    {
      base.Update(gameTime);
      float totalSeconds = (float) gameTime.ElapsedGameTime.TotalSeconds;
      this.Position = this.Position + this.Velocity * totalSeconds;
      this.Rotation += this.RotationVelocity * totalSeconds;
      this.Scale = Vector2.Clamp(Vector2.Add(this.Scale, 
          Vector2.Multiply(this.ScaleVelocity, totalSeconds)), Vector2.Zero, new Vector2(100f));
      this.Colour = new Color((float) (((double) this.Colour.R 
          + (double) this.ColourVelocity.X * (double) totalSeconds) / 256.0), 
          (float) (((double) ((Color)this.Colour).G + (double) this.ColourVelocity.Y * (double) totalSeconds) / 256.0), 
          (float) (((double) ((Color)this.Colour).B + (double) this.ColourVelocity.Z * (double) totalSeconds) / 256.0), 
          (float) (((double) ((Color)this.Colour).A + (double) this.ColourVelocity.W * (double) totalSeconds) / 256.0));
      if (!(gameTime.TotalGameTime - this.SpawnTime > this.Lifespan))
        return;
      ((Emitter) this.Parent).RemoveParticle(this);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
      Vector2 vector2 = new Vector2((float) this.myTexture.Width / 2f, (float) this.myTexture.Height / 2f);
      Rectangle rectangle = new Rectangle(0, 0, this.myTexture.Width, this.myTexture.Height);
      spriteBatch.Draw(this.myTexture, this.myPosition, new Rectangle?(rectangle), this.Colour, this.Rotation, vector2, this.myScale, this.Effect, this.Depth);
      base.Draw(spriteBatch);
    }
  }
}
