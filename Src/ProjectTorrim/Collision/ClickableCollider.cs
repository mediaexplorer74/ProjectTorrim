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

    // Mouse support fields
    private Dictionary<KeyValuePair<List<MouseButtons>, InputType>, MouseFunction> myMouseFunctions 
        = new Dictionary<KeyValuePair<List<MouseButtons>, InputType>, MouseFunction>();
    private Dictionary<List<MouseButtons>, InputType> myPreviousActiveButtons;
    private Dictionary<List<MouseButtons>, InputType> myActiveButtons;

    // Touch support fields
    private Dictionary<KeyValuePair<List<int>, InputType>, TouchFunction> myTouchFunctions 
            = new Dictionary<KeyValuePair<List<int>, InputType>, TouchFunction>();
    private Dictionary<List<int>, InputType> myPreviousActiveTouches;
    private Dictionary<List<int>, InputType> myActiveTouches;

    public ClickableCollider(GameObject parent)
      : base(parent)
    {
      // Mouse input initialization
      this.myPreviousActiveButtons = new Dictionary<List<MouseButtons>, InputType>();
      this.myActiveButtons = new Dictionary<List<MouseButtons>, InputType>();
      
      // Touch support initialization
      this.myPreviousActiveTouches = new Dictionary<List<int>, InputType>();
      this.myActiveTouches = new Dictionary<List<int>, InputType>();

      this.myInput = new InputEvents(this.Parent);

      this.AddEvent(typeof(CursorCollider), TouchType.Touching, new CollisionFunc(this.Cursor_Touch));
      this.AddEvent(typeof(CursorCollider), TouchType.TouchStart, new CollisionFunc(this.Cursor_Touch));
    }

    // Touch mouse registration
    public void AddMouseInput(MouseButtons button, InputType type, MouseFunction mousefunc)
    {
      this.AddMouseInput(new List<MouseButtons>() { button }, type, mousefunc);
    }

    public void AddMouseInput(List<MouseButtons> buttons, InputType type, MouseFunction mousefunc)
    {
      List<InputType> inputTypeList;

      if (!this.myInput.MouseInputTypes.TryGetValue(buttons, out inputTypeList) 
                || !inputTypeList.Contains(type))
        this.myInput.AddMouseButtons(buttons, type, new MouseFunction(this.Mouse_Click));

      this.myMouseFunctions.Add(new KeyValuePair<List<MouseButtons>, InputType>(buttons, type), mousefunc);
    }

    // Touch input registration
    public void AddTouchInput(int touchId, InputType type, TouchFunction touchfunc)
    {
      this.AddTouchInput(new List<int> { touchId }, type, touchfunc);
    }

    public void AddTouchInput(List<int> touchIds, InputType type, TouchFunction touchfunc)
    {
      List<InputType> inputTypeList;

      if (!this.myInput.TouchInputTypes.TryGetValue(touchIds, out inputTypeList) 
                || !inputTypeList.Contains(type))
        this.myInput.AddTouches(touchIds, type, new TouchFunction(this.Touch_Tap));

      this.myTouchFunctions.Add(new KeyValuePair<List<int>, InputType>(touchIds, type), touchfunc);
    }

    // Mouse event handler
    public void Mouse_Click(object sender, MouseEventArgs e)
    {
      this.myActiveButtons.Add(e.Buttons, e.Type);
    }

    // Touch event handler
    public void Touch_Tap(object sender, TouchEventArgs e)
    {
      this.myActiveTouches.Add(e.TouchIds, e.Type);
    }

    public void Cursor_Touch(object sender, CollisionEventArgs e)
    {
      // Mouse
      foreach (List<MouseButtons> key in this.myPreviousActiveButtons.Keys)
      {
        InputType previousActiveButton = this.myPreviousActiveButtons[key];
        MouseFunction mouseFunction = 
                    this.myMouseFunctions[new KeyValuePair<List<MouseButtons>, InputType>(key, 
                    previousActiveButton)];
        if (mouseFunction != null)
        {
          MouseEventArgs e1 = new MouseEventArgs();
          e1.GameTime = e.GameTime;
          e1.Buttons = key;
          e1.Type = previousActiveButton;
          e1.Position = this.Layer.Cursor.Position;
          mouseFunction((object)this, e1);
          //TEST: Uncomment the line below to exit after handling the first mouse event
          //return; // Exit after handling the first mouse event
        }
      }

      // Touch
      foreach (List<int> key in this.myPreviousActiveTouches.Keys)
      {
        InputType previousActiveTouch = this.myPreviousActiveTouches[key];
        TouchFunction touchFunction = this.myTouchFunctions[new KeyValuePair<List<int>, 
            InputType>(key, previousActiveTouch)];
        if (touchFunction != null)
        {
          TouchEventArgs e1 = new TouchEventArgs();
          e1.GameTime = e.GameTime;
          e1.TouchIds = key;
          e1.Type = previousActiveTouch;
          e1.Position = this.Layer.Cursor.Position;
          touchFunction((object)this, e1);
        }
      }
    }

    public override void Update(GameTime gameTime)
    {
      // mouse input update 
      this.myPreviousActiveButtons = this.myActiveButtons;
      this.myActiveButtons = new Dictionary<List<MouseButtons>, InputType>();

      // Touch support update
      this.myPreviousActiveTouches = this.myActiveTouches;
      this.myActiveTouches = new Dictionary<List<int>, InputType>();

      base.Update(gameTime);
    }
  }
}
