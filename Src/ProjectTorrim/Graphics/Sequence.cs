
// Type: BrawlerSource.Graphics.Sequence
// Assembly: ProjectTorrim, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC8AE86A-8582-48C1-AE0D-A25C0B7D84A4
//

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#nullable disable
namespace BrawlerSource.Graphics
{
  public class Sequence
  {
    public string TexturePath;
    public Texture2D Texture;
    public int Width;
    public int Height;
    public bool Looping;
    public int InitialImageIndex;
    public Vector2 Scroll = Vector2.Zero;
    public int ImageTotal = 1;
    public int FrameSpeed = 1;
    public SpriteEffects Effect;
    public EndFunction SequenceEnd;
  }
}
