
// Type: BrawlerSource.EnemySpawner
// Assembly: ProjectTorrim, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC8AE86A-8582-48C1-AE0D-A25C0B7D84A4
//

using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

#nullable disable
namespace BrawlerSource
{
  public class EnemySpawner : GameObject
  {
    private TimeSpan myLastSpawnTime;
    public TimeSpan SpawnRate;
    private Queue<BrawlerSource.PathFinding.Node> myEnemyPath;
    private Queue<Enemy> myEnemiesToSpawn;

    public int EnemiesLeft => this.myEnemiesToSpawn.Count;

    public EnemySpawner(GameObject parent, Queue<BrawlerSource.PathFinding.Node> enemyPath)
      : base(parent)
    {
      this.myEnemyPath = enemyPath;
      this.myLastSpawnTime = new TimeSpan(0L);
      this.myEnemiesToSpawn = new Queue<Enemy>();
    }

    public override void Update(GameTime gameTime)
    {
      if (this.Enabled && this.Layer.TotalTime - this.myLastSpawnTime > this.SpawnRate && this.EnemiesLeft > 0)
        this.SpawnEnemy(this.myEnemiesToSpawn.Dequeue());
      base.Update(gameTime);
    }

    public void SpawnEnemy(Enemy e)
    {
      this.SubGameObjects.Add((GameObject) e);
      e.AddToDraw();
      this.myLastSpawnTime = this.Layer.TotalTime;
    }

    public void Pool(Queue<Type> enemiesToSpawn)
    {
      this.myEnemiesToSpawn = new Queue<Enemy>();
      foreach (Type type in enemiesToSpawn)
      {
        Queue<BrawlerSource.PathFinding.Node> nodeQueue = new Queue<BrawlerSource.PathFinding.Node>((IEnumerable<BrawlerSource.PathFinding.Node>) this.myEnemyPath);
        object[] objArray = new object[3]
        {
          (object) this,
          (object) this.myEnemyPath.Peek().Position,
          (object) nodeQueue
        };
        Enemy instance = (Enemy) Activator.CreateInstance(type, objArray);
        instance.LoadContent();
        this.SubGameObjects.Remove((GameObject) instance);
        this.myEnemiesToSpawn.Enqueue(instance);
      }
    }
  }
}
