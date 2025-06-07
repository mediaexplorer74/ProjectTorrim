
// Type: BrawlerSource.Mechanics.Projectiles.Homing
// Assembly: ProjectTorrim, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC8AE86A-8582-48C1-AE0D-A25C0B7D84A4
//

using BrawlerSource.Physics;
using Microsoft.Xna.Framework;

#nullable disable
namespace BrawlerSource.Mechanics.Projectiles
{
  public class Homing : Projectile
  {
    public Homing(GameObject parent, ProjectileInfo info)
      : base(parent, info)
    {
    }

    public Homing(Layer layer, ProjectileInfo info)
      : base(layer, info)
    {
    }

    public override void Update(GameTime gameTime)
    {
      base.Update(gameTime);
      if (this.Target == null)
        return;
      Physics.Point point = this.myPoint;
      Vector2 zero = (this.Target.Position - this.Position).ToVector2().NormalizeToZero();
      Vector2 nextVelocity = this.myPoint.NextVelocity;
      double num = (double) nextVelocity.Length();
      Vector2 vector2 = Vector2.Multiply(zero, (float) num);
      point.Velocity = vector2;
    }
  }
}
