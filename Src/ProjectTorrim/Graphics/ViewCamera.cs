
// Type: BrawlerSource.Graphics.ViewCamera
// Assembly: ProjectTorrim, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC8AE86A-8582-48C1-AE0D-A25C0B7D84A4
//

using BrawlerSource.Collision.Intersections;
using BrawlerSource.Input;
using Microsoft.Xna.Framework;
using System;

#nullable disable
namespace BrawlerSource.Graphics
{
  public class ViewCamera : GameObject
  {
    private Rectangular myIntersection;
    private InputEvents myInput;
    private bool RequestDrag;
    private bool IsDragging;
    private bool WasDragging;
    private Position DragPosition;
    private Position DragStart;
    public Position Position = new Position(0.0f, 0.0f);
    public Position PositionOffset = new Position(0.0f, 0.0f);
    public Position MinPosition;
    public Position MaxPosition;
    private Vector2 myInitialScreenSize;
    private Vector2 myScreenSize;
    private float myRelativeScale;
    private float myScale;
    private float myInverseScale;

    public event EventHandler OnResize;

    public Vector2 ScreenCentre { get; protected set; }

    public Vector2 ScreenSize
    {
      get => this.myScreenSize;
      set
      {
        this.myScreenSize = value;
        this.ScreenCentre = Vector2.Multiply(value, 0.5f);
        this.RecalculateView();
      }
    }

    public Vector2 TopLeft => this.myIntersection.TopLeft.ToVector2();

    public Vector2 BottomRight => this.myIntersection.BottomRight.ToVector2();

    public Vector2 WorldSize => this.myIntersection.Dimensions.ToVector2();

    public float RelativeScale
    {
      get => this.myRelativeScale;
      set
      {
        this.myRelativeScale = value;
        this.Scale = this.myRelativeScale;
        this.RecalculateView();
      }
    }

    public float Scale
    {
      get => this.myScale;
      protected set
      {
        this.myScale = value;
        this.myInverseScale = 1f / this.myScale;
      }
    }

    public ViewCamera(Layer layer, Vector2 size)
      : base(layer)
    {
      this.Game.Graphics.OnScreenResize += new EventHandler(this.Graphics_OnScreenResize);
      this.myInput = new InputEvents((GameObject) this);
      this.myIntersection = new Rectangular()
      {
        Position = this.Position
      };
      this.myInitialScreenSize = size;
      this.ScreenSize = size;
      this.RelativeScale = 1f;
    }

    private void RecalculateView()
    {
      Vector2 vector2 = Vector2.Divide(this.ScreenSize, this.myInitialScreenSize);
      this.Scale = ((double) vector2.X < (double) vector2.Y ? vector2.X : vector2.Y) * this.RelativeScale;
      this.myIntersection.Dimensions = new Position(Vector2.Divide(this.ScreenSize, this.Scale));
    }

    private void Graphics_OnScreenResize(object sender, EventArgs e)
    {
      this.ScreenSize = ((ScreenResizeEventArgs) e).Size;
      EventHandler onResize = this.OnResize;
      if (onResize == null)
        return;
      onResize((object) this, new EventArgs());
    }

    public Position GetScreenPosition(Position worldPosition)
    {
      return (worldPosition - this.Position) * this.Scale + this.ScreenCentre;
    }

    public Position GetWorldPosition(Position screenPosition)
    {
      return (screenPosition - this.ScreenCentre) * this.myInverseScale + this.Position;
    }

    public void SetDraggable()
    {
      this.myInput.AddMouseButton(MouseButtons.Left, InputType.Pressed, new MouseFunction(this.Drag));
      this.myInput.AddMouseButton(MouseButtons.Left, InputType.Held, new MouseFunction(this.Drag));
      this.myInput.AddMouseButton(MouseButtons.Left, InputType.Released, new MouseFunction(this.StopDrag));
    }

    public void StopDrag(object sender, BrawlerEventArgs e) => this.SetIsDragging(false);

    public void Drag(object sender, BrawlerEventArgs e) => this.SetIsDragging(true);

    public virtual void SetIsDragging(bool isDragging)
    {
      this.RequestDrag = isDragging;
      if (this.RequestDrag)
      {
        this.DragPosition = this.Position;
        this.DragStart = this.Layer.Cursor.ScreenPosition;
      }
      else
        this.IsDragging = false;
    }

    public override void Update(GameTime gameTime)
    {
      if (this.IsDragging && this.Layer.ActiveDragger == null)
        this.Layer.ActiveDragger = (GameObject) this;
      else if (this.WasDragging && !this.IsDragging && this.Layer.ActiveDragger == this)
        this.Layer.ActiveDragger = (GameObject) null;
      if (this.IsDragging && this.Layer.ActiveDragger == this)
      {
        this.Position = this.DragPosition + (this.DragStart - this.Layer.Cursor.ScreenPosition) / this.Scale;
        if (this.MinPosition != (Position) null && this.MaxPosition != (Position) null)
          this.Position = this.Position.Clamp(this.MinPosition, this.MaxPosition);
      }
      this.WasDragging = this.IsDragging;
      this.IsDragging = this.RequestDrag;
      this.myIntersection.Position = this.Position + this.PositionOffset;
      base.Update(gameTime);
    }
  }
}
