
// Type: BrawlerSource.Menu.InstructionsLayer
// Assembly: ProjectTorrim, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC8AE86A-8582-48C1-AE0D-A25C0B7D84A4
//

using BrawlerSource.Graphics;
using BrawlerSource.Input;
using BrawlerSource.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

#nullable disable
namespace BrawlerSource.Menu
{
  public class InstructionsLayer : Layer
  {
    public InstructionsLayer(Level level, int index)
      : base(level, index)
    {
      this.DrawStates = GameStates.Pause | GameStates.Live;
    }

    public override void Initilize()
    {
      base.Initilize();
      new InputEvents((Layer) this).AddKey((Keys) 27, InputType.Pressed, new KeyFunction(this.Back));
      Sprite sprite = new Sprite((Layer) this);
      sprite.Sequence = new Sequence()
      {
        TexturePath = "Menu_Background",
        Width = 256,
        Height = 256
      };
      sprite.Position = new Position(0.0f, 0.0f);
      sprite.Depth = 0.0f;
      sprite.Scale = Vector2.Divide(new Vector2(384f, 288f), 256f);
      sprite.Colour = new Color(Color.White, 0.5f);
      sprite.AddToDraw();
      Text text1 = new Text((Layer) this);
      text1.String = "Instructions";
      text1.FontPath = "Font";
      text1.Colour = new Color(247, 209, 0);
      text1.Depth = 1f;
      text1.Scale = new Vector2(1f);
      text1.Position = new Position(0.0f, -100f);
      text1.AddToDraw();
      Text text2 = new Text((Layer) this);
      text2.String = "Defend the camp from the evil ones";
      text2.FontPath = "Font";
      text2.Colour = Color.White;
      text2.Depth = 1f;
      text2.Scale = new Vector2(0.5f);
      text2.Position = new Position(0.0f, -50f);
      text2.AddToDraw();
      Text text3 = new Text((Layer) this);
      text3.String = "Buy towers from the menu at the bottom";
      text3.FontPath = "Font";
      text3.Colour = Color.White;
      text3.Depth = 1f;
      text3.Scale = new Vector2(0.5f);
      text3.Position = new Position(0.0f, 0.0f);
      text3.AddToDraw();
      Text text4 = new Text((Layer) this);
      text4.String = "Click to drag selected towers";
      text4.FontPath = "Font";
      text4.Colour = Color.White;
      text4.Depth = 1f;
      text4.Scale = new Vector2(0.5f);
      text4.Position = new Position(0.0f, 50f);
      text4.AddToDraw();
      Button button = new Button((Layer) this, Align.Centre, 
          new Position(0.0f, 100f), new Position(256f, 40f), 
          new MouseFunction(this.Back), new TouchFunction(this.Back));
      button.SetText("Back");
      button.SetTextScale(new Vector2(1f));
      button.AddToDraw();
    }

    public void Back(object sender, BrawlerEventArgs e) => this.Back();

    public void Back()
    {
      ((LevelMenu) this.Level).MainMenu.IsEnabled = true;
      this.IsEnabled = false;
    }
  }
}
