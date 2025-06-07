
// Type: BrawlerSource.Framework.LevelEditor.DraggableDeletable
// Assembly: ProjectTorrim, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC8AE86A-8582-48C1-AE0D-A25C0B7D84A4
//

using BrawlerSource.Graphics;
using BrawlerSource.Input;
using BrawlerSource.UI;

#nullable disable
namespace BrawlerSource.Framework.LevelEditor
{
  public class DraggableDeletable : Draggable
  {
    public Grid Grid;

    public TileInfo Info { get; set; }

    public DraggableDeletable(Layer layer, Position position)
      : this(layer, position, Draggable.DefaultDimensions, Draggable.DefaultSequence, 1f)
    {
    }

    public DraggableDeletable(
      Layer layer,
      Position position,
      Position dimensions,
      Sequence sequence,
      float depth)
      : base(layer, position, dimensions, sequence, depth)
    {
    }

    public DraggableDeletable(GameObject parent, Position position)
      : this(parent, position, Draggable.DefaultDimensions, Draggable.DefaultSequence, 1f)
    {
    }

    public DraggableDeletable(
      GameObject parent,
      Position position,
      Position dimensions,
      Sequence sequence,
      float depth)
      : base(parent, position, dimensions, sequence, depth)
    {
    }

    protected override void Construct(
      Position position,
      Position dimensions,
      Sequence sequence,
      float depth)
    {
      base.Construct(position, dimensions, sequence, depth);
      this.myCollider.AddMouseInput(MouseButtons.Right, InputType.Pressed, new MouseFunction(this.Delete));
    }

    public void Delete(object sender, MouseEventArgs e)
    {
      this.Grid?.ObjectList.Remove(this);
      this.QueueDispose();
    }

    public DraggableDeletableProperties GetProperties()
    {
      DraggableDeletableProperties properties = new DraggableDeletableProperties();
      properties.Dimensions = this.Dimensions;
      properties.Position = this.Position;
      properties.Info = this.Info;
      return properties;
    }

    public void SetProperties(DraggableDeletableProperties properties)
    {
      this.Info = properties.Info;
      this.Position = properties.Position;
      this.mySprite.Sequence = new Sequence()
      {
        TexturePath = this.Info.SpriteName,
        Width = this.SnapSize,
        Height = this.SnapSize
      };
      this.mySprite.LoadContent();
      this.myRectangle.Dimensions = properties.Dimensions;
      this.IsRepeating = this.Info.IsRepeating;
      this.IsResizeable = this.Info.IsResizeable;
    }
  }
}
