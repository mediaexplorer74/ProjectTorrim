
// Type: BrawlerSource.UI.Button
// Assembly: ProjectTorrim, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC8AE86A-8582-48C1-AE0D-A25C0B7D84A4
//

using BrawlerSource.Collision;
using BrawlerSource.Collision.Intersections;
using BrawlerSource.Graphics;
using BrawlerSource.Input;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

#nullable disable
namespace BrawlerSource.UI
{
  public class Button : Element
  {
    private MouseFunction Mouse_Click;
    public List<object> Args;
    private ClickableCollider myCollider;
    private bool myIsDisabled;
    private Color myColour;
    public Sprite SpriteMain;
    public Sprite SpriteShadow;
    public Sprite SpriteFront;
    private Position mySpriteFrontOffset;
    public Text TextMain;

    public bool IsDisabled
    {
      get => this.myIsDisabled;
      set => this.SetIsDisabled(value);
    }

    public Button(
      GameObject parent,
      Align alignment,
      Position position,
      Position dimensions,
      MouseFunction func)
      : base(parent, dimensions.ToVector2(), alignment, position)
    {
      this.Construct(alignment, position, dimensions, func);
    }

    public Button(
      Layer layer,
      Align alignment,
      Position position,
      Position dimensions,
      MouseFunction func)
      : base(layer, dimensions.ToVector2(), alignment, position)
    {
      this.Construct(alignment, position, dimensions, func);
    }

    protected virtual void Construct(
      Align alignment,
      Position position,
      Position dimensions,
      MouseFunction func)
    {
      this.Args = new List<object>();
      this.Mouse_Click = func;
      this.Alignment = alignment;
      this.Offset = position;
      this.mySpriteFrontOffset = new Position(0.0f);
      ClickableCollider clickableCollider = new ClickableCollider((GameObject) this);
      clickableCollider.Position = this.Position;
      this.myCollider = clickableCollider;
      this.myCollider.AddIntersection((IIntersectionable) new Rectangular()
      {
        Dimensions = new Position(this.Scale)
      });
      this.myCollider.AddMouseInput(MouseButtons.Left, InputType.Pressed, new MouseFunction(this.OnClick));
      Sprite sprite1 = new Sprite((GameObject) this);
      sprite1.Sequence = new Sequence()
      {
        TexturePath = "Square_Rounded_256",
        Width = 256,
        Height = 256
      };
      sprite1.Scale = Vector2.Divide(this.Scale, 256f);
      sprite1.Depth = 0.5f;
      sprite1.Position = this.Position;
      this.SpriteMain = sprite1;
      Sprite sprite2 = new Sprite((GameObject) this);
      sprite2.Sequence = new Sequence()
      {
        TexturePath = "Square_Rounded_256",
        Width = 256,
        Height = 256
      };
      sprite2.Scale = Vector2.Divide(this.Scale, 256f);
      sprite2.Depth = 0.4f;
      sprite2.Position = this.Position + new Position(8f, 8f);
      sprite2.Colour = new Color(0, 0, 0, 128);
      this.SpriteShadow = sprite2;
      Text text = new Text((GameObject) this);
      text.FontPath = "Font";
      text.Colour = Color.Black;
      text.Depth = 1f;
      text.Scale = new Vector2(0.4f);
      text.Position = this.Position;
      this.TextMain = text;
      this.SetIsDisabled(false);
      this.SetColour(Color.White);
    }

    public override void LoadContent()
    {
      base.LoadContent();
      if (this.SpriteFront == null)
        return;
      this.FitSpriteFront();
    }

    private void FitSpriteFront()
    {
      Vector2 vector2 = new Vector2((float) this.SpriteFront.Sequence.Width, (float) this.SpriteFront.Sequence.Height);
      if ((double) this.Scale.X >= (double) vector2.X && (double) this.Scale.Y >= (double) vector2.Y)
        return;
      this.SpriteFront.Scale = Vector2.Divide(this.Scale, vector2);
    }

    public override void Update(GameTime gameTime)
    {
      this.myCollider.IsDisabled = this.myIsDisabled || this.IsHidden;
      this.myCollider.Position = this.Position;
      this.SpriteMain.Position = this.Position;
      this.SpriteShadow.Position = this.Position + new Position(8f, 8f);
      this.TextMain.Position = this.Position;
      if (this.SpriteFront != null)
        this.SpriteFront.Position = this.Position + this.mySpriteFrontOffset;
      base.Update(gameTime);
    }

    private void OnClick(object sender, MouseEventArgs e)
    {
      this.Level.GameLayer?.ViewCamera.SetIsDragging(false);
      e.Args = this.Args;
      this.Mouse_Click(sender, e);
    }

    public void SetText(string text) => this.TextMain.String = text;

    public void SetTextScale(Vector2 scale) => this.TextMain.Scale = scale;

    public void SetSprite(string texturePath) => this.SetSprite(texturePath, new Vector2?());

    public void SetSprite(string texturePath, Vector2? size)
    {
      Sequence sequence = new Sequence()
      {
        TexturePath = texturePath
      };
      Vector2 vector2 = Vector2.Divide(this.Scale, size ?? new Vector2(32f));
      Sprite sprite = new Sprite((GameObject) this);
      sprite.Sequence = sequence;
      sprite.Scale = new Vector2((double) vector2.X > (double) vector2.Y ? vector2.Y : vector2.X);
      sprite.Depth = 0.6f;
      sprite.Position = this.Position;
      this.SpriteFront = sprite;
      if (size.HasValue)
      {
        sequence.Width = (int) size.Value.X;
        sequence.Height = (int) size.Value.Y;
      }
      if (!this.SubGameObjects.Contains((GameObject) this.SpriteFront))
        return;
      this.SpriteFront.LoadContent();
      this.FitSpriteFront();
    }

    public void SetSpritePosition(Position position) => this.mySpriteFrontOffset = position;

    public void SetColour(Color colour)
    {
      this.myColour = colour;
      if (this.myIsDisabled)
        return;
      this.SpriteMain.Colour = this.myColour;
    }

    public void SetIsDisabled(bool isDisabled)
    {
      this.myIsDisabled = isDisabled;
      this.SpriteMain.Colour = this.myIsDisabled ? Color.DarkGray : this.myColour;
      this.myCollider.IsDisabled = this.myIsDisabled;
    }
  }
}
