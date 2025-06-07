
// Type: BrawlerSource.UI.UniqueDraggable
// Assembly: ProjectTorrim, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC8AE86A-8582-48C1-AE0D-A25C0B7D84A4
//

using BrawlerSource.Graphics;

#nullable disable
namespace BrawlerSource.UI
{
  public class UniqueDraggable : Draggable
  {
    public UniqueDraggable(GameObject parent, Position position)
      : this(parent, position, Draggable.DefaultDimensions, Draggable.DefaultSequence, 1f)
    {
    }

    public UniqueDraggable(
      GameObject parent,
      Position position,
      Position dimensions,
      Sequence sequence,
      float depth)
      : base(parent, position, dimensions, sequence, depth)
    {
    }

    public UniqueDraggable(Layer layer, Position position)
      : this(layer, position, Draggable.DefaultDimensions, Draggable.DefaultSequence, 1f)
    {
    }

    public UniqueDraggable(
      Layer layer,
      Position position,
      Position dimensions,
      Sequence sequence,
      float depth)
      : base(layer, position, dimensions, sequence, depth)
    {
    }

    protected override void Construct(
      Position position,
      Position dimensions,
      Sequence sequence,
      float depth)
    {
      base.Construct(position, dimensions, sequence, depth);
      this.AddInvalidCollisionType(typeof (UniqueDraggable));
    }
  }
}
