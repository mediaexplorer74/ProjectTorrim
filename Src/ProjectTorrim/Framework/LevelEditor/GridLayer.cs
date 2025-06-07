
// Type: BrawlerSource.Framework.LevelEditor.GridLayer
// Assembly: ProjectTorrim, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC8AE86A-8582-48C1-AE0D-A25C0B7D84A4
//

#nullable disable
namespace BrawlerSource.Framework.LevelEditor
{
  public class GridLayer(Level level, int index) : Layer(level, index)
  {
    public MultiGrid Grids;

    public override void Initilize()
    {
      base.Initilize();
      this.ViewCamera.SetDraggable();
      this.ViewCamera.PositionOffset = new Position(0.0f, -16f);
      this.Grids = new MultiGrid((Layer) this, new Position(1920f, 1080f));
      this.GameObjects.Add((GameObject) this.Grids);
      this.Grids.AddToDraw();
      this.Grids.AddGrid();
    }
  }
}
