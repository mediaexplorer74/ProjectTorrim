
// Type: BrawlerSource.Audio.Channel
// Assembly: ProjectTorrim, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC8AE86A-8582-48C1-AE0D-A25C0B7D84A4
//

using System;

#nullable disable
namespace BrawlerSource.Audio
{
  public class Channel
  {
    private float myVolume;

    public event EventHandler OnVolumeChanged;

    public float Volume
    {
      get => this.myVolume;
      set
      {
        if ((double) value == (double) this.myVolume)
          return;
        this.myVolume = value;
        EventHandler onVolumeChanged = this.OnVolumeChanged;
        if (onVolumeChanged == null)
          return;
        onVolumeChanged((object) this, (EventArgs) new VolumeChangedEventArgs()
        {
          Volume = this.myVolume
        });
      }
    }

    public Channel() => this.Volume = 1f;
  }
}
