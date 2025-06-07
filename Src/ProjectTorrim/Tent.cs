
// Type: BrawlerSource.Tent
// Assembly: ProjectTorrim, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC8AE86A-8582-48C1-AE0D-A25C0B7D84A4
//

using BrawlerSource.Collision.Intersections;
using BrawlerSource.Framework.LevelEditor;
using BrawlerSource.Graphics;

#nullable disable
namespace BrawlerSource
{
  [Prefab(typeof (Tent), "Base_B", true, true)]
  public class Tent : Bases
  {
    public Tent(Layer layer, Position position, Position dimensions, float depth)
      : base(layer, position, dimensions, depth)
    {
      SequenceManager<BaseSpriteSequence> sequenceManager1 = new SequenceManager<BaseSpriteSequence>((GameObject) this);
      sequenceManager1.AddSequence(BaseSpriteSequence.Standing, new Sequence()
      {
        TexturePath = "Base_B",
        Width = 48,
        Height = 48,
        InitialImageIndex = 6
      });
      sequenceManager1.AddSequence(BaseSpriteSequence.Falling, new Sequence()
      {
        TexturePath = "Base_B",
        Width = 48,
        Height = 48,
        InitialImageIndex = 6,
        ImageTotal = 6,
        FrameSpeed = 8,
        SequenceEnd = new EndFunction(((Bases) this).FallFinish)
      });
      sequenceManager1.AddSequence(BaseSpriteSequence.Down, new Sequence()
      {
        TexturePath = "Base_B",
        Width = 48,
        Height = 48,
        InitialImageIndex = 11
      });
      SequenceManager<BaseSpriteSequence> sequenceManager2 = sequenceManager1;
      Sprite sprite = new Sprite((GameObject) this);
      sprite.Position = this.Position + new Position(0.0f, -48f);
      sprite.Depth = this.Depth;
      sequenceManager2.SetSprite(sprite);
      this.myBaseSequences.Add(sequenceManager1);
      this.myLastBaseIndex = this.myBaseSequences.Count - 1;
      this.myCollider.AddIntersection((IIntersectionable) new Rectangular()
      {
        Position = (this.Position + new Position(0.0f, -48f)),
        Dimensions = new Position(48f, 48f)
      });
    }
  }
}
