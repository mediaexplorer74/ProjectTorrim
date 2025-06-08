
// Type: BrawlerSource.Input.MobileInput
// Assembly: ProjectTorrim, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC8AE86A-8582-48C1-AE0D-A25C0B7D84A4
//

using Microsoft.Xna.Framework.Input.Touch;
using System;
using System.Collections.Generic;

#nullable disable
namespace BrawlerSource.Input
{
  public static class MobileInput
  {
    private static TouchCollection TouchState;
    private static TouchCollection prevTouchState = TouchPanel.GetState();
    private static TouchLocation lastTouchLocation;
    //private static Dictionary<TouchLocationState, Queue<TouchLocation>> myTouchState = new Dictionary<TouchLocationState, Queue<TouchLocation>>();

    public static Position GetPosition()
    {
        //TouchCollection 
        /*TouchState = TouchPanel.GetState();

        if (TouchState.Count > 0)
        {
            lastTouchLocation = TouchState[TouchState.Count - 1];
            return new Position(lastTouchLocation.Position);
        }

        return new Position(new Microsoft.Xna.Framework.Vector2(0,0));*/
        Position pos = new Position(new Microsoft.Xna.Framework.Vector2(0, 0));

        if (TouchState.Count > 0)
            pos = new Position(TouchState[0].Position);

        return pos;
    }

    /*public static TouchLocation? TouchPerformed(TouchLocationState type)
    {
      TouchLocation? nullable = new TouchLocation?();
      //if (MobileInput.myTouchState.ContainsKey(type))
      //  nullable = new TouchLocation?(MobileInput.myTouchState[type].Peek());
      return nullable;
    }*/

    public static bool ButtonPressed(TouchLocationState type)
    {
        //TouchState = TouchPanel.GetState();
        if (TouchState.Count > 0)
            return true;

        return false;
    }

    public static bool ButtonReleased(TouchLocationState type)
    {
        //TouchState = TouchPanel.GetState();
        if (TouchState.Count == 0)
            return true;

        return false;
    }

    public static bool ButtonHeld(TouchLocationState type)
    {
       //TouchState = TouchPanel.GetState();
       if (TouchState.Count > 0 && prevTouchState.Count > 0)
         return true;

       return false;
    }

    public static bool ButtonUp(TouchLocationState type)
    {
       if (TouchState.Count == 0 && prevTouchState.Count > 0)
            return true;

        return false;
    }

    public static bool ButtonDown(TouchLocationState type)
    {
        if (TouchState.Count > 0 && prevTouchState.Count == 0)
            return true;

        return false;
    }

    public static void Update()
    {
      //MobileInput.myTouchState = new Dictionary<TouchLocationState, Queue<TouchLocation>>();
      //TouchCollection TouchState = TouchPanel.GetState();
      //foreach (TouchLocation touchLocation in TouchState)
      //{
      //  if (!MobileInput.myTouchState.ContainsKey(touchLocation.State))
      //    MobileInput.myTouchState.Add(touchLocation.State, new Queue<TouchLocation>());
      //  MobileInput.myTouchState[touchLocation.State].Enqueue(touchLocation);
      //}
      MobileInput.prevTouchState = MobileInput.TouchState;
      MobileInput.TouchState = TouchPanel.GetState();
    }
 
  }
}

// Type: BrawlerSource.Input.MouseInput
// Assembly: ProjectTorrim, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC8AE86A-8582-48C1-AE0D-A25C0B7D84A4
//
/*
using Microsoft.Xna.Framework.Input.Touch;

#nullable disable
namespace BrawlerSource.Input
{
    public static class MobileInput
    {
        private static TouchCollection prevTouchState;
        private static TouchCollection touchState;

        public static Position GetPosition()
        {
            Position pos = null;
            try
            {
                pos = new Position(MobileInput.touchState[0].Position);
            }
            catch { }

            return pos;
        }

        public static bool ButtonUp(MobileButtons touchButton)
        {
            return !MobileInput.IsButtonDown(MobileInput.mouseState, mouseButton);
        }

        public static bool ButtonDown(MouseButtons mouseButton)
        {
            return MobileInput.IsButtonDown(MobileInput.mouseState, mouseButton);
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
            ButtonState buttonState = (ButtonState)0;
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
*/