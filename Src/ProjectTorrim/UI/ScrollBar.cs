
// Type: BrawlerSource.UI.ScrollBar
// Assembly: ProjectTorrim, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC8AE86A-8582-48C1-AE0D-A25C0B7D84A4
//

using BrawlerSource.Input;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

#nullable disable
namespace BrawlerSource.UI
{
  public class ScrollBar : Element
  {
    private Slider mySlider;
    public Dictionary<DrawableGameObject, Position> ScrollingObjects;

    public ScrollBar(Layer layer, Vector2 scale, Align alignment)
      : base(layer, scale, alignment, new Position(0.0f))
    {
      this.ScrollingObjects = new Dictionary<DrawableGameObject, Position>();
      Button button1 = new Button((GameObject) this, alignment | Align.Left, 
          new Position(0.0f), new Position(16f), new MouseFunction(this.ScrollLeft), 
          new TouchFunction(this.ScrollLeft));
      button1.SetColour(Color.Gray);
      button1.SetText("<");
      Button button2 = new Button((GameObject) this, alignment | Align.Right, 
          new Position(0.0f), new Position(16f), new MouseFunction(this.ScrollRight), 
          new TouchFunction(this.ScrollRight));
      button2.SetColour(Color.Gray);
      button2.SetText(">");
      this.mySlider = new Slider((GameObject) this, 
          new Position(this.Layer.ViewCamera.WorldSize.X, 16f) - new Position(32f, 0.0f), 16, 
          new Position(0.0f, (float) ((double) this.Layer.ViewCamera.WorldSize.Y / 2.0 
          - (double) this.Scale.Y / 2.0 - 4.0)));
      this.mySlider.Cursor.Position = new Position(this.mySlider.Cursor.MinPosition.X, 
          this.mySlider.Cursor.Position.Y);
    }

    public void ScrollLeft(object sender, BrawlerEventArgs e)
    {
      UniqueDraggable cursor = this.mySlider.Cursor;
      cursor.Position = cursor.Position + new Position(-8f, 0.0f);
    }

    public void ScrollRight(object sender, BrawlerEventArgs e)
    {
      UniqueDraggable cursor = this.mySlider.Cursor;
      cursor.Position = cursor.Position + new Position(8f, 0.0f);
    }

    public void AddScrollItem(DrawableGameObject item)
    {
      this.ScrollingObjects.Add(item, item.Position);
    }

    public override void Update(GameTime gameTime)
    {
      foreach (DrawableGameObject key in this.ScrollingObjects.Keys)
      {
        Position scrollingObject = this.ScrollingObjects[key];
        key.Position = scrollingObject + new Position((float) ((double) key.Scale.X 
            * (double) this.mySlider.Value * -1.0), 0.0f);
      }
      base.Update(gameTime);
    }
  }
}
