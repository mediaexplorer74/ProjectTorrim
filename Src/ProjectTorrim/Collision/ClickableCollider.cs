
// Type: BrawlerSource.Collision.ClickableCollider
// Assembly: ProjectTorrim, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC8AE86A-8582-48C1-AE0D-A25C0B7D84A4
//

using BrawlerSource.Input;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

#nullable disable
namespace BrawlerSource.Collision
{
  public class ClickableCollider : Collider
  {
    private InputEvents myInput;
    private Dictionary<KeyValuePair<List<MouseButtons>, InputType>, MouseFunction> myMouseFunctions = new Dictionary<KeyValuePair<List<MouseButtons>, InputType>, MouseFunction>();
    private Dictionary<List<MouseButtons>, InputType> myPreviousActiveButtons;
    private Dictionary<List<MouseButtons>, InputType> myActiveButtons;

    public ClickableCollider(GameObject parent)
      : base(parent)
    {
      this.myPreviousActiveButtons = new Dictionary<List<MouseButtons>, InputType>();
      this.myActiveButtons = new Dictionary<List<MouseButtons>, InputType>();
      this.myInput = new InputEvents(this.Parent);
      this.AddEvent(typeof (CursorCollider), TouchType.Touching, new CollisionFunc(this.Cursor_Touch));
      this.AddEvent(typeof (CursorCollider), TouchType.TouchStart, new CollisionFunc(this.Cursor_Touch));
    }

    public void AddMouseInput(MouseButtons button, InputType type, MouseFunction func)
    {
      this.AddMouseInput(new List<MouseButtons>() { button }, type, func);
    }

    public void AddMouseInput(List<MouseButtons> buttons, InputType type, MouseFunction func)
    {
      List<InputType> inputTypeList;
      if (!this.myInput.MouseInputTypes.TryGetValue(buttons, out inputTypeList) || !inputTypeList.Contains(type))
        this.myInput.AddMouseButtons(buttons, type, new MouseFunction(this.Mouse_Click));
      this.myMouseFunctions.Add(new KeyValuePair<List<MouseButtons>, InputType>(buttons, type), func);
    }

    public void Mouse_Click(object sender, MouseEventArgs e)
    {
      this.myActiveButtons.Add(e.Buttons, e.Type);
    }

    public void Cursor_Touch(object sender, CollisionEventArgs e)
    {
      foreach (List<MouseButtons> key in this.myPreviousActiveButtons.Keys)
      {
        InputType previousActiveButton = this.myPreviousActiveButtons[key];
        MouseFunction mouseFunction = this.myMouseFunctions[new KeyValuePair<List<MouseButtons>, InputType>(key, previousActiveButton)];
        if (mouseFunction != null)
        {
          MouseEventArgs e1 = new MouseEventArgs();
          e1.GameTime = e.GameTime;
          e1.Buttons = key;
          e1.Type = previousActiveButton;
          e1.Position = this.Layer.Cursor.Position;
          mouseFunction((object) this, e1);
        }
      }
    }

    public override void Update(GameTime gameTime)
    {
      this.myPreviousActiveButtons = this.myActiveButtons;
      this.myActiveButtons = new Dictionary<List<MouseButtons>, InputType>();
      base.Update(gameTime);
    }
  }
}
