
// Type: BrawlerSource.Graphics.SequenceManager`1
// Assembly: ProjectTorrim, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC8AE86A-8582-48C1-AE0D-A25C0B7D84A4
//

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace BrawlerSource.Graphics
{
  public class SequenceManager<T> : DrawableGameObject
  {
    public Sprite Sprite;
    public Dictionary<T, BrawlerSource.Graphics.Sequence> Sequences = new Dictionary<T, BrawlerSource.Graphics.Sequence>();
    public Dictionary<string, Texture2D> Textures = new Dictionary<string, Texture2D>();
    public HashSet<string> TextureList = new HashSet<string>();
    private bool myPrimarySequenceLock;
    private int myPrimaryRepetitions;
    public T PrimarySequence;
    public T Sequence;
    private T myDesiredSequence;

    public SequenceManager(Layer layer)
      : base(layer)
    {
    }

    public SequenceManager(GameObject parent)
      : base(parent)
    {
    }

    public override void LoadContent()
    {
      foreach (string texture in this.TextureList)
      {
        this.Textures.Add(texture, this.Game.Content.Load<Texture2D>(texture));
        foreach (T key in this.Sequences.Keys)
        {
          BrawlerSource.Graphics.Sequence sequence = this.Sequences[key];
          if (sequence.TexturePath == texture)
            sequence.Texture = this.Textures[texture];
        }
      }
      base.LoadContent();
    }

    public override void Update(GameTime gameTime)
    {
      if (!(this.myPrimarySequenceLock ? this.PrimarySequence : this.myDesiredSequence).Equals((object) this.Sequence))
      {
        this.Sprite.Sequence = this.Sequences[this.myPrimarySequenceLock ? this.PrimarySequence : this.myDesiredSequence];
        this.Sequence = this.myPrimarySequenceLock ? this.PrimarySequence : this.myDesiredSequence;
      }
      base.Update(gameTime);
    }

    public void AddSequence(T name, BrawlerSource.Graphics.Sequence sequence)
    {
      this.Sequences.Add(name, sequence);
      this.TextureList.Add(sequence.TexturePath);
    }

    public void SetSequence(T sequence) => this.SetSequence(sequence, false);

    private void SetSequence(T sequence, bool isPrimary)
    {
      if (this.myPrimarySequenceLock || isPrimary)
        return;
      this.myDesiredSequence = sequence;
    }

    public void SetSequence(T sequence, double imageSpeed)
    {
      if (this.myPrimarySequenceLock)
        return;
      this.SetSequence(sequence);
      this.SetSequenceSpeed(imageSpeed);
    }

    public void PlaySequence(T sequence) => this.PlaySequence(sequence, 1);

    public void PlaySequence(T sequence, int repetitions)
    {
      this.PlaySequence(sequence, repetitions, 1.0);
    }

    public void PlaySequence(T sequence, int repetitions, double imageSpeed)
    {
      if (this.myPrimarySequenceLock)
        return;
      this.PrimarySequence = sequence;
      this.myPrimaryRepetitions = repetitions;
      this.SetSequence(sequence, true);
      this.SetSequenceSpeed(imageSpeed);
      this.myPrimarySequenceLock = true;
      this.Sprite.OnSequenceEnd = new EndFunction(this.OnSequenceEnd);
    }

    public void OnSequenceEnd()
    {
      if (!this.myPrimarySequenceLock)
        return;
      --this.myPrimaryRepetitions;
      if (this.myPrimaryRepetitions >= 1)
        return;
      this.Sprite.OnSequenceEnd = (EndFunction) null;
      this.myPrimarySequenceLock = false;
      this.SetSequence(this.Sequence);
      this.SetSequenceSpeed(0.0);
      EndFunction sequenceEnd = this.Sprite.Sequence.SequenceEnd;
      if (sequenceEnd == null)
        return;
      sequenceEnd();
    }

    public void SetSequenceSpeed(double imageSpeed)
    {
      if (this.myPrimarySequenceLock)
        return;
      this.Sprite.ImageSpeed = imageSpeed;
    }

    public void StopSequence()
    {
      if (this.myPrimarySequenceLock)
        return;
      this.Sprite.ResetImageIndex();
      this.SetSequenceSpeed(0.0);
    }

    public void SetSprite(Sprite sprite)
    {
      this.Sprite = sprite;
      T key = this.Sequences.Keys.First<T>();
      this.Sprite.Sequence = this.Sequences[key];
      this.Sequence = key;
      this.myDesiredSequence = key;
      this.Sprite.ImageSpeed = 1.0;
    }
  }
}
