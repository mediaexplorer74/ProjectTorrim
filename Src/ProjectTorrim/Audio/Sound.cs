
// Type: BrawlerSource.Audio.Sound
// Assembly: ProjectTorrim, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC8AE86A-8582-48C1-AE0D-A25C0B7D84A4
//

using Microsoft.Xna.Framework.Audio;

#nullable disable
namespace BrawlerSource.Audio
{
  public class Sound : AudibleGameObject
  {
    private SoundEffect mySound;
    public float Pitch;

    public Sound(GameObject parent, Channel channel)
      : base(parent, channel)
    {
    }

    public Sound(Layer layer, Channel channel)
      : base(layer, channel)
    {
    }

    public override void Construct(Channel channel)
    {
      this.Pitch = (float) (0.25 - this.Random.NextDouble() * 0.5);
      base.Construct(channel);
    }

    public override void LoadContent()
    {
      this.mySound = this.Game.Content.Load<SoundEffect>(this.AudioPath);
      base.LoadContent();
    }

    public override void Play() => this.mySound.Play(this.myVolume, this.Pitch, 0.0f);
  }
}
