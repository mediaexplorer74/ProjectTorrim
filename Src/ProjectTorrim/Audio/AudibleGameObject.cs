
// Type: BrawlerSource.Audio.AudibleGameObject
// Assembly: ProjectTorrim, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC8AE86A-8582-48C1-AE0D-A25C0B7D84A4
//

#nullable disable
namespace BrawlerSource.Audio
{
  public class AudibleGameObject : ContentGameObject
  {
    public static int AudioCount;
    public string AudioPath;
    public Channel Channel;

    public float Volume { get; set; }

    protected float myVolume => this.Channel.Volume * this.Volume * this.Game.Master.Volume;

    public AudibleGameObject(GameObject parent, Channel channel)
      : base(parent)
    {
      this.Construct(channel);
    }

    public AudibleGameObject(Layer layer, Channel channel)
      : base(layer)
    {
      this.Construct(channel);
    }

    public virtual void Construct(Channel channel)
    {
      this.Channel = channel;
      this.Volume = 1f;
    }

    public virtual void Play()
    {
    }
  }
}
