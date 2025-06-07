
// Type: BrawlerSource.Audio.MediaSong
// Assembly: ProjectTorrim, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC8AE86A-8582-48C1-AE0D-A25C0B7D84A4
//

using Microsoft.Xna.Framework.Media;
using System;

#nullable disable
namespace BrawlerSource.Audio
{
  public class MediaSong : AudibleGameObject
  {
    private Song mySound;

    public MediaSong(GameObject parent, Channel channel)
      : base(parent, channel)
    {
    }

    public MediaSong(Layer layer, Channel channel)
      : base(layer, channel)
    {
    }

    public override void Construct(Channel channel)
    {
      base.Construct(channel);
      this.Game.Master.OnVolumeChanged += new EventHandler(this.Channel_OnVolumeChanged);
      this.Channel.OnVolumeChanged += new EventHandler(this.Channel_OnVolumeChanged);
    }

    private void Channel_OnVolumeChanged(object sender, EventArgs e)
    {
      MediaPlayer.Volume = this.myVolume;
    }

    public override void LoadContent()
    {
      this.mySound = this.Game.Content.Load<Song>(this.AudioPath);
      MediaPlayer.Volume = this.myVolume;
      base.LoadContent();
    }

    public override void Play()
    {
      MediaPlayer.IsRepeating = true;
      MediaPlayer.Play(this.mySound);
    }
  }
}
