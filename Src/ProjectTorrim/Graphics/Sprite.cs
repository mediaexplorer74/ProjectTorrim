
// Type: BrawlerSource.Graphics.Sprite
// Assembly: ProjectTorrim, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC8AE86A-8582-48C1-AE0D-A25C0B7D84A4
//

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

#nullable disable
namespace BrawlerSource.Graphics
{
  public class Sprite : DrawableGameObject
  {
    public Vector2 Scroll = Vector2.Zero;
    public int ImageIndex;
    public double ImageSpeed;
    public double FrameTimeElapsed;
    private Sequence mySequence;
    public EndFunction OnSequenceEnd;

    public Sequence Sequence
    {
      get => this.mySequence;
      set
      {
        this.mySequence = value;
        this.ResetImageIndex();
      }
    }

    public Sprite(GameObject parent)
      : base(parent)
    {
    }

    public Sprite(Layer layer)
      : base(layer)
    {
    }

    public override void LoadContent()
    {
      this.mySequence.Texture = this.Game.Content.Load<Texture2D>(this.mySequence.TexturePath);
      if (this.mySequence.Width == 0)
        this.mySequence.Width = this.mySequence.Texture.Width;
      if (this.mySequence.Height == 0)
        this.mySequence.Height = this.mySequence.Texture.Height;
      base.LoadContent();
    }

    public override void Update(GameTime gameTime)
    {
      double totalSeconds = gameTime.ElapsedGameTime.TotalSeconds;
      this.Scroll = Vector2.Add(this.Scroll, Vector2.Multiply(this.mySequence.Scroll, (float) totalSeconds));
      if (this.ImageSpeed == 0.0)
        return;
      this.FrameTimeElapsed += totalSeconds;
      if (this.FrameTimeElapsed <= this.ImageSpeed / (double) this.mySequence.FrameSpeed)
        return;
      int num = (int) (this.ImageSpeed / Math.Abs(this.ImageSpeed));
      this.ImageIndex += num;
      this.FrameTimeElapsed = 0.0;
      if (this.ImageIndex < this.mySequence.InitialImageIndex + this.mySequence.ImageTotal && this.ImageIndex >= this.mySequence.InitialImageIndex)
        return;
      if (this.mySequence.Looping)
        this.ResetImageIndex();
      else
        this.ImageIndex -= num;
      EndFunction endFunction = this.OnSequenceEnd ?? this.Sequence.SequenceEnd;
      if (endFunction == null)
        return;
      endFunction();
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
      if (this.mySequence.Texture != null && !this.IsHidden)
      {
        Vector2 vector2 = new Vector2((float) this.mySequence.Width / 2f, 
            (float) this.mySequence.Height / 2f);
        spriteBatch.Draw(this.mySequence.Texture, this.myPosition, new Rectangle?(this.GetCurrentFrame()), this.Colour, this.Rotation, vector2, this.myScale, this.mySequence.Effect | this.Effect, this.Depth);
      }
      base.Draw(spriteBatch);
    }

    public void ResetImageIndex()
    {
      this.ImageIndex = this.ImageSpeed < 0.0 ? this.mySequence.InitialImageIndex + this.mySequence.ImageTotal - 1 : this.mySequence.InitialImageIndex;
    }

    public Rectangle GetCurrentFrame()
    {
      int num1 = this.ImageIndex * this.mySequence.Width % this.mySequence.Texture.Bounds.Width;
      int num2 = this.ImageIndex * this.mySequence.Width / this.mySequence.Texture.Bounds.Width * this.mySequence.Height;
      int x = (int) this.Scroll.X;
      return new Rectangle(num1 + x, num2 + (int) this.Scroll.Y, this.mySequence.Width, this.mySequence.Height);
    }
  }
}
