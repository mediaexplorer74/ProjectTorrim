// Type: BrawlerSource.Input.InputEvents
// Assembly: ProjectTorrim, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC8AE86A-8582-48C1-AE0D-A25C0B7D84A4

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using System;
using System.Collections.Generic;

#nullable disable
namespace BrawlerSource.Input
{
  public class InputEvents : GameObject
  {
    private Dictionary<KeyValuePair<List<Keys>, InputType>, KeyFunction> KeyFunctions = 
            new Dictionary<KeyValuePair<List<Keys>, InputType>, KeyFunction>();

    public HashSet<List<Keys>> KeyList = new HashSet<List<Keys>>();

    public Dictionary<List<Keys>, List<InputType>> KeyInputTypes = 
            new Dictionary<List<Keys>, List<InputType>>();

    private Dictionary<KeyValuePair<List<MouseButtons>, InputType>, MouseFunction> MouseFunctions 
            = new Dictionary<KeyValuePair<List<MouseButtons>, InputType>, MouseFunction>();

    public HashSet<List<MouseButtons>> MouseList = new HashSet<List<MouseButtons>>();
    public Dictionary<List<MouseButtons>, List<InputType>> MouseInputTypes 
            = new Dictionary<List<MouseButtons>, List<InputType>>();

    private Dictionary<KeyValuePair<List<int>, InputType>, TouchFunction> TouchFunctions = 
            new Dictionary<KeyValuePair<List<int>, InputType>, TouchFunction>();
    public HashSet<List<int>> TouchList = new HashSet<List<int>>();
    public Dictionary<List<int>, List<InputType>> TouchInputTypes = 
            new Dictionary<List<int>, List<InputType>>();

    public InputEvents(Layer layer)
      : base(layer)
    {
    }

    public InputEvents(GameObject parent)
      : base(parent)
    {
    }

    public override void Update(GameTime gameTime)
    {
      if (!this.Game.IsActive)
        return;

      // * keyboard handling *
      foreach (List<Keys> key in this.KeyList)
      {
        foreach (InputType inputType in this.KeyInputTypes[key])
        {
          if (this.AreKeysValid(key, inputType))
          {
            KeyFunction keyFunction = this.KeyFunctions[new KeyValuePair<List<Keys>, InputType>(key, inputType)];
            KeyEventArgs e = new KeyEventArgs();
            e.GameTime = gameTime;
            e.Buttons = key;
            e.Type = inputType;
            keyFunction((object) this, e);
          }
        }
      }

      // * mouse  handling *
      foreach (List<MouseButtons> mouse in this.MouseList)
      {
        foreach (InputType inputType in this.MouseInputTypes[mouse])
        {
          if (this.IsMouseValid(mouse, inputType))
          {
            MouseFunction mouseFunction = this.MouseFunctions[new KeyValuePair<List<MouseButtons>, InputType>(mouse, inputType)];
            MouseEventArgs e = new MouseEventArgs();
            e.GameTime = gameTime;
            e.Buttons = mouse;
            e.Type = inputType;
            e.Position = this.Layer.Cursor.Position;
            mouseFunction((object) this, e);
            //TEST: Uncomment the line below to exit after handling the first mouse event
            //return; // Exit after handling the first mouse event
          }
        }
      }

      // * touch-screen handling *
      var touchCollection = TouchPanel.GetState();
      foreach (List<int> touchIds in TouchList)
      {
        foreach (InputType inputType in TouchInputTypes[touchIds])
        {
          if (AreTouchesValid(touchIds, inputType))
          {
            TouchFunction touchFunction = 
                 TouchFunctions[new KeyValuePair<List<int>, InputType>(touchIds, inputType)];
            var firstTouch = touchCollection.Count > 0 ? touchCollection[0] : default;
            TouchEventArgs e = new TouchEventArgs();            
            e.GameTime = gameTime;
            e.TouchIds = touchIds;
            e.Type = inputType;
            e.Position = new Position(firstTouch.Position);
            //e.Position = this.Layer.Cursor.Position;

            touchFunction(this, e);
          }
        }
      }
    }

    public void AddKey(Keys key, InputType inputType, KeyFunction func)
    {
      this.AddKeys(new List<Keys>() { key }, inputType, func);
    }

    public void AddKeys(List<Keys> keys, InputType inputType, KeyFunction func)
    {
      this.KeyList.Add(keys);
      if (!this.KeyInputTypes.ContainsKey(keys))
        this.KeyInputTypes.Add(keys, new List<InputType>());
      this.KeyInputTypes[keys].Add(inputType);
      this.KeyFunctions.Add(new KeyValuePair<List<Keys>, InputType>(keys, inputType), func);
    }

    public void AddMouseButton(MouseButtons button, InputType inputType, MouseFunction func)
    {
      this.AddMouseButtons(new List<MouseButtons>()
      {
        button
      }, inputType, func);
    }

    public void AddMouseButtons(
      List<MouseButtons> buttons,
      InputType inputType,
      MouseFunction func)
    {
      this.MouseList.Add(buttons);
      if (!this.MouseInputTypes.ContainsKey(buttons))
        this.MouseInputTypes.Add(buttons, new List<InputType>());
      this.MouseInputTypes[buttons].Add(inputType);
      this.MouseFunctions.Add(new KeyValuePair<List<MouseButtons>, InputType>(buttons, inputType), func);
    }

    public void AddTouch(int touchId, InputType inputType, TouchFunction func)
    {
      AddTouches(new List<int> { touchId }, inputType, func);
    }

    public void AddTouches(List<int> touchIds, InputType inputType, TouchFunction func)
    {
      TouchList.Add(touchIds);
      if (!TouchInputTypes.ContainsKey(touchIds))
        TouchInputTypes.Add(touchIds, new List<InputType>());
      TouchInputTypes[touchIds].Add(inputType);
      TouchFunctions.Add(new KeyValuePair<List<int>, InputType>(touchIds, inputType), func);
    }

    public bool AreKeysValid(List<Keys> keys, InputType inputType)
    {
      HashSet<bool> boolSet = new HashSet<bool>();
      foreach (Keys key in keys)
      {
        bool flag = false;
        switch (inputType)
        {
          case InputType.Up:
            flag = KeyboardInput.KeyUp(key);
            break;
          case InputType.Down:
            flag = KeyboardInput.KeyDown(key);
            break;
          case InputType.Held:
            flag = KeyboardInput.KeyHeld(key);
            break;
          case InputType.Pressed:
            flag = KeyboardInput.KeyPressed(key);
            break;
          case InputType.Released:
            flag = KeyboardInput.KeyReleased(key);
            break;
        }
        boolSet.Add(flag);
      }
      return !boolSet.Contains(false);
    }

    public bool IsMouseValid(List<MouseButtons> mouseButtons, InputType inputType)
    {
      HashSet<bool> boolSet = new HashSet<bool>();
      foreach (MouseButtons mouseButton in mouseButtons)
      {
        bool flag = false;
        switch (inputType)
        {
          case InputType.Up:
            flag = MouseInput.ButtonUp(mouseButton);
            break;
          case InputType.Down:
            flag = MouseInput.ButtonDown(mouseButton);
            break;
          case InputType.Held:
            flag = MouseInput.ButtonHeld(mouseButton);
            break;
          case InputType.Pressed:
            flag = MouseInput.ButtonPressed(mouseButton);
            break;
          case InputType.Released:
            flag = MouseInput.ButtonReleased(mouseButton);
            break;
        }
        boolSet.Add(flag);
      }
      return !boolSet.Contains(false);
    }

    public bool AreTouchesValid(List<int> touchIds, InputType inputType)
    {
      var touchCollection = TouchPanel.GetState();
      HashSet<bool> boolSet = new HashSet<bool>();
      foreach (int id in touchIds)
      {
        bool flag = false;
        foreach (var touch in touchCollection)
        {
          if (true)//(touch.Id == id)
          {
            //TEMP
            //flag = true; // For now, we assume the touch is valid if it exists in the collection
            switch (inputType)
            {
                case InputType.Pressed:
                  flag = MobileInput.ButtonPressed(touch.State); //touch.State == TouchLocationState.Pressed;
                break;
                case InputType.Released:
                  flag = MobileInput.ButtonReleased(touch.State);//touch.State == TouchLocationState.Released;
                break;
                case InputType.Held:
                  flag = MobileInput.ButtonHeld(touch.State); //touch.State == TouchLocationState.Moved;
                break;
                case InputType.Up:
                  flag = MobileInput.ButtonUp(touch.State);//touch.State == TouchLocationState.Invalid;
                break;
                case InputType.Down:
                  flag = MobileInput.ButtonDown(touch.State);//touch.State == TouchLocationState.Pressed || touch.State == TouchLocationState.Moved;
                break;
                //case InputType.Moved:
                //  flag = true;//touch.State == TouchLocationState.Pressed ||
                            //touch.State == TouchLocationState.Moved;
                //break;

                }
            }
        }
        boolSet.Add(flag);
      }
      return !boolSet.Contains(false);
    }

   }
}
