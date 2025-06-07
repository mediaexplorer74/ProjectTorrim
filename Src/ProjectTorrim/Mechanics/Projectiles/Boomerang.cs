
// Type: BrawlerSource.Mechanics.Projectiles.Boomerang
// Assembly: ProjectTorrim, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC8AE86A-8582-48C1-AE0D-A25C0B7D84A4
//

using BrawlerSource.Physics;
using Microsoft.Xna.Framework;

#nullable disable
namespace BrawlerSource.Mechanics.Projectiles
{
  public class Boomerang : Projectile
  {
    private bool hasFlipped;

    public Boomerang(GameObject parent, ProjectileInfo info)
      : base(parent, info)
    {
    }

    public Boomerang(Layer layer, ProjectileInfo info)
      : base(layer, info)
    {
    }

    protected override void Construct(ProjectileInfo info)
    {
      base.Construct(info);
      this.hasFlipped = false;
    }

    public override void Start(
      GameTime gameTime,
      Position position,
      Vector2 velocity,
      float distance,
      Enemy target = null)
    {
      base.Start(gameTime, position, velocity, distance, target);
      this.hasFlipped = false;
    }

    public override void DistanceReached()
    {
      if (!this.hasFlipped)
      {
        this.myEndTime = this.myEndTime + this.myEndSpan;
        Physics.Point point = this.myPoint;
        point.Velocity = Vector2.Multiply(point.Velocity, -1f);
        this.hasFlipped = true;
      }
      else
        base.DistanceReached();
    }
  }
}
