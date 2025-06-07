
// Type: BrawlerSource.Input.KeyboardInput
// Assembly: ProjectTorrim, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC8AE86A-8582-48C1-AE0D-A25C0B7D84A4
//

using Microsoft.Xna.Framework.Input;

#nullable disable
namespace BrawlerSource.Input
{
  public static class KeyboardInput
  {
    private static KeyboardState prevKeyboardState;
    private static KeyboardState keyboardState;

    public static bool KeyUp(Keys key)
    {
      return KeyboardInput.keyboardState.IsKeyUp(key);
    }

    public static bool KeyDown(Keys key)
    {
      return KeyboardInput.keyboardState.IsKeyDown(key);
    }

    public static bool KeyHeld(Keys key)
    {
      return KeyboardInput.keyboardState.IsKeyDown(key)
                && KeyboardInput.prevKeyboardState.IsKeyDown(key);
    }

    public static bool KeyPressed(Keys key)
    {
      return KeyboardInput.keyboardState.IsKeyDown(key) 
                && KeyboardInput.prevKeyboardState.IsKeyUp(key);
    }

    public static bool KeyReleased(Keys key)
    {
      return KeyboardInput.keyboardState.IsKeyUp(key) 
                && KeyboardInput.prevKeyboardState.IsKeyDown(key);
    }

    public static void Update()
    {
      KeyboardInput.prevKeyboardState = KeyboardInput.keyboardState;
      KeyboardInput.keyboardState = Keyboard.GetState();
    }
  }
}
