
// Type: BrawlerSourceWindows.Program
// Assembly: ProjectTorrim, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC8AE86A-8582-48C1-AE0D-A25C0B7D84A4
//

using BrawlerSource;
using System;

#nullable disable
namespace BrawlerSourceWindows
{
  public static class Program
  {
    [STAThread]
    private static void Main(string[] args)
    {
      using (BrawlerGame brawlerGame = new BrawlerGame(args))
        brawlerGame.Run();
    }
  }
}
