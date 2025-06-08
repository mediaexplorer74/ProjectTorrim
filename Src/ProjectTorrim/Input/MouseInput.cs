
// Type: BrawlerSource.Input.MouseInput
// Assembly: ProjectTorrim, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC8AE86A-8582-48C1-AE0D-A25C0B7D84A4
//

using Microsoft.Xna.Framework.Input;

#nullable disable
namespace BrawlerSource.Input
{
  public static class MouseInput
  {
    private static MouseState prevMouseState;
    private static MouseState mouseState;

    public static Position GetPosition()
    {     
        return new Position(MouseInput.mouseState.Position);     
    }

    public static bool ButtonUp(MouseButtons mouseButton)
    {
      return !MouseInput.IsButtonDown(MouseInput.mouseState, mouseButton);
    }

    public static bool ButtonDown(MouseButtons mouseButton)
    {
      return MouseInput.IsButtonDown(MouseInput.mouseState, mouseButton);
    }

    public static bool ButtonHeld(MouseButtons mouseButton)
    {
      return MouseInput.IsButtonDown(MouseInput.mouseState, mouseButton) && MouseInput.IsButtonDown(MouseInput.prevMouseState, mouseButton);
    }

    public static bool ButtonPressed(MouseButtons mouseButton)
    {
      return MouseInput.IsButtonDown(MouseInput.mouseState, mouseButton) && !MouseInput.IsButtonDown(MouseInput.prevMouseState, mouseButton);
    }

    public static bool ButtonReleased(MouseButtons mouseButton)
    {
      return !MouseInput.IsButtonDown(MouseInput.mouseState, mouseButton) && MouseInput.IsButtonDown(MouseInput.prevMouseState, mouseButton);
    }

    private static bool IsButtonDown(MouseState ms, MouseButtons mb)
    {
      return MouseInput.ResolveMouseButtonState(ms, mb) == ButtonState.Pressed;
    }

    private static ButtonState ResolveMouseButtonState(MouseState ms, MouseButtons mb)
    {
      ButtonState buttonState = (ButtonState) 0;
      switch (mb)
      {
        case MouseButtons.Left:
          buttonState = ms.LeftButton;
          break;
        case MouseButtons.Right:
          buttonState = ms.RightButton;
          break;
        case MouseButtons.Middle:
          buttonState = ms.MiddleButton;
          break;
      }
      return buttonState;
    }

    public static void Update()
    {
      MouseInput.prevMouseState = MouseInput.mouseState;
      MouseInput.mouseState = Mouse.GetState();
    }
  }
}
