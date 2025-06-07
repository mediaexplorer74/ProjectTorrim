
// Type: BrawlerSource.Framework.Timer
// Assembly: ProjectTorrim, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC8AE86A-8582-48C1-AE0D-A25C0B7D84A4
//

using System.Collections.Generic;
using System.Diagnostics;

#nullable disable
namespace BrawlerSource.Framework
{
  public static class Timer
  {
    public static List<Dictionary<string, List<long>>> allTimes = new List<Dictionary<string, List<long>>>();
    public static Dictionary<string, List<long>> times = new Dictionary<string, List<long>>();
    public static Dictionary<string, Stack<Stopwatch>> stopwatches = new Dictionary<string, Stack<Stopwatch>>();

    public static void StartTimer(string name)
    {
      Stack<Stopwatch> stopwatchStack;
      if (!Timer.stopwatches.TryGetValue(name, out stopwatchStack))
        Timer.stopwatches.Add(name, stopwatchStack = new Stack<Stopwatch>());
      Stopwatch stopwatch = new Stopwatch();
      stopwatchStack.Push(stopwatch);
      stopwatch.Start();
    }

    public static void FinishTimer(string name)
    {
      Stopwatch stopwatch = Timer.stopwatches[name].Pop();
      stopwatch.Stop();
      List<long> longList;
      if (!Timer.times.TryGetValue(name, out longList))
        Timer.times.Add(name, longList = new List<long>());
      longList.Add(stopwatch.ElapsedTicks);
    }

    public static void Reset()
    {
      Timer.allTimes.Add(new Dictionary<string, List<long>>((IDictionary<string, List<long>>) Timer.times));
      Timer.times.Clear();
    }

    public static void Write()
    {
      foreach (Dictionary<string, List<long>> allTime in Timer.allTimes)
      {
        foreach (KeyValuePair<string, List<long>> keyValuePair in allTime)
        {
          long num1 = 0;
          foreach (long num2 in keyValuePair.Value)
            num1 += num2;
        }
      }
    }
  }
}
