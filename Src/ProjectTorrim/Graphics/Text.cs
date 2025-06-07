
// Type: BrawlerSource.Graphics.Text
// Assembly: ProjectTorrim, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC8AE86A-8582-48C1-AE0D-A25C0B7D84A4
//

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#nullable disable
namespace BrawlerSource.Graphics
{
  public class Text : DrawableGameObject
  {
    public SpriteFont Font;
    public string FontPath;
    public string myString;

    public string String
    {
      get => this.myString;
      set
      {
        this.myString = value;
        if (this.Font == null || string.IsNullOrEmpty(this.String))
          return;
        this.Origin = Vector2.Divide(this.Font.MeasureString(this.myString), 2f);
      }
    }

    public Text(Layer layer)
      : base(layer)
    {
    }

    public Text(GameObject parent)
      : base(parent)
    {
    }

    public override void LoadContent()
    {
      this.Font = this.Game.Content.Load<SpriteFont>(this.FontPath);
      this.String = this.String;
      base.LoadContent();
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
      if (string.IsNullOrEmpty(this.String) || this.IsHidden)
        return;
      spriteBatch.DrawString(this.Font, this.String, this.myPosition, this.Colour, this.Rotation, this.Origin, this.myScale, this.Effect, this.Depth);
    }
  }
}
