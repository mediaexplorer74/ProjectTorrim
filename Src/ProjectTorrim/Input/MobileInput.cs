
// Type: BrawlerSource.Input.MobileInput
// Assembly: ProjectTorrim, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC8AE86A-8582-48C1-AE0D-A25C0B7D84A4
//

using Microsoft.Xna.Framework.Input.Touch;
using System.Collections.Generic;

#nullable disable
namespace BrawlerSource.Input
{
  public static class MobileInput
  {
    private static Dictionary<TouchLocationState, Queue<TouchLocation>> myTouchState = new Dictionary<TouchLocationState, Queue<TouchLocation>>();

    public static TouchLocation? TouchPerformed(TouchLocationState type)
    {
      TouchLocation? nullable = new TouchLocation?();
      if (MobileInput.myTouchState.ContainsKey(type))
        nullable = new TouchLocation?(MobileInput.myTouchState[type].Peek());
      return nullable;
    }

    public static void Update()
    {
      MobileInput.myTouchState = new Dictionary<TouchLocationState, Queue<TouchLocation>>();
      TouchCollection state = TouchPanel.GetState();
      foreach (TouchLocation touchLocation in state)
      {
        if (!MobileInput.myTouchState.ContainsKey(touchLocation.State))
          MobileInput.myTouchState.Add(touchLocation.State, new Queue<TouchLocation>());
        MobileInput.myTouchState[touchLocation.State].Enqueue(touchLocation);
      }
    }
  }
}
