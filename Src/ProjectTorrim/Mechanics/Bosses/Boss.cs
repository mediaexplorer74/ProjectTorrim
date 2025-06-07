
// Type: BrawlerSource.Mechanics.Bosses.Boss
// Assembly: ProjectTorrim, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC8AE86A-8582-48C1-AE0D-A25C0B7D84A4
//

using BrawlerSource.UI;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

#nullable disable
namespace BrawlerSource.Mechanics.Bosses
{
  public class Boss : Enemy
  {
    protected PercentageBar myHealthbar;

    public Boss(Layer layer, Position position, int health, int speed, Queue<BrawlerSource.PathFinding.Node> path)
      : base(layer, position, health, speed, path)
    {
    }

    public Boss(GameObject parent, Position position, int health, int speed, Queue<BrawlerSource.PathFinding.Node> path)
      : base(parent, position, health, speed, path)
    {
    }

    protected override void Construct(Position position, int health, int speed, Queue<BrawlerSource.PathFinding.Node> path)
    {
      base.Construct(position, health, speed, path);
      this.Reward = this.Health * 2;
      this.Depth = 0.8f;
      PercentageBar percentageBar = new PercentageBar((GameObject) this, this.Position + new Position(0.0f, -80f), new Position(128f, 8f), this.Health, Align.Left);
      percentageBar.Depth = this.Depth;
      this.myHealthbar = percentageBar;
      this.myHealthbar.Value = this.Health;
    }

    public override void Update(GameTime gameTime)
    {
      this.myHealthbar.Position = this.Position + new Position(0.0f, -80f);
      this.myHealthbar.Value = this.Health;
      base.Update(gameTime);
    }
  }
}
