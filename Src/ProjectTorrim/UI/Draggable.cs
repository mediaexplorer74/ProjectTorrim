
// Type: BrawlerSource.UI.Draggable
// Assembly: ProjectTorrim, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC8AE86A-8582-48C1-AE0D-A25C0B7D84A4
//

using BrawlerSource.Collision;
using BrawlerSource.Collision.Intersections;
using BrawlerSource.Graphics;
using BrawlerSource.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

#nullable disable
namespace BrawlerSource.UI
{
  public class Draggable : DrawableGameObject
  {
    protected static Sequence DefaultSequence = new Sequence()
    {
      TexturePath = "Square_32",
      Width = 32,
      Height = 32
    };
    protected static Position DefaultDimensions = new Position(32f);
    private int EdgeSize = 4;
    protected Sprite mySprite;
    protected ClickableCollider myCollider;
    protected Rectangular myRectangle;
    private InputEvents myInput;
    private TimeSpan StartDragging;
    public TimeSpan? DragTime;
    public bool WasDragging;
    public bool IsDragging;
    public Position MouseOffset;
    protected HashSet<GameObject> InvalidCollisions;
    protected HashSet<GameObject> PreviousInvalidCollisions;
    public bool IsDisabled;
    public Position MinPosition;
    public Position MaxPosition;
    public int SnapSize;
    public bool IsResizeable;
    private Position mySideScale;
    private Position myAnchorPosition;
    private bool myIsRepeating;

    public event EventHandler OnMove;

    public Position Dimensions => this.myRectangle.Dimensions;

    public bool IsRepeating
    {
      get => this.myIsRepeating;
      set
      {
        this.myIsRepeating = value;
        if (this.mySprite == null)
          return;
        this.ScaleSpriteToRectangle();
      }
    }

    public Draggable(GameObject parent, Position position)
      : this(parent, position, Draggable.DefaultDimensions, Draggable.DefaultSequence, 1f)
    {
    }

    public Draggable(
      GameObject parent,
      Position position,
      Position dimensions,
      Sequence sequence,
      float depth)
      : base(parent)
    {
      this.Construct(position, dimensions, sequence, depth);
    }

    public Draggable(Layer layer, Position position)
      : this(layer, position, Draggable.DefaultDimensions, Draggable.DefaultSequence, 1f)
    {
    }

    public Draggable(
      Layer layer,
      Position position,
      Position dimensions,
      Sequence sequence,
      float depth)
      : base(layer)
    {
      this.Construct(position, dimensions, sequence, depth);
    }

    protected virtual void Construct(
      Position position,
      Position dimensions,
      Sequence sequence,
      float depth)
    {
      this.Position = position;
      this.IsDisabled = false;
      this.WasDragging = false;
      this.IsDragging = false;
      this.MouseOffset = new Position();
      this.InvalidCollisions = new HashSet<GameObject>();
      this.PreviousInvalidCollisions = new HashSet<GameObject>();
      this.SnapSize = 1;
      this.myRectangle = new Rectangular()
      {
        Dimensions = dimensions
      };
      ClickableCollider clickableCollider = new ClickableCollider((GameObject) this);
      clickableCollider.Position = this.Position;
      this.myCollider = clickableCollider;
      this.myCollider.AddIntersection((IIntersectionable) this.myRectangle);
      this.myInput = new InputEvents((GameObject) this);
      this.myCollider.AddMouseInput(MouseButtons.Left, InputType.Pressed, new MouseFunction(this.OnPressed));
      this.myInput.AddMouseButton(MouseButtons.Left, InputType.Released, new MouseFunction(this.OnReleased));
      if (sequence == null)
        return;
      Sprite sprite = new Sprite((GameObject) this);
      sprite.Sequence = sequence;
      sprite.Position = this.Position;
      sprite.Depth = depth;
      this.mySprite = sprite;
      this.ScaleSpriteToRectangle();
    }

    protected void ScaleSpriteToRectangle()
    {
      if (this.IsRepeating)
      {
        this.mySprite.Sequence.Width = (int) this.myRectangle.Width;
        this.mySprite.Sequence.Height = (int) this.myRectangle.Height;
        this.mySprite.Scale = new Vector2(1f);
      }
      else
        this.mySprite.Scale = (new Position(this.myRectangle.Width, this.myRectangle.Height) / new Position((float) this.mySprite.Sequence.Width, (float) this.mySprite.Sequence.Height)).ToVector2();
    }

    public void SetDisabled(bool isDisabled)
    {
      this.myCollider.IsDisabled = isDisabled;
      this.IsDisabled = isDisabled;
    }

    public void AddInvalidCollisionType(Type t)
    {
      this.myCollider.AddEvent(t, TouchType.Touching, new CollisionFunc(this.InvalidateDrop));
    }

    public void InvalidateDrop(object sender, CollisionEventArgs e)
    {
      this.InvalidCollisions.Add(e.Trigger);
    }

    public void OnReleased(object sender, BrawlerEventArgs e)
    {
      this.SetIsDragging(e.GameTime, false);
      this.mySideScale = (Position) null;
      this.myAnchorPosition = (Position) null;
    }

    public void OnPressed(object sender, MouseEventArgs e)
    {
      this.OnPressed((BrawlerEventArgs) e, e.Position);
    }

    public void OnPressed(BrawlerEventArgs e, Position pos)
    {
      this.SetIsDragging(e.GameTime, true);
      if (!this.IsDragging || !this.IsResizeable)
        return;
      this.mySideScale = new Position((double) pos.X < (double) this.myRectangle.Left || (double) pos.X > (double) this.myRectangle.Left + (double) this.EdgeSize ? ((double) pos.X > (double) this.myRectangle.Right || (double) pos.X < (double) this.myRectangle.Right - (double) this.EdgeSize ? 0.0f : 1f) : -1f, (double) pos.Y < (double) this.myRectangle.Top || (double) pos.Y > (double) this.myRectangle.Top + (double) this.EdgeSize ? ((double) pos.Y > (double) this.myRectangle.Bottom || (double) pos.Y < (double) this.myRectangle.Bottom - (double) this.EdgeSize ? 0.0f : 1f) : -1f);
      if (!(this.mySideScale != new Position(0.0f)))
        return;
      this.myAnchorPosition = new Position((double) this.mySideScale.X == 0.0 ? this.Position.X : ((double) this.mySideScale.X == -1.0 ? this.myRectangle.Right : this.myRectangle.Left), (double) this.mySideScale.Y == 0.0 ? this.Position.Y : ((double) this.mySideScale.Y == -1.0 ? this.myRectangle.Bottom : this.myRectangle.Top));
    }

    public void SetIsDragging(GameTime gameTime, bool isDragging)
    {
      this.IsDragging = isDragging && (this.Layer.ActiveDragger == null || this.Layer.ActiveDragger == this);
      if (this.IsDragging)
      {
        this.Layer.ActiveDragger = (GameObject) this;
        this.StartDragging = gameTime.TotalGameTime;
        this.DragTime = new TimeSpan?();
      }
      else
        this.DragTime = new TimeSpan?(gameTime.TotalGameTime - this.StartDragging);
    }

    public override void FirstUpdate(GameTime gameTime)
    {
      this.Position = this.Position.Floor((double) this.SnapSize) + this.myRectangle.Dimensions / 2f % (float) this.SnapSize;
      this.myCollider.Position = this.Position;
      if (this.mySprite != null)
        this.mySprite.Position = this.Position;
      base.FirstUpdate(gameTime);
    }

    public override void Update(GameTime gameTime)
    {
      this.PreviousInvalidCollisions = this.InvalidCollisions;
      this.InvalidCollisions = new HashSet<GameObject>();
      this.ValidateDrag();
      if (this.IsDragging)
        this.Drag();
      this.myCollider.Position = this.Position;
      if (this.mySprite != null)
        this.mySprite.Position = this.Position;
      this.WasDragging = this.IsDragging;
      base.Update(gameTime);
    }

    public bool IsValidPosition() => this.PreviousInvalidCollisions.Count == 0;

    private void ValidateDrag()
    {
      this.IsDragging = ((this.IsDragging ? 1 : 0) | (this.IsValidPosition() ? 0 : (this.Layer.ActiveDragger == this ? 1 : 0))) != 0;
      if (!this.WasDragging && this.IsDragging)
        this.MouseOffset = this.Position - this.Layer.Cursor.Position;
      else if (this.WasDragging && !this.IsDragging && this.Layer.ActiveDragger == this)
        this.Layer.ActiveDragger = (GameObject) null;
      if (!this.IsDragging)
        return;
      this.DragTime = new TimeSpan?();
    }

    private void Drag()
    {
      if (this.myAnchorPosition != (Position) null)
      {
        Position position1 = this.Layer.Cursor.Position - this.myAnchorPosition;
        position1.X = (double) this.mySideScale.X == 0.0 ? this.myRectangle.Width : position1.X;
        position1.Y = (double) this.mySideScale.Y == 0.0 ? this.myRectangle.Height : position1.Y;
        Position position2 = (position1.Abs() / (float) this.SnapSize).Round(0, 1.0).Clamp(new Position(1f), new Position(float.MaxValue));
        if (this.IsRepeating)
        {
          this.mySprite.Sequence.Width = (int) ((double) position2.X * (double) this.SnapSize);
          this.mySprite.Sequence.Height = (int) ((double) position2.Y * (double) this.SnapSize);
        }
        else
          this.mySprite.Scale = position2.ToVector2();
        this.myRectangle.Width = (float) (int) ((double) this.mySprite.Sequence.Width * (double) this.mySprite.Scale.X);
        this.myRectangle.Height = (float) (int) ((double) this.mySprite.Sequence.Height * (double) this.mySprite.Scale.Y);
        this.Position = this.myAnchorPosition + new Position(this.myRectangle.Width, this.myRectangle.Height) * this.mySideScale / 2f;
        this.mySprite.Position = this.Position;
        this.myCollider.Position = this.Position;
      }
      else
        this.Position = (this.Layer.Cursor.Position + this.MouseOffset).Floor((double) this.SnapSize) + this.myRectangle.Dimensions / 2f % (float) this.SnapSize;
      if (this.MinPosition != (Position) null && this.MaxPosition != (Position) null)
        this.Position = this.Position.Clamp(this.MinPosition, this.MaxPosition);
      EventHandler onMove = this.OnMove;
      if (onMove == null)
        return;
      onMove((object) this, (EventArgs) new MoveEventArgs());
    }
  }
}
