
// Type: BrawlerSource.SupportSpawner
// Assembly: ProjectTorrim, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC8AE86A-8582-48C1-AE0D-A25C0B7D84A4
//

using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

#nullable disable
namespace BrawlerSource
{
  public class SupportSpawner : EnemySpawner
  {
    public List<Type> SupportEnemies;
    private TimeSpan? myLastGroupTime;
    public int GroupSize;
    private TimeSpan myWaitRate;

    public SupportSpawner(GameObject parent, Queue<BrawlerSource.PathFinding.Node> enemyPath)
      : base(parent, enemyPath)
    {
      this.SupportEnemies = new List<Type>();
    }

    public override void Update(GameTime gameTime)
    {
      if (this.EnemiesLeft == 0 && !this.myLastGroupTime.HasValue)
      {
        this.myLastGroupTime = new TimeSpan?(this.Layer.TotalTime);
        this.myWaitRate = TimeSpan.FromMilliseconds(this.SpawnRate.TotalMilliseconds * (double) this.GroupSize * 2.0);
      }
      if (this.Enabled && this.myLastGroupTime.HasValue)
      {
        TimeSpan totalTime = this.Layer.TotalTime;
        TimeSpan? lastGroupTime = this.myLastGroupTime;
        TimeSpan? nullable = lastGroupTime.HasValue ? new TimeSpan?(totalTime - lastGroupTime.GetValueOrDefault()) : new TimeSpan?();
        TimeSpan waitRate = this.myWaitRate;
        if ((nullable.HasValue ? (nullable.GetValueOrDefault() > waitRate ? 1 : 0) : 0) != 0)
        {
          this.CreateSupportGroup();
          this.myLastGroupTime = new TimeSpan?();
          this.myWaitRate = TimeSpan.Zero;
        }
      }
      base.Update(gameTime);
    }

    public void CreateSupportGroup()
    {
      if (this.SupportEnemies.Count <= 0)
        return;
      Queue<Type> enemiesToSpawn = new Queue<Type>();
      Type supportEnemy = this.SupportEnemies[this.Random.Next(0, this.SupportEnemies.Count)];
      for (int index = 0; index < this.GroupSize; ++index)
        enemiesToSpawn.Enqueue(supportEnemy);
      this.Pool(enemiesToSpawn);
    }
  }
}
