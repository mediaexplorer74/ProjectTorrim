
// Type: BrawlerSource.Mechanics.Wave
// Assembly: ProjectTorrim, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC8AE86A-8582-48C1-AE0D-A25C0B7D84A4
//

using System;
using System.Collections.Generic;

#nullable disable
namespace BrawlerSource.Mechanics
{
  public class Wave
  {
    public TimeSpan EnemySpawnRate;
    public TimeSpan SupportSpawnRate;
    public int SupportGroupSize;
    public Queue<Type> ToSpawn;
    public List<Type> ToSupport;
  }
}
